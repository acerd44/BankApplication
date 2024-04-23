﻿using BankLibrary.Services;
using BankLibrary.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [ApiController]
    [Route("account")]
    public class TransactionController : Controller
    {
        private readonly IAccountService _accountService;
        public TransactionController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        [HttpGet("accountId")]
        public ActionResult<IEnumerable<TransactionViewModel>> GetTransactions(int accountId, int limit = 20, int offset = 0)
        {
            if (_accountService.GetAccount(accountId) == null) return NotFound();
            var transactions = _accountService.GetTransactions(accountId);
            transactions = transactions.Skip(offset).Take(limit);

            var transactionsView = transactions.Select(t => new TransactionViewModel
            {
                Id = t.TransactionId,
                AccountId = t.AccountId,
                Amount = t.Amount,
                Balance = t.Balance,
                Date = t.Date,
                Operation = t.Operation,
                Symbol = t.Symbol,
                Type = t.Type
            }).ToList();

            return transactionsView;
        }
    }
}
