using AutoMapper;
using BankLibrary.Infrastructure;
using BankLibrary.Models;
using BankLibrary.Models.Migrations;
using BankLibrary.Services;
using BankLibrary.ViewModels;
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
        private readonly IMapper _mapper;
        public EditModel(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }
        public CRUDCustomerViewModel Customer { get; set; }
        public int CustomerId { get; set; }
        public void OnGet(int customerId)
        {
            CustomerId = customerId;
            var customer = _customerService.GetCustomer(customerId);
            Customer = new CRUDCustomerViewModel();
            _mapper.Map(customer, Customer);
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
                    if (customer.NationalId != Customer.NationalId || customer.Emailaddress != Customer.Emailaddress)
                    {
                        bool proceed = true;
                        if (_customerService.ValidateEmail(Customer.Emailaddress) && customer.Emailaddress != Customer.Emailaddress)
                        {
                            proceed = false;
                            ModelState.AddModelError("Customer.Emailaddress", "Email address is already in use, please enter another one.");
                        }
                        if (_customerService.ValidateNationalId(Customer.NationalId) && customer.NationalId != Customer.NationalId)
                        {
                            proceed = false;
                            ModelState.AddModelError("Customer.NationalId", "National Id is already in use, please enter another one.");
                        }
                        if (!proceed)
                        {
                            return Page();
                        }
                    }

                    TempData["Message"] = "Update was successful!";
                    if (activate)
                    {
                        customer.IsActive = true;
                        TempData["Message"] = "Update was successful and user was activated!";
                    }
                    customer.CountryCode = CountryMapper.GetCountryCode(Customer.Country);
                    customer.Telephonecountrycode = CountryMapper.GetTelephoneCode(Customer.Country.ToString());
                    Customer.IsActive = customer.IsActive;
                    _mapper.Map(Customer, customer);
                    _customerService.Update();
                    return RedirectToPage("Index");
                }
                return Page();
            }
        }
    }
}
