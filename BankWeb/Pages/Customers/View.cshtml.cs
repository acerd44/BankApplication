using BankLibrary.Models;
using BankLibrary.Services;
using BankWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages.Customers
{
    [Authorize(Roles = "Admin, Cashier")]
    public class ViewModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        public ViewModel(ICustomerService customerService, IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
        }
        public CustomerCardViewModel Customer { get; set; }
        public List<AccountViewModel> Accounts { get; set; }
        public decimal TotalBalance { get; set; }
        public void OnGet(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            if (customer != null)
            {
                Customer = new CustomerCardViewModel
                {
                    Id = customer.CustomerId,
                    Birthday = customer.Birthday,
                    City = customer.City,
                    Country = customer.Country,
                    Emailaddress = customer.Emailaddress ?? string.Empty,
                    Givenname = customer.Givenname,
                    Surname = customer.Surname,
                    Gender = customer.Gender,
                    Streetaddress = customer.Streetaddress,
                    Telephonenumber = customer.Telephonenumber,
                    Telephonecountrycode = customer.Telephonecountrycode,
                    Zipcode = customer.Zipcode,
                    NationalId = customer.NationalId ?? string.Empty,
                    IsActive = customer.IsActive
                };
                Accounts = _accountService.GetAccountsOfCustomer(customerId).Select(
                    a => new AccountViewModel
                    {
                        Id = a.AccountId,
                        Balance = a.Balance,
                        Created = a.Created,
                        Country = Customer.Country
                    }).ToList();
                TotalBalance = Accounts.Sum(a => a.Balance);
            }
        }
    }
}
