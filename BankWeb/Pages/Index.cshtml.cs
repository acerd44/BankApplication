using BankLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly ApplicationDbContext _context;
        public int Customers { get; set; }
        public int Accounts { get; set; }
        public decimal BalanceSum { get; set; }
        public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
            Customers = _context.Customers.Count();
            Accounts = _context.Accounts.Count();
            BalanceSum = _context.Accounts.Sum(a => a.Balance);
        }
    }
}
