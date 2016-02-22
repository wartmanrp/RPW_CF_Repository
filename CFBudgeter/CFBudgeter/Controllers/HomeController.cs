using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CFBudgeter.Models;

namespace CFBudgeter.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index(int? id)
        {
            var db = new ApplicationDbContext();
            var model = new DashboardViewModel();

            model.Household = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;

            if (id == null)
            {
            var activeBudget = db.Budgets.First(b => b.HouseholdId == model.Household.Id);
            model.HouseholdBudgets = new SelectList(model.Household.Budgets, "Id", "Name", activeBudget.Id);
            model.CurrentBudget = activeBudget;
            }




            //var today = DateTime.Now;

            //model.BudgetPeriodTransactions = db.Accounts.SelectMany(a => a.Transactions).Where(t => t.Date.Year == today.Year && t.Date.Month == today.Month).ToList();
            //var TransactionsByCategory = model.BudgetPeriodTransactions.Where(b => b.CategoryId = Category).Sum(t => t.Amount);

            model.Accounts = db.Accounts.Where(a => a.HouseholdId == model.Household.Id).ToList();
            var accountIds = model.Accounts.Select(a => a.Id).ToList();
            model.RecentTransactions = db.Transactions.Where(t => accountIds.Contains(t.AccountId)).OrderByDescending(i => i.Id).Take(5).ToList();


            model.Invitations = db.Invitations.Where(e => e.ToEmail == User.Identity.Name).ToList();
            return View(model);
        }

        //POST: Index Submit
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectBudget(int Id)
        {
            
            
            model.CurrentBudget.Id = Id;
             
            return View(model);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}