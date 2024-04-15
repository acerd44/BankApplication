using BankLibrary.Services;
using BankWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages.Accounts
{
    [Authorize(Roles = "Admin, Cashier")]
    public class ViewModel : PageModel
    {
        private readonly IAccountService _accountService;
        public ViewModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public AccountViewModel Account { get; set; }
        public List<TransactionViewModel> Transactions { get; set; }
        public void OnGet(int accountId)
        {
            var account = _accountService.GetAccount(accountId);
            Account = new AccountViewModel { Id = 0 };
            if (account != null)
            {
                Account = new AccountViewModel
                {
                    Id = account.AccountId,
                    Balance = account.Balance,
                    Created = account.Created,
                    Name = _accountService.GetAccountOwner(accountId)
                };
                Transactions = account.Transactions.Select(
                t => new TransactionViewModel
                {
                    Id = t.TransactionId,
                    AccountId = accountId,
                    Amount = t.Amount,
                    Balance = t.Balance,
                    Date = t.Date,
                    Operation = t.Operation,
                    Symbol = t.Symbol,
                    Type = t.Type
                }).ToList();
            }
        }
        public IActionResult OnGetShowMore(int accountId, int pageNo)
        {
            var transactionList = _accountService.GetMoreTransactions(accountId, pageNo).Select(
                t => new TransactionViewModel
                {
                    Id = t.TransactionId,
                    AccountId = t.AccountId,
                    Amount = t.Amount,
                    Date = t.Date,
                    Balance = t.Balance,
                    Operation = t.Operation,
                    Symbol = t.Symbol,
                    Type = t.Type
                }).ToList();
            return new JsonResult(new { transactions = transactionList });
        }
    }
}