using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
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
        List<Account> GetAccountsOfCustomer(int customerId);
        List<Account> GetTopTenAccountsOfCountry(string country);
        List<Transaction> GetMoreTransactions(int accountId, int pageNo);
        void AddTransaction(int accountId, decimal amount, bool withdraw, string symbol);
        Account GetAccount(int accountId);
        string GetAccountOwner(int accountId);
        ResponseCode Withdraw(int accountId, decimal amount);
        ResponseCode Deposit(int accountId, decimal amount);
        void Update();
    }
}
