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
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public CustomerController(ICustomerService customerService, IMapper mapper, IAccountService accountService)
        {
            _customerService = customerService;
            _mapper = mapper;
            _accountService = accountService;
        }
        [HttpGet("customerId")]
        public ActionResult<APICustomerViewModel> GetCustomerCard(int customerId)
        {
            var customer = _customerService.GetCustomer(customerId);
            if (customer == null) return NotFound();

            var customerView = new APICustomerViewModel();
            customerView.CustomerInfo = _mapper.Map<CustomerCardViewModel>(customer);
            if (_accountService.GetAccountsOfCustomer(customerView.CustomerInfo.CustomerId).Count > 0)
                customerView.Accounts = _accountService.GetAccountsOfCustomer(customerView.CustomerInfo.CustomerId).Select(a => new APIAccountViewModel
                {
                    AccountId = a.AccountId,
                    Balance = a.Balance
                }).ToList();
            return customerView;
        }
    }
}
