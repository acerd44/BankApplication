using BankLibrary.Infrastructure;
using BankLibrary.Models;
using BankLibrary.Models.Migrations;
using BankLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankWeb.Pages.Customers
{
    [BindProperties]
    [Authorize(Roles = "Admin, Cashier")]
    public class EditModel : PageModel
    {
        private readonly ICustomerService _customerService;
        public EditModel(ICustomerService customerService)
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
        public string? Emailaddress { get; set; }
        [Required]
        [Range(1, 3, ErrorMessage = "Please choose a gender")]
        public Gender Gender { get; set; }
        [Required]
        public string Zipcode { get; set; }
        public DateOnly? Birthday { get; set; }
        public bool IsActive { get; set; }
        public string? Telephonenumber { get; set; }
        [RegularExpression(@"^\d{6}[A-Z-]\d{3}[a-zA-Z0-9]$", ErrorMessage = "Enter a ten-digit national id, like so: XXXXXX-XXXX")]
        public string? NationalId { get; set; }
        public int CustomerId { get; set; }
        public void OnGet(int customerId)
        {
            CustomerId = customerId;
            var customer = _customerService.GetCustomer(customerId);
            IsActive = customer.IsActive;
            Givenname = customer.Givenname;
            Surname = customer.Surname;
            Streetaddress = customer.Streetaddress;
            Country = (Country)Enum.Parse(typeof(Country), customer.Country);
            City = customer.City;
            Emailaddress = customer.Emailaddress;
            Gender = (Gender)Enum.Parse(typeof(Gender), customer.Gender, true);
            Zipcode = customer.Zipcode;
            Birthday = customer.Birthday;
            Telephonenumber = customer.Telephonenumber;
            NationalId = customer.NationalId;
        }

        public IActionResult OnPost(int customerId, bool delete, bool activate)
        {
            if (delete)
            {
                _customerService.Delete(customerId);
                TempData["Message"] = "Delete was successful!";
                return RedirectToPage("Index");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var customer = _customerService.GetCustomer(customerId);
                    if (customer.NationalId != NationalId || customer.Emailaddress != Emailaddress)
                    {
                        bool proceed = true;
                        if (_customerService.ValidateEmail(Emailaddress) && customer.Emailaddress != Emailaddress)
                        {
                            proceed = false;
                            ModelState.AddModelError("Emailaddress", "Email address is already in use, please enter another one.");
                        }
                        if (_customerService.ValidateNationalId(NationalId) && customer.NationalId != NationalId)
                        {
                            proceed = false;
                            ModelState.AddModelError("NationalId", "National Id is already in use, please enter another one.");
                        }
                        if (!proceed)
                        {
                            return Page();
                        }
                    }
                    customer.Givenname = Givenname;
                    customer.Surname = Surname;
                    customer.Streetaddress = Streetaddress;
                    customer.Country = CountryMapper.GetCountry(Country);
                    customer.CountryCode = CountryMapper.GetCountryCode(Country);
                    customer.City = City;
                    customer.Emailaddress = Emailaddress ?? string.Empty;
                    customer.Gender = _customerService.GetGender(Gender);
                    customer.Zipcode = Zipcode;
                    customer.Birthday = Birthday;
                    customer.Telephonenumber = Telephonenumber;
                    customer.Telephonecountrycode = CountryMapper.GetTelephoneCode(customer.Country);
                    customer.NationalId = NationalId ?? string.Empty;
                    TempData["Message"] = "Update was successful!";
                    if (activate)
                    {
                        customer.IsActive = true;
                        TempData["Message"] = "Update was successful and user was activated!";
                    }
                    _customerService.Update();
                    return RedirectToPage("Index");
                }
                return Page();
            }
        }
    }
}
