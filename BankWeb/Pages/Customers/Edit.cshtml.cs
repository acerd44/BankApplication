using BankLibrary.Models;
using BankLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace BankWeb.Pages.Customers
{
    [BindProperties]
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
        public List<SelectListItem> Countries { get; set; }
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
        public List<SelectListItem> Genders { get; set; }
        [Required]
        public string Zipcode { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Telephonenumber { get; set; }
        public string? NationalId { get; set; }
        public int CustomerId { get; set; }
        public void OnGet(int customerId)
        {
            CustomerId = customerId;
            Genders = _customerService.GetGenderList();
            Countries = _customerService.GetCountryList();
            var customer = _customerService.GetCustomer(customerId);
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

        public IActionResult OnPost(int customerId, bool delete)
        {
            if (delete)
            {
                _customerService.Delete(customerId);
                return RedirectToPage("Index");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var customer = _customerService.GetCustomer(customerId);
                    customer.Givenname = Givenname;
                    customer.Surname = Surname;
                    customer.Streetaddress = Streetaddress;
                    customer.Country = _customerService.GetCountry(Country);
                    customer.CountryCode = _customerService.GetCountryCode(Country);
                    customer.City = City;
                    customer.Emailaddress = Emailaddress;
                    customer.Gender = _customerService.GetGender(Gender);
                    customer.Zipcode = Zipcode;
                    customer.Birthday = Birthday;
                    customer.Telephonenumber = Telephonenumber;
                    customer.NationalId = NationalId;
                    _customerService.Update();
                    return RedirectToPage("Index");
                }
                Genders = _customerService.GetGenderList();
                Countries = _customerService.GetCountryList();
                return Page();
            }
        }
    }
}
