using BankLibrary.Models;
using BankLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace BankWeb.Pages.Customers
{
    [BindProperties]
    [Authorize(Roles = "Admin, Cashier")]
    public class NewModel : PageModel
    {
        private readonly ICustomerService _customerService;
        public NewModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [MaxLength(20)]
        [Required]
        public string Givenname { get; set; }
        [MaxLength(20)]
        [Required]
        public string Surname { get; set; }
        [StringLength(100)]
        [Required]
        public string Streetaddress { get; set; }
        [Required]
        [Range(1, 4, ErrorMessage = "Please choose a country")]
        public Country Country { get; set; }
        [StringLength(50)]
        [Required]
        public string City { get; set; }
        [StringLength(100)]
        [Required]
        [EmailAddress]
        public string Emailaddress { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "Please choose a gender")]
        public Gender Gender { get; set; }
        [Required]
        public string Zipcode { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Telephonenumber { get; set; }
        [RegularExpression(@"^\d{6}[A-Z-]\d{3}[a-zA-Z0-9]$", ErrorMessage = "Enter a ten-digit national id, like so: XXXXXX-XXXX")]
        public string? NationalId { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (_customerService.ValidateEmail(Emailaddress) || _customerService.ValidateNationalId(NationalId))
                {
                    if (_customerService.ValidateEmail(Emailaddress))
                        ModelState.AddModelError("Emailaddress", "Email address is already in use, please enter another one.");
                    if (_customerService.ValidateNationalId(NationalId))
                        ModelState.AddModelError("NationalId", "National Id is already in use, please enter another one.");
                    return Page();
                }
                var customer = new Customer
                {
                    IsActive = true,
                    Givenname = Givenname,
                    Surname = Surname,
                    Streetaddress = Streetaddress,
                    Country = CountryMapper.GetCountry(Country),
                    CountryCode = CountryMapper.GetCountryCode(Country),
                    City = City,
                    Birthday = Birthday,
                    Telephonenumber = Telephonenumber ?? string.Empty,
                    Telephonecountrycode = CountryMapper.GetTelephoneCode(CountryMapper.GetCountry(Country)),
                    NationalId = NationalId ?? string.Empty,
                    Gender = _customerService.GetGender(Gender),
                    Emailaddress = Emailaddress ?? string.Empty,
                    Zipcode = Zipcode
                };
                _customerService.AddCustomer(customer);
                TempData["Message"] = "Customer was created successfully!";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
