using BankWeb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages
{
    public class AccountModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public AccountModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public void OnGet(int accountId)
        {
            var account = _context.Accounts.First(a => a.AccountId == accountId);
            AccountNo = account.AccountId.ToString();
            Balance = account.Balance;
        }

        public string AccountNo { get; set; }
        public decimal Balance { get; set; }
    }
}
