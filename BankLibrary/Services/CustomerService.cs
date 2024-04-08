using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
using ISO3166;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }
        public PagedResult<Customer> GetCustomers(string sortColumn, string sortOrder, int page, string search)
        {
            var query = _context.Customers.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(c => c.Givenname.Contains(search) || c.City.Contains(search));
            }
            if (sortColumn == "Id")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.CustomerId);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.CustomerId);
            }
            else if (sortColumn == "Name")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Givenname);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Givenname);
            }
            else if (sortColumn == "Country")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.Country);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.Country);
            }
            else if (sortColumn == "City")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.City);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.City);
            }

            return query.GetPaged(page, 50);
        }

        public Customer GetCustomer(int customerId)
        {
            return _context.Customers.First(c => c.CustomerId == customerId);
        }

        public List<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            Update();
        }

        public string GetCountryCode(string countryName)
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            CultureInfo swedishCulture = CultureInfo.GetCultureInfo("sv-SE");
            Country country = Country.List.FirstOrDefault(c => c.Name.Equals(countryName, StringComparison.OrdinalIgnoreCase));
            if (country != null)
            {
                return country.TwoLetterCode;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = swedishCulture;
                Thread.CurrentThread.CurrentUICulture = swedishCulture;
                country = Country.List.FirstOrDefault(c => c.Name.Equals(countryName, StringComparison.OrdinalIgnoreCase));
                if (country != null)
                {
                    Thread.CurrentThread.CurrentCulture = currentCulture;
                    Thread.CurrentThread.CurrentUICulture = currentCulture;
                    return country.TwoLetterCode;
                }
            }
            Thread.CurrentThread.CurrentCulture = currentCulture;
            Thread.CurrentThread.CurrentUICulture = currentCulture;
            return "";
        }
        public void Update()
        {
            _context.SaveChanges();
        }
    }
}
