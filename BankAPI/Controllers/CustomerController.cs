using AutoMapper;
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
        private readonly IMapper _mapper;
        public CustomerController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }
        [HttpGet("customerId")]
        public ActionResult<CustomerCardViewModel> GetCustomerCard(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            if (customer == null) return NotFound();

            var customerView = new CustomerCardViewModel();
            _mapper.Map(customer, customerView);
            return customerView;
        }
    }
}
