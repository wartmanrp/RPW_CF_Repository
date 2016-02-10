using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CFBudgeter.Models
{

    public class Household
    {
        public Household()
        {
            this.Members = new HashSet<ApplicationUser>();
            this.Accounts = new HashSet<Account>();
            this.Budgets = new HashSet<Budget>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ApplicationUser> Members { get; set;}
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Budget> Budgets { get; set; }

    }

    public class Account
    {
        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public int HouseholdId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreationDate { get; set; }     
        public double Balance { get; set; }
        public double ReconciledBalance { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }

    }

    public class Transaction
    {
        public Transaction()
        {
            this.Users = new HashSet<ApplicationUser>();
            this.Accounts = new HashSet<Account>();
            this.Categories = new HashSet<Category>();
        }
        public int Id { get; set; }
        public string Descriptions { get; set; }
        public string Type { get; set; }
        public DateTimeOffset Date { get; set; }
        public double Amount { get; set; }
        public bool Reconciled { get; set; }
        public double ReconciledAmount { get; set; }

        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }

    }

    public class Category
    {
        public Category()
        {
            this.Households = new HashSet<Household>();
            this.Transactions = new HashSet<Transaction>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Household> Households{ get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }

    public class Budget
    {
        public Budget()
        {
            this.Households = new HashSet<Household>();
            this.BudgetItems = new HashSet<BudgetItem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Household> Households { get; set; }
        public virtual ICollection<BudgetItem> BudgetItems { get; set; }
    }

    public class BudgetItem
    {
        public BudgetItem()
        {
            this.Categories = new HashSet<Category>();
            this.Budgets = new HashSet<Budget>();
        }

        public int Id { get; set; }
        public double Amount { get; set; }

        public virtual ICollection<Budget> Budgets { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }

    public class Invitation
    {
        public Invitation()
        {
            this.Households = new HashSet<Household>();
        }

        public int Id { get; set; }
        public string ToEmail { get; set; }

        public virtual ICollection<Household> Households { get; set; }
    }
}
