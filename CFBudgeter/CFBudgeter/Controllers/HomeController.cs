﻿using System;
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
            if (User.Identity.IsAuthenticated)
            {
                var db = new ApplicationDbContext();
                var model = new DashboardViewModel();

                model.Household = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;
                if (model.Household.Budgets.Count() == 0)
                {
                    return RedirectToAction("Create", "Budgets");
                }

                if (id == null)
                {
                    var activeBudget = db.Budgets.First(b => b.HouseholdId == model.Household.Id);
                    model.HouseholdBudgets = new SelectList(model.Household.Budgets, "Id", "Name", activeBudget.Id);
                    model.CurrentBudgetId = activeBudget.Id;
                    model.CurrentBudget = activeBudget;
                }
                else
                {
                    model.HouseholdBudgets = new SelectList(model.Household.Budgets, "Id", "Name", id);
                    model.CurrentBudgetId = (int)id;
                    model.CurrentBudget = db.Budgets.First(b => b.Id == id);
                }



                //var today = DateTime.Now;

                //model.BudgetPeriodTransactions = db.Accounts.SelectMany(a => a.Transactions).Where(t => t.Date.Year == today.Year && t.Date.Month == today.Month).ToList();
                //var TransactionsByCategory = model.BudgetPeriodTransactions.Where(b => b.CategoryId = Category).Sum(t => t.Amount);

                model.Accounts = db.Accounts.Where(a => a.HouseholdId == model.Household.Id).Take(5).ToList();
                var accountIds = model.Accounts.Select(a => a.Id).ToList();
                model.RecentTransactions = db.Transactions.Where(t => accountIds.Contains(t.AccountId)).OrderByDescending(i => i.Id).Take(5).ToList();


                model.Invitations = db.Invitations.Where(e => e.ToEmail == User.Identity.Name).ToList();
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        //POST: Index Submit
        //This action allows you to switch between budgets on the dashboard.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectBudget(DashboardViewModel model)
        {            
            return RedirectToAction("Index", "Home", new { id = model.CurrentBudgetId});
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