using BankLibrary.Services;
using BankWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BankWeb.Pages
{
    public class CustomersModel : PageModel
    {
        private readonly ICustomerService _customerService;
        public CustomersModel(ICustomerService customerService)
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
        public void OnGet(string sortColumn, string sortOrder, int pageNumber, string search)
        {
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
                    Name = c.Givenname + " " + c.Surname,
                    Country = c.Country
                }).ToList();
        }
    }
}
