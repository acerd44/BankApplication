using BankLibrary.Models;
using BankLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BankWeb.Pages.Customer
{
    [BindProperties]
    public class NewModel : PageModel
    {
        private readonly ICustomerService _customerService;
        public NewModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [MaxLength(20)][Required] public string FirstName { get; set; }
        [MaxLength(20)][Required] public string LastName { get; set; }
        [StringLength(100)][Required] public string StreetAddress { get; set; }
        [StringLength(50)][Required] public string Country { get; set; }
        [StringLength(50)][Required] public string City { get; set; }
        [StringLength(100)][Required][EmailAddress] public string Email { get; set; }
        [Required] public string Gender { get; set; }
        [Required] public string ZipCode { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? TelephoneNumber { get; set; }
        public string? NationalId { get; set; }
        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                var customer = new BankLibrary.Models.Customer
                {
                    Givenname = FirstName,
                    Surname = LastName,
                    Streetaddress = StreetAddress,
                    Country = Country,
                    CountryCode = _customerService.GetCountryCode(Country),
                    City = City,
                    Birthday = Birthday,
                    Telephonenumber = TelephoneNumber,
                    NationalId = NationalId,
                    Gender = Gender,
                    Emailaddress = Email,
                    Zipcode = ZipCode
                };
                _customerService.AddCustomer(customer);
                return RedirectToPage("Index");
            }
            return Page();
        }
        //
    }
}
