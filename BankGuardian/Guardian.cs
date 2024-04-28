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
            CreateDirectoryAndFiles();
            var countries = Enum.GetValues(typeof(Country))
                .Cast<Country>()
                .Skip(1)
                .Select(c => c.ToString())
                .ToArray();
            List<Transaction> transactions = new();
            List<TransactionÍnfo> transactionsAboveLimit = new();
            List<int> alreadyCheckedTransactions = new();
            List<int> alreadyCheckedCustomers = new();
            List<int> customersAboveLimit = new();
            FillListFromFile(ref alreadyCheckedTransactions, "../../../reports/checked_transactions.txt");
            FillListFromFile(ref alreadyCheckedCustomers, "../../../reports/checked_customers.txt");
            for (int countryIndex = 0; countryIndex < 4; countryIndex++)
            {
                if (File.Exists($"../../../reports/{countries[countryIndex]}_{DateTime.Now:yyyy_MM_dd}.txt"))
                {
                    Console.WriteLine("There has already been a check today. Stopping...");
                    return;
                }
                Console.WriteLine("Checking " + countries[countryIndex]);
                transactionsAboveLimit = new();
                customersAboveLimit = new();
                transactions = _transactionService.GetTransactions(countries[countryIndex])
                    .Where(t => t.Date >= DateOnly.FromDateTime(DateTime.Now.AddDays(-1))
                    && !alreadyCheckedTransactions.Contains(t.TransactionId)).ToList();
                var accountsAboveLimit = _accountService.GetAccounts(countries[countryIndex])
                    .Where(a => a.Transactions
                       .Where(t => t.Date >= DateOnly.FromDateTime(DateTime.Now.AddDays(-3))).Sum(t => t.Amount) > 23000)
                    .Select(a => a.AccountId)
                    .ToList();
                accountsAboveLimit.ForEach(a =>
                {
                    if (!alreadyCheckedCustomers.Contains(_customerService.GetCustomer(_accountService.GetAccount(a)).CustomerId))
                        customersAboveLimit.Add(_customerService.GetCustomer(_accountService.GetAccount(a)).CustomerId);
                });
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
                        alreadyCheckedTransactions.Add(transactions[i].TransactionId);
                    }
                }

                using (StreamWriter sw = new($"../../../reports/{countries[countryIndex]}_{DateTime.Now:yyyy_MM_dd}.txt"))
                {
                    sw.WriteLine($"Suspicious transactions in {countries[countryIndex]}");
                    sw.WriteLine("Transactions above 15000 SEK:");
                    transactionsAboveLimit.ForEach(t =>
                    {
                        using (StreamWriter sw2 = new("../../../reports/checked_transactions.txt", true))
                        {
                            sw2.WriteLine(t.Transaction.TransactionId);
                        }
                        sw.WriteLine($"Transaction ID: {t.Transaction.TransactionId} " +
                        $"- Customer ID: {t.CustomerId} " +
                        $"- Account ID: {t.AccountId} " +
                        $"- Amount: SEK {t.Amount}");
                    });
                }
                using (StreamWriter sw = new($"../../../reports/suspicious_customers_{DateTime.Now.AddDays(-3):yyyyMMdd}-{DateTime.Now:yyyyMMdd}.txt"))
                {
                    sw.WriteLine("\nCustomers that have made transactions above 23000 SEK in total in the past 72 hours.");
                    sw.WriteLine($"Between {DateTime.Now.AddDays(-3):yyyy-MM-dd} and {DateTime.Now:yyyy-MM-dd}");
                    customersAboveLimit.ForEach(c =>
                    {
                        using (StreamWriter sw2 = new("../../../reports/checked_customers.txt"))
                        {
                            sw2.WriteLine(c);
                        }
                        sw.WriteLine($"Customer ID: {c}");
                    });
                }
                Console.WriteLine($"\nSuspicious transactions found: {transactionsAboveLimit.Count}");
                Console.WriteLine($"Suspicious customers found: {customersAboveLimit.Count}");
                Console.WriteLine("_____________________________________________________________________");
            }
        }
        public static void CreateDirectoryAndFiles()
        {
            if (!Directory.Exists("../../../reports"))
            {
                Directory.CreateDirectory("../../../reports");
            }
            if (!File.Exists("../../../reports/checked_transactions.txt"))
            {
                using StreamWriter sr = new("../../../reports/checked_transactions.txt");
            }
            if (!File.Exists("../../../reports/checked_customers.txt"))
            {
                using StreamWriter sr = new("../../../reports/checked_customers.txt");
            }
        }
        public static void FillListFromFile(ref List<int> list, string path)
        {
            using (StreamReader sr = new(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    if (int.TryParse(line, out int customerId))
                    {
                        list.Add(customerId);
                    }
                }
            }
        }
    }
}