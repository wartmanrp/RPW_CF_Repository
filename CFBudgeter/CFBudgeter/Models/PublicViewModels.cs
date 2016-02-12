using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFBudgeter.Models
{
    public class DashboardViewModel
    {
        public Household Household { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Transaction> RecentTransactions { get; set; }
        public Budget Budget { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
    }
}