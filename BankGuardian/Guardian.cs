using BankLibrary.Models;
using BankLibrary.Services;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuardian
{
    public class Guardian
    {
        private readonly IAccountService _accountService;
        private readonly ICustomerService _customerService;
        private readonly ITransactionService _transactionService;
        public Guardian(IAccountService accountService, ICustomerService customerService, ITransactionService transactionService)
        {
            _accountService = accountService;
            _customerService = customerService;
            _transactionService = transactionService;
        }

        public void Check()
        {
            if (!Directory.Exists("../../../reports")) Directory.CreateDirectory("../../../reports");
            var countries = Enum.GetValues(typeof(Country))
                .Cast<Country>()
                .Skip(1)
                .Select(c => c.ToString())
                .ToArray();
            List<Transaction> transactions = new();
            List<TransactionÍnfo> transactionsAboveLimit = new();
            List<int> customersAboveLimit = new();
            for (int countryIndex = 0; countryIndex < 4; countryIndex++)
            {
                Console.WriteLine("Checking " + countries[countryIndex]);
                transactions = _transactionService.GetTransactions(countries[countryIndex]).Where(t => t.Date >= DateOnly.FromDateTime(DateTime.Now.AddDays(-1))).ToList();
                //transactions = _transactionService.GetTransactions(countries[countryIndex])
                //    .Where(t => t.Date >= DateOnly.FromDateTime(DateTime.Now.AddDays(-90)))
                //    .ToList();
                //transactionAmountInPast72Hours = transactions
                //.Where(t => t.Date >= DateOnly.FromDateTime(DateTime.Now.AddDays(-180)))
                //    .Where(t => t.Date >= DateOnly.FromDateTime(DateTime.Now.AddDays(-3)))
                //    .Sum(t => t.Amount);
                var accountsAboveLimit = _accountService.GetAccounts(countries[countryIndex])
                    .Where(a => a.Transactions
                       .Where(t => t.Date >= DateOnly.FromDateTime(DateTime.Now.AddDays(-3))).Sum(t => t.Amount) > 23000)
                    .Select(a => a.AccountId)
                    .ToList();
                accountsAboveLimit.ForEach(a => customersAboveLimit.Add(_customerService.GetCustomer(_accountService.GetAccount(a)).CustomerId));
                for (int i = 0; i < transactions.Count; i++)
                {
                    if (transactions[i].Amount >= 15000)
                    {
                        transactionsAboveLimit.Add(new TransactionÍnfo
                        {
                            Transaction = transactions[i],
                            Amount = transactions[i].Amount,
                            Country = countries[countryIndex],
                            CustomerId = _customerService.GetCustomer(transactions[i]).CustomerId,
                            AccountId = _accountService.GetAccount(transactions[i]).AccountId
                        });
                    }
                    //transactions.Where(t => t.Date >= DateOnly.FromDateTime(DateTime.Now.AddDays(-3)) >= 23000
                }
                using (StreamWriter sw = new($"../../../reports/{countries[countryIndex]}_{DateTime.Now:yyyy_MM_dd}.txt"))
                {
                    sw.WriteLine($"Suspicious transactions in {countries[countryIndex]}");
                    sw.WriteLine("Transactions above 15000 SEK:");
                    foreach (var t in transactionsAboveLimit)
                    {
                        transactionsAboveLimit.ForEach(t =>
                        sw.WriteLine($"Transaction ID: {t.Transaction.TransactionId} " +
                        $"- Customer ID: {t.CustomerId} " +
                        $"- Account ID: {t.AccountId} " +
                        $"- Amount: SEK {t.Amount}"));
                    }
                    sw.WriteLine("\nCustomers that have made transactions above 23000 SEK in total in the past 72 hours.");
                    customersAboveLimit.ForEach(c => sw.WriteLine($"Customer ID: {c}"));
                }
            }
        }
    }
}