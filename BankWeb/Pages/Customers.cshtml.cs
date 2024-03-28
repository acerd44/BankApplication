using BankLibrary.Models;
using BankWeb.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BankWeb.Pages
{
    public class CustomersModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        public CustomersModel(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<CustomerViewModel> Customers { get; set; }
        public void OnGet(string sortColumn, string sortOrder)
        {
            var query = _context.Customers.Take(50).Select(c => new CustomerViewModel
            {
                Id = c.CustomerId,
                Name = c.Givenname + " " + c.Surname,
                Country = c.Country
            });
            if (sortColumn == "Id")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Id);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Id);
            }
            else if (sortColumn == "Name")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Name);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Name);
            }
            else if (sortColumn == "Country")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Country);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Country);
            }

            Customers = query.ToList();

        }
    }
}
