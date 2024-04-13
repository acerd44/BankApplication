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
        List<Account> GetAccounts(int customerId);
        List<Account> GetTop10Accounts(string country);
        List<Transaction> GetMoreTransactionse(int accountId, int pageNo);
        Account GetAccount(int accountId);
        string GetAccountOwner(int accountId);
        ResponseCode Withdraw(int accountId, decimal amount);
        ResponseCode Deposit(int accountId, decimal amount);
        void Update();
    }
}
