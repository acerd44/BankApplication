using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
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
        Customer GetCustomer(int customerId);
        bool ValidateEmail(string email);
        bool ValidateNationalId(string nationalId);
        void AddCustomer(Customer customer);
        List<SelectListItem> GetCountryList();
        string GetCountry(Country country);
        string GetCountryCode(Country country);
        string GetTelephoneCode(string country);
        List<SelectListItem> GetGenderList();
        string GetGender(Gender gender);
        void Update();
        void Delete(int customerId);
    }
}
