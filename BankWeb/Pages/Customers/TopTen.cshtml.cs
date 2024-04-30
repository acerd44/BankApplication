using BankLibrary.Models;
using BankLibrary.Services;
using BankLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BankWeb.Pages.Customers
{
    [ResponseCache(Duration = 60, VaryByQueryKeys = new[] { "country" })]
    public class TopTenModel : PageModel
    {
        private readonly ICustomerService _customerService;
        public TopTenModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        public List<TopTenCustomerViewModel> TopTenCustomers { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public void OnGet(string country)
        {
            Country = country;
            CountryCode = CountryMapper.GetCountryCode(Country);
            TopTenCustomers = _customerService.GetTopTenCustomers(country);
        }
    }
}
