﻿using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using BankLibrary.ViewModels;
using Microsoft.EntityFrameworkCore;

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
            else if (sortColumn == "Active")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(c => c.IsActive);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(c => c.IsActive);
            }
            return query.GetPaged(page, 50);
        }
        public List<Customer> GetCustomers(bool active)
        {
            return _context.Customers.Where(c => c.IsActive == active).ToList();
        }
        public List<Customer> GetCustomers(string country)
        {
            return _context.Customers.Where(c => c.Country == country).Distinct().ToList();
        }
        public List<TopTenCustomerViewModel> GetTopTenCustomers(string country)
        {
            return _context.Customers
                .Where(c => c.Country == country)
                .Select(
                c => new TopTenCustomerViewModel
                {
                    Id = c.CustomerId,
                    Name = c.Givenname + " " + c.Surname,
                    Balance = c.Dispositions.Select(d => d.Account.Balance).Sum()
                }).OrderByDescending(c => c.Balance)
                .Take(10)
                .ToList();
        }
        public Customer GetCustomer(int customerId)
        {
            return _context.Customers.FirstOrDefault(c => c.CustomerId == customerId);
        }
        public Customer GetCustomer(Transaction transaction)
        {
            var account = _context.Accounts.Where(a => a.Transactions.Any(t => t.TransactionId == transaction.TransactionId)).First();
            return _context.Customers.Where(c => c.Dispositions.Any(d => d.AccountId == account.AccountId)).First();
        }
        public Customer GetCustomer(Account account)
        {
            return _context.Dispositions.Where(d => d.AccountId == account.AccountId)
                .Select(d => d.Customer)
                .First();
        }
        public bool ValidateEmail(string email)
        {
            return _context.Customers.Any(c => c.Emailaddress == email);
        }
        public bool ValidateNationalId(string nationalId)
        {
            if (!string.IsNullOrEmpty(nationalId))
                return _context.Customers.Any(c => c.NationalId == nationalId);
            return false;
        }
        public void AddCustomer(Customer customer)
        {
            _context.Customers.Add(customer);
            var account = new Account
            {
                Created = DateOnly.FromDateTime(DateTime.Today),
                Balance = 0,
                Frequency = "Monthly",
                IsActive = true,
            };
            _context.Accounts.Add(account);
            Update();
            var disposition = new Disposition
            {
                CustomerId = customer.CustomerId,
                AccountId = account.AccountId,
                Type = "OWNER"
            };
            account.Dispositions.Add(disposition);
            Update();
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
            var customer = GetCustomer(customerId);
            if (_context.Accounts.Where(a => a.AccountId == customerId && a.IsActive).Any())
            {
                foreach (var account in _context.Accounts.Where(a => a.AccountId == customerId && a.IsActive))
                {
                    account.IsActive = false;
                }
            }
            customer.IsActive = false;
            Update();
        }
    }
}
