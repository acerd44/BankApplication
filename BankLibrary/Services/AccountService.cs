using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.Services
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;
        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }
        public PagedResult<Account> GetAccounts(string sortColumn, string sortOrder, int page)
        {
            var query = _context.Accounts.Where(c => c.IsActive);
            if (sortColumn == "Account number")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.AccountId);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(a => a.AccountId);
            }
            else if (sortColumn == "Balance")
            {
                if (sortOrder == "asc")
                    query = query.OrderBy(a => a.Balance);
                else if (sortOrder == "desc")
                    query = query.OrderByDescending(a => a.Balance);
            }
            return query.GetPaged(page, 50);
        }
        public List<Account> GetAccounts(bool active)
        {
            return _context.Accounts.Where(a => a.IsActive == active).ToList();
        }
        public List<Account> GetAccounts(string country)
        {
            return _context.Accounts.Where(a => a.Dispositions.Any(d => d.Customer.Country == country)).Distinct().ToList();
        }
        public List<Account> GetAccountsOfCustomer(int customerId)
        {
            return _context.Accounts.Where(a => a.Dispositions.Any(d => d.AccountId == a.AccountId && d.CustomerId == customerId)).ToList();
        }
        public List<Account> GetTopTenAccounts(string country)
        {
            var top10Ids = _context.Dispositions
                .Where(d => d.Customer.Country == country)
                .Select(d => d.AccountId)
                .Distinct()
                .OrderByDescending(accId => _context.Accounts.First(a => a.AccountId == accId).Balance)
                .Take(10)
                .ToList();
            return _context.Accounts.Where(a => top10Ids.Contains(a.AccountId))
                .ToList();
        }
        public decimal GetBalanceOfAccounts(string country)
        {
            return GetAccounts(country).Sum(a => a.Balance);
        }
        public void AddTransaction(int accountId, decimal amount, bool withdraw, string symbol)
        {
            var account = _context.Accounts.First(a => a.AccountId == accountId);
            var balance = account.Balance;
            string operation;
            string type;
            if (withdraw)
            {
                amount = -amount;
                type = "Credit";
                operation = "Credit in Cash";
            }
            else
            {
                type = "Debit";
                operation = "Withdrawal in Cash";
            }
            var transaction = new Transaction
            {
                AccountId = accountId,
                Amount = amount,
                Symbol = symbol,
                Date = DateOnly.FromDateTime(DateTime.Now),
                Balance = balance,
                Type = type,
                Operation = operation
            };
            _context.Transactions.Add(transaction);
            Update();
        }
        public Account GetAccount(int accountId)
        {
            return _context.Accounts.Include(a => a.Transactions).FirstOrDefault(a => a.AccountId == accountId);
        }
        public Account GetAccount(Transaction transaction)
        {
            return _context.Accounts.Where(a => a.Transactions.Any(t => t.TransactionId == transaction.TransactionId)).First();
        }
        public void AddAccount(int customerId)
        {
            var account = new Account
            {
                Created = DateOnly.FromDateTime(DateTime.Today),
                Balance = 0,
                Frequency = "Monthly",
                IsActive = true,
            };
            _context.Accounts.Add(account);
            Update();
            var disposition = new Disposition
            {
                CustomerId = customerId,
                AccountId = account.AccountId,
                Type = "OWNER"
            };
            account.Dispositions.Add(disposition);
            Update();
        }
        public void Activate(int id, bool isCustomer)
        {
            if (isCustomer)
            {
                var customer = _context.Customers.First(c => c.CustomerId == id);
                var accounts = GetAccountsOfCustomer(id);
                if (accounts.Count != 0)
                {
                    foreach (var account in accounts)
                    {
                        account.IsActive = true;
                    }
                }
            }
            else
            {
                var account = GetAccount(id);
                account.IsActive = true;
            }
            Update();
        }
        public void Deactivate(int accountId)
        {
            var account = GetAccount(accountId);
            account.IsActive = false;
            Update();
        }
        public string GetAccountOwner(int accountId)
        {
            var customer = _context.Customers.Where(c => c.Dispositions.Any(d => d.AccountId == accountId)).First();
            return customer.Givenname + " " + customer.Surname;
        }
        public ResponseCode Deposit(int accountId, decimal amount)
        {
            var account = GetAccount(accountId);
            if (amount < 100 || amount > 10000)
            {
                return ResponseCode.IncorrectAmount;
            }
            account.Balance += amount;
            _context.SaveChanges();
            return ResponseCode.OK;
        }
        public ResponseCode Withdraw(int accountId, decimal amount)
        {
            var account = GetAccount(accountId);
            if (account.Balance < amount)
            {
                return ResponseCode.BalanceTooLow;
            }
            else if (amount < 100 || amount > 10000)
            {
                return ResponseCode.IncorrectAmount;
            }
            account.Balance -= amount;
            _context.SaveChanges();
            return ResponseCode.OK;
        }
        public void Update()
        {
            _context.SaveChanges();
        }
    }
}
