using AutoMapper;
using BankLibrary.Models;
using BankLibrary.Services;
using BankLibrary.ViewModels;
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
        private readonly IMapper _mapper;
        public NewModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }
        public CRUDCustomerViewModel Customer { get; set; }
        public void OnGet()
        {
            Customer = new();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                if (_customerService.ValidateEmail(Customer.Emailaddress) || _customerService.ValidateNationalId(Customer.NationalId))
                {
                    if (_customerService.ValidateEmail(Customer.Emailaddress))
                        ModelState.AddModelError("Customer.Emailaddress", "Email address is already in use, please enter another one.");
                    if (_customerService.ValidateNationalId(Customer.NationalId))
                        ModelState.AddModelError("Customer.NationalId", "National Id is already in use, please enter another one.");
                    return Page();
                }
                var customer = new Customer();
                _mapper.Map(Customer, customer);
                customer.IsActive = true;
                customer.CountryCode = CountryMapper.GetCountryCode(Customer.Country);
                customer.Telephonecountrycode = CountryMapper.GetTelephoneCode(Customer.Country.ToString());
                _customerService.AddCustomer(customer);
                TempData["Message"] = "Customer was created successfully!";
                return RedirectToPage("Index");
            }
            return Page();
        }
    }
}
