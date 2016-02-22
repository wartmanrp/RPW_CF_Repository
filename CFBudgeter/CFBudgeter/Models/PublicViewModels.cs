using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CFBudgeter.Models
{
    public class DashboardViewModel
    {
        public Household Household { get; set; }
        public ICollection<Account> Accounts { get; set; }
        public ICollection<Transaction> RecentTransactions { get; set; }

        public Budget CurrentBudget { get; set; }
        public SelectList HouseholdBudgets { get; set; }
        public ICollection<Invitation> Invitations { get; set; }
    }
}