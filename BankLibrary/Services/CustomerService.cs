using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public List<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }
        public Customer GetCustomer(int customerId)
        {
            return _context.Customers.First(c => c.CustomerId == customerId);
        }
        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            Update();
        }
        public List<SelectListItem> GetCountryList()
        {
            return Enum.GetValues<Country>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                }).ToList();
        }
        public string GetCountry(Country country)
        {
            return Enum.GetName(typeof(Country), country)!;
        }
        public string GetCountryCode(Country country)
        {
            return CountryMapper.GetCountryCode(country);
        }
        public List<SelectListItem> GetGenderList()
        {
            return Enum.GetValues<Gender>()
                .Select(c => new SelectListItem
                {
                    Value = c.ToString(),
                    Text = c.ToString()
                }).ToList();
        }
        public string GetGender(Gender gender)
        {
            return Enum.GetName(typeof(Gender), gender)!.ToLower();
        }
        public void Update()
        {
            _context.SaveChanges();
        }
        public void Delete(int customerId)
        {

            Update();
        }
    }
}
