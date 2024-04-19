using BankLibrary.Services;
using BankLibrary.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace BankWeb.Pages.Customers
{
    [Authorize(Roles = "Admin, Cashier")]
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;
        public IndexModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public List<CustomerViewModel> Customers { get; set; }
        public int CurrentPage { get; set; }
        public string SortOrder { get; set; }
        public string SortColumn { get; set; }
        public string Search { get; set; }
        public int CustomerId { get; set; }
        public int PageCount { get; set; }
        public string? Message { get; set; }
        public IActionResult OnGet(string sortColumn, string sortOrder, int pageNumber, string search)
        {
            Message = TempData["Message"]?.ToString();
            if (search != null && int.TryParse(search, out _))
            {
                if (_customerService.GetCustomer(int.Parse(search)) != null)
                    return RedirectToPage("/Customers/View", new { customerId = int.Parse(search) });
            }
            if (pageNumber == 0) pageNumber = 1;
            CurrentPage = pageNumber;
            SortColumn = sortColumn;
            SortOrder = sortOrder;
            Search = search;
            var result = _customerService.GetCustomers(sortColumn, sortOrder, pageNumber, search);
            PageCount = result.PageCount;
            Customers = result.Results
                .Select(c => new CustomerViewModel
                {
                    Id = c.CustomerId,
                    NationalId = c.NationalId,
                    Name = c.Givenname + " " + c.Surname,
                    Country = c.Country,
                    City = char.ToUpper(c.City[0]) + c.City.Substring(1).ToLower(),
                    Streetaddress = c.Streetaddress
                }).ToList();
            return Page();
        }
    }
}
