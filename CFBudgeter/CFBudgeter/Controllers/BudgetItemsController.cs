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
    public class BudgetItemsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: BudgetItems
        [Authorize]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Budgets");
        }

        // GET: BudgetItems/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }

            var budgetId = db.Users.FirstOrDefault(
                u => u.UserName == User.Identity.Name)
                .Household.Budgets.Select(b => b.Id).ToList();

            if (budgetId.Contains(budgetItem.BudgetId))
            {
                return View(budgetItem);
            }
            return HttpNotFound();
        }

        // GET: BudgetItems/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            var houseId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;
            ViewBag.Categories = new SelectList(db.Households.Find(houseId).Categories.ToList(), "Id", "Name");

            var model = new BudgetItem { BudgetId = id };
            
            return View(model);
        }

        // POST: BudgetItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,CategoryId,BudgetId,Amount")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {

                db.BudgetItems.Add(budgetItem);
                db.SaveChanges();
                return RedirectToAction("Details", "Budgets", new { id = budgetItem.BudgetId});
            }

            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", budgetItem.BudgetId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", budgetItem.BudgetId);
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name", budgetItem.CategoryId);
            var budgetIds = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)
                .Household
                .Budgets
                .Select(b => b.Id)
                .ToList();
            if (budgetIds.Contains(budgetItem.BudgetId))
            {
                return View(budgetItem);
            }
            return HttpNotFound();
        }

        // POST: BudgetItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,CategoryId,BudgetId,Amount")] BudgetItem budgetItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(budgetItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Budgets", new { id = budgetItem.BudgetId });
            }
            ViewBag.BudgetId = new SelectList(db.Budgets, "Id", "Name", budgetItem.BudgetId);
            return View(budgetItem);
        }

        // GET: BudgetItems/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var budgetItem = db.BudgetItems.Find(id);
            if (budgetItem == null)
            {
                return HttpNotFound();
            }

            var budgetIds = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)
                .Household
                .Budgets
                .Select(b => b.Id)
                .ToList();
            if(budgetIds.Contains(budgetItem.BudgetId))
            {
                return View(budgetItem);
            }
            return HttpNotFound();
        }

        // POST: BudgetItems/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BudgetItem budgetItem = db.BudgetItems.Find(id);
            db.BudgetItems.Remove(budgetItem);
            db.SaveChanges();
            return RedirectToAction("Details", "Budgets", new { id = budgetItem.BudgetId});
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
