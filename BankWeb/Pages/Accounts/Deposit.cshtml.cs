using BankLibrary.Models;
using BankLibrary.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace BankWeb.Pages.Accounts
{
    [BindProperties]
    [Authorize(Roles = "Admin, Cashier")]
    public class DepositModel : PageModel
    {
        private readonly IAccountService _accountService;
        public DepositModel(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [Range(100, 10000)]
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }

        public void OnGet(int accountId)
        {
            Balance = _accountService.GetAccount(accountId).Balance;
        }

        public IActionResult OnPost(int accountId)
        {
            Balance = _accountService.GetAccount(accountId).Balance;
            var status = _accountService.Deposit(accountId, Amount);
            if (ModelState.IsValid)
            {
                if (status == ResponseCode.OK)
                {
                    _accountService.AddTransaction(accountId, Amount, false, "");
                    return RedirectToPage("Index");
                }
            }
            if (status == ResponseCode.BalanceTooLow)
            {
                ModelState.AddModelError("Amount", "You don't have enough money");
            }
            else if (status == ResponseCode.IncorrectAmount)
            {
                ModelState.AddModelError("Amount", "Please enter an amount between 100 and 10000.");
            }
            return Page();
        }
    }
}