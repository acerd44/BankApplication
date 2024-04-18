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
                    AccountCount = _accountService.GetAccountsFromCountry("Sweden").Count(),
                    CustomerCount = _customerService.GetCustomersFromCountry("Sweden").Count(),
                    TotalBalance = _accountService.GetBalanceOfAccountsFromCountry("Sweden"),
                    ImageLink = "~/images/SE-Sweden-Flag-icon.png"
                },
                new CountryStatisticViewModel
                {
                    Country = "Denmark",
                    AccountCount = _accountService.GetAccountsFromCountry("Denmark").Count(),
                    CustomerCount = _customerService.GetCustomersFromCountry("Denmark").Count(),
                    TotalBalance = _accountService.GetBalanceOfAccountsFromCountry("Denmark"),
                    ImageLink = "~/images/DK-Denmark-Flag-icon.png"
                },
                new CountryStatisticViewModel
                {
                    Country = "Norway",
                    AccountCount = _accountService.GetAccountsFromCountry("Norway").Count(),
                    CustomerCount = _customerService.GetCustomersFromCountry("Norway").Count(),
                    TotalBalance = _accountService.GetBalanceOfAccountsFromCountry("Norway"),
                    ImageLink = "~/images/NO-Norway-Flag-icon.png"
                },
                new CountryStatisticViewModel
                {
                    Country = "Finland",
                    AccountCount = _accountService.GetAccountsFromCountry("Finland").Count(),
                    CustomerCount = _customerService.GetCustomersFromCountry("Finland").Count(),
                    TotalBalance = _accountService.GetBalanceOfAccountsFromCountry("Finland"),
                    ImageLink = "~/images/FI-Finland-Flag-icon.png"
                }
            };
        }
    }
}
