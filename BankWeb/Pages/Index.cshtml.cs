using BankLibrary.Models;
using BankLibrary.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        private readonly IAccountService _accountService;
        public int SwedenCustomers { get; set; }
        public int SwedenAccounts { get; set; }
        public decimal SwedenBalance { get; set; }        
        public int DenmarkCustomers { get; set; }
        public int DenmarkAccounts { get; set; }
        public decimal DenmarkBalance { get; set; }        
        public int NorwayCustomers { get; set; }
        public int NorwayAccounts { get; set; }
        public decimal NorwayBalance { get; set; }        
        public int FinlandCustomers { get; set; }
        public int FinlandAccounts { get; set; }
        public decimal FinlandBalance { get; set; }
        public IndexModel(ICustomerService customerService, IAccountService accountService)
        {
            _customerService = customerService;
            _accountService = accountService;
        }

        public void OnGet()
        {
            SwedenCustomers = _customerService.GetCustomersFromCountry("Sweden").Count();
            SwedenAccounts = _accountService.GetAccountsFromCountry("Sweden").Count();
            SwedenBalance = _accountService.GetBalanceOfAccountsFromCountry("Sweden");
            DenmarkCustomers = _customerService.GetCustomersFromCountry("Denmark").Count();
            DenmarkAccounts = _accountService.GetAccountsFromCountry("Denmark").Count();
            DenmarkBalance = _accountService.GetBalanceOfAccountsFromCountry("Denmark");
            NorwayCustomers = _customerService.GetCustomersFromCountry("Norway").Count();
            NorwayAccounts = _accountService.GetAccountsFromCountry("Norway").Count();
            NorwayBalance = _accountService.GetBalanceOfAccountsFromCountry("Norway");
            FinlandCustomers = _customerService.GetCustomersFromCountry("Finland").Count();
            FinlandAccounts = _accountService.GetAccountsFromCountry("Finland").Count();
            FinlandBalance = _accountService.GetBalanceOfAccountsFromCountry("Finland");
        }
    }
}
