using BankLibrary.Models;
using BankLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankWeb.Pages.Customers
{
    [BindProperties]
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
        public List<SelectListItem> Countries { get; set; }
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
        public List<SelectListItem> Genders { get; set; }
        [Required]
        public string Zipcode { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Telephonenumber { get; set; }
        public string? NationalId { get; set; }
        public void OnGet()
        {
            Genders = _customerService.GetGenderList();
            Countries = _customerService.GetCountryList();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    Givenname = Givenname,
                    Surname = Surname,
                    Streetaddress = Streetaddress,
                    Country = _customerService.GetCountry(Country),
                    CountryCode = _customerService.GetCountryCode(Country),
                    City = City,
                    Birthday = Birthday,
                    Telephonenumber = Telephonenumber,
                    NationalId = NationalId,
                    Gender = _customerService.GetGender(Gender),
                    Emailaddress = Emailaddress,
                    Zipcode = Zipcode
                };
                _customerService.AddCustomer(customer);
                return RedirectToPage("Index");
            }
            Genders = _customerService.GetGenderList();
            Countries = _customerService.GetCountryList();
            return Page();
        }
    }
}
