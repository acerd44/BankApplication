using BankLibrary.Infrastructure.Paging;
using BankLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ApplicationDbContext _context;
        public TransactionService(ApplicationDbContext context)
        {
            _context = context;
        }
        public IEnumerable<Transaction> GetTransactions(int accountId)
        {
            return _context.Accounts.Where(a => a.AccountId == accountId)
                .SelectMany(a => a.Transactions)
                .OrderByDescending(t => t.Date);
        }
        public List<Transaction> GetTransactions(string country)
        {
            return _context.Accounts.Where(a => a.Dispositions.Any(d => d.Customer.Country == country))
                .Distinct()
                .SelectMany(a => a.Transactions)
                .OrderByDescending(t => t.Date)
                .ToList();
        }
        public List<Transaction> GetMoreTransactions(int accountId, int pageNo)
        {
            return _context.Accounts
                .Where(a => a.AccountId == accountId)
                .SelectMany(a => a.Transactions)
                .OrderByDescending(t => t.Date)
                .GetPaged(pageNo, 20).Results
                .ToList();
        }
    }
}
