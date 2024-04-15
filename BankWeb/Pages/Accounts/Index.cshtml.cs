using BankLibrary.Services;
using BankWeb.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages.Accounts
{
    [Authorize(Roles = "Admin, Cashier")]
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        public IndexModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public List<AccountViewModel> Accounts { get; set; }
        public int CurrentPage { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public string Search { get; set; }
        public int CustomerId { get; set; }
        public int PageCount { get; set; }
        public string? Message { get; set; }
        public bool ShowCountry { get; set; } = false;
        public string Country { get; set; }
        public IActionResult OnGet(string country, string sortColumn, string sortOrder, int pageNumber, string search)
        {
            Message = TempData["Message"]?.ToString();
            if (!string.IsNullOrEmpty(country))
            {
                ShowCountry = true;
                Country = country;
                Accounts = _accountService.GetTopTenAccountsOfCountry(country).Select(
                    a => new AccountViewModel
                    {
                        Id = a.AccountId,
                        Name = _accountService.GetAccountOwner(a.AccountId),
                        Balance = a.Balance,
                        Country = country,
                    }).ToList();
            }
            else
            {
                if (search != null && int.TryParse(search, out _))
                {
                    if (_accountService.GetAccount(int.Parse(search)) != null)
                        return RedirectToPage("/Accounts/View", new { accountId = int.Parse(search) });
                }
                if (pageNumber == 0) pageNumber = 1;
                CurrentPage = pageNumber;
                SortColumn = sortColumn;
                SortOrder = sortOrder;
                Search = search;
                var result = _accountService.GetAccounts(sortColumn, sortOrder, pageNumber);
                PageCount = result.PageCount;
                Accounts = result.Results.Select(
                    a => new AccountViewModel
                    {
                        Id = a.AccountId,
                        Name = _accountService.GetAccountOwner(a.AccountId),
                        Balance = a.Balance
                    }).ToList();
            }
            return Page();
        }
    }
}
