using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.Services
{
    public interface IAccountService
    {
        PagedResult<Account> GetAccounts(string sortColumn, string sortOrder, int page);
        List<Account> GetAccounts(bool active);
        List<Account> GetAccounts(string country);
        List<Account> GetAccountsOfCustomer(int customerId);
        List<Account> GetTopTenAccounts(string country);
        decimal GetBalanceOfAccounts(string country);
        void AddTransaction(int accountId, decimal amount, bool withdraw, string symbol);
        Account GetAccount(int accountId);
        Account GetAccount(Transaction transaction);
        void AddAccount(int customerId);
        void Activate(int id, bool customer);
        void Deactivate(int accountId);
        string GetAccountOwner(int accountId);
        ResponseCode Withdraw(int accountId, decimal amount);
        ResponseCode Deposit(int accountId, decimal amount);
        void Update();
    }
}
