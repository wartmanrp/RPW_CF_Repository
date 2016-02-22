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
    public class HouseholdAccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        [Authorize]
        public ActionResult Index()
        {
            //restrict to current household
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;
            var accounts = db.Accounts.Where(a => a.HouseholdId == currentUser);
            return View(accounts.ToList());
        }

        // GET: Accounts/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentHousehold = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;

            Account account = currentHousehold.Accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        [Authorize]
        public ActionResult Create()
        {
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Balance")] Account account)
        {
            account.HouseholdId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;
            if (ModelState.IsValid)
            {
                account.ReconciledBalance = account.Balance;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index", "HouseholdAccounts");
            }

            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            return View(account);
        }

        // GET: Accounts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentHousehold = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;

            Account account = currentHousehold.Accounts.FirstOrDefault(a => a.Id == id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Balance,ReconciledBalance")] Account account)
        {
            account.HouseholdId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;

            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdId = new SelectList(db.Households, "Id", "Name", account.HouseholdId);
            return View(account);
        }

        // GET: Accounts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentHousehold = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;

            Account account = currentHousehold.Accounts.FirstOrDefault(a => a.Id == id); 
            
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var currentHousehold = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;

            Account account = currentHousehold.Accounts.FirstOrDefault(a => a.Id == id);
            db.Accounts.Remove(account);
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
