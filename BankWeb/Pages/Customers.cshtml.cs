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
        public void OnGet(string sortColumn, string sortOrder)
        {
            Customers = _customerService.GetCustomers(sortColumn, sortOrder)
                .Select(c => new CustomerViewModel
                {
                    Id = c.CustomerId,
                    Name = c.Givenname + " " + c.Surname,
                    Country = c.Country
                }).ToList();
        }
    }
}
