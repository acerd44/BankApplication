using BankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.Services
{
    public interface ITransactionService
    {
        IEnumerable<Transaction> GetTransactions(int accountId);
        List<Transaction> GetTransactions(string country);
        List<Transaction> GetMoreTransactions(int accountId, int pageNo);
    }
}
