using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
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
        List<Customer> GetCustomers();
        Customer GetCustomer(int customerId);
        void AddCustomer(Customer customer);
        void Update();
        string GetCountryCode(string country);
    }
}
