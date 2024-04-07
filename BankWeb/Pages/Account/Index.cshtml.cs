using BankLibrary.Services;
using BankWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages.Account
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public List<AccountViewModel> Accounts { get; set; }
        public string CustomerName { get; set; }
        public void OnGet(int customerId, string customerName)
        {
            Accounts = _accountService.GetAccounts(customerId).Select(
                a => new AccountViewModel
                {
                    Id = a.AccountId,
                    AccountNumber = a.AccountId,
                    Balance = a.Balance
                }).ToList();
            CustomerName = customerName;
        }
    }
}
