using BankLibrary.Models;
using BankLibrary.Services;
using BankLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        public List<CountryStatisticViewModel> CountryStatistics { get; set; }
        public IndexModel(ICustomerService customerService, IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
        }
        public void OnGet()
        {
            CountryStatistics = new()
            {
                new CountryStatisticViewModel
                {
                    Country = "Sweden",
                    AccountCount = _accountService.GetAccounts("Sweden").Count(),
                    CustomerCount = _customerService.GetCustomers("Sweden").Count(),
                    TotalBalance = _accountService.GetBalanceOfAccounts("Sweden"),
                },
                new CountryStatisticViewModel
                {
                    Country = "Denmark",
                    AccountCount = _accountService.GetAccounts("Denmark").Count(),
                    CustomerCount = _customerService.GetCustomers("Denmark").Count(),
                    TotalBalance = _accountService.GetBalanceOfAccounts("Denmark"),
                },
                new CountryStatisticViewModel
                {
                    Country = "Norway",
                    AccountCount = _accountService.GetAccounts("Norway").Count(),
                    CustomerCount = _customerService.GetCustomers("Norway").Count(),
                    TotalBalance = _accountService.GetBalanceOfAccounts("Norway"),
                },
                new CountryStatisticViewModel
                {
                    Country = "Finland",
                    AccountCount = _accountService.GetAccounts("Finland").Count(),
                    CustomerCount = _customerService.GetCustomers("Finland").Count(),
                    TotalBalance = _accountService.GetBalanceOfAccounts("Finland"),
                }
            };
        }
    }
}