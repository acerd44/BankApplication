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
        Account GetAccount(int accountId);
        ResponseCode Withdraw(int accountId, decimal amount);
        ResponseCode Deposit(int accountId, decimal amount);
        void Update();
    }
}
