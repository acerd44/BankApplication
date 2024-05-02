using BankLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankGuardian
{
    public class TransactionÍnfo
    {
        public Transaction Transaction { get; set; }
        public decimal Amount { get; set; }
        public string Country { get; set; }
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
    }
}
