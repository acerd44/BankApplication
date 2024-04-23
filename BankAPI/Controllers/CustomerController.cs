using BankLibrary.Services;
using BankLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("api/me")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        [HttpGet("customerId")]
        public ActionResult<CustomerCardViewModel> GetCustomerCard(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            if (customer == null) return NotFound();
            var customerView = new CustomerCardViewModel
            {
                Id = customer.CustomerId,
                NationalId = customer.NationalId,
                Surname = customer.Surname,
                Givenname = customer.Givenname,
                Birthday = customer.Birthday,
                City = customer.City,
                Country = customer.Country,
                Emailaddress = customer.Emailaddress,
                IsActive = customer.IsActive,
                Gender = customer.Gender,
                Streetaddress = customer.Streetaddress,
                Telephonecountrycode = customer.Telephonecountrycode,
                Telephonenumber = customer.Telephonenumber,
                Zipcode = customer.Zipcode
            };
            return customerView;
        }
    }
}
