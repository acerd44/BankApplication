using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary.ViewModels
{
    public class APICustomerViewModel
    {
        public CustomerCardViewModel CustomerInfo { get; set; } = null!;
        public List<APIAccountViewModel>? Accounts { get; set; }
    }
}
