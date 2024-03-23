using BankWeb.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages
{
    [Authorize(Roles="Admin")]
    public class LoansModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public List<Loan> Loans { get; set; }
        public LoansModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public void OnGet()
        {
            Loans  = _context.Loans.Take(50).ToList();
        }
    }
}
