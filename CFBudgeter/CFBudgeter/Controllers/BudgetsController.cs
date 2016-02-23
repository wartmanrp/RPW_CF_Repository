using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CFBudgeter.Models;

namespace CFBudgeter.Controllers
{
    [RequireHttps]
    public class BudgetsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budgets
        [Authorize]
        public ActionResult Index()
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;
            var budgets = db.Budgets.Where(b => b.HouseholdId == currentUser);

            return View(budgets.ToList());
        }

        // GET: Budgets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentHousehold = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;

            Budget budget = currentHousehold.Budgets.FirstOrDefault(a => a.Id == id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // GET: Budgets/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Budgets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,HouseholdId,Name")] Budget budget)
        {
            budget.HouseholdId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;

            if (ModelState.IsValid)
            {
                db.Budgets.Add(budget);
                db.SaveChanges();

                var userCats = db.Categories.Where(c => c.Households.FirstOrDefault().Id == budget.HouseholdId);
                if (userCats.Count() < 1)
                {
                    return RedirectToAction("Create", "Categories");
                }
                var userBudgetItems = db.BudgetItems.Count();
                if (userBudgetItems < 1)
                {
                    return RedirectToAction("Create", "BudgetItems", new { id = budget.Id });
                }
                return RedirectToAction("Index", "Budgets");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentHousehold = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;

            Budget budget = currentHousehold.Budgets.FirstOrDefault(a => a.Id == id);
            if (budget == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // POST: Budgets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,HouseholdId,Name")] Budget budget)
        {
            budget.HouseholdId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;

            if (ModelState.IsValid)
            {
                db.Entry(budget).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Budgets");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", budget.HouseholdId);
            return View(budget);
        }

        // GET: Budgets/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentHousehold = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;

            Budget budget = currentHousehold.Budgets.FirstOrDefault(a => a.Id == id);

            if (budget == null)
            {
                return HttpNotFound();
            }
            return View(budget);
        }

        // POST: Budgets/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var currentHousehold = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;

            Budget budget = currentHousehold.Budgets.FirstOrDefault(a => a.Id == id);
            db.Budgets.Remove(budget);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
