﻿using BankLibrary.Models;
using Microsoft.EntityFrameworkCore;
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

        public List<Account> GetAccounts(int customerId)
        {
            return _context.Accounts.Where(a => a.Dispositions.Any(d => d.AccountId == a.AccountId && d.CustomerId == customerId) && a.IsActive).ToList();
        }
        public List<Account> GetTop10Accounts(string country)
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
        public Account GetAccount(int accountId)
        {
            return _context.Accounts.First(a => a.AccountId == accountId);
        }
        public ResponseCode Deposit(int accountId, decimal amount)
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