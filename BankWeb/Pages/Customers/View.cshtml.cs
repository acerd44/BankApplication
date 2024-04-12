using BankLibrary.Models;
using BankLibrary.Services;
using BankWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages.Customers
{
    public class ViewModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        public ViewModel(ICustomerService customerService, IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
        }
        public Customer Customer { get; set; }
        public List<AccountViewModel> Accounts { get; set; }
        public decimal TotalBalance { get; set; }
        public void OnGet(int customerId)
        {
            Customer = _customerService.GetCustomer(customerId);
            Accounts = _accountService.GetAccounts(customerId).Select(
                a => new AccountViewModel
                {
                    Id = a.AccountId,
                    AccountNumber = a.AccountId,
                    Balance = a.Balance,
                    Created = a.Created,
                    Country = Customer.Country
                }).ToList();
            TotalBalance = Accounts.Sum(a => a.Balance);
        }
    }
}
