using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
using BankLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.Services
{
    public interface ICustomerService
    {
        PagedResult<Customer> GetCustomers(string sortColumn, string sortOrder, int page, string search);
        List<Customer> GetCustomers(bool active);
        List<Customer> GetCustomers(string country);
        List<TopTenCustomerViewModel> GetTopTenCustomers(string country);
        Customer GetCustomer(int customerId);
        Customer GetCustomer(Transaction transaction);
        Customer GetCustomer(Account account);
        bool ValidateEmail(string email);
        bool ValidateNationalId(string nationalId);
        void AddCustomer(Customer customer);
        string GetGender(Gender gender);
        void Update();
        void Delete(int customerId);
    }
}
