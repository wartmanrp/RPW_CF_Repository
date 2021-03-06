﻿using System;
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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        [Authorize]
        public ActionResult Index(string userId)
        {
            return RedirectToAction("Index", "HouseholdAccounts");
        }

        // GET: Transactions/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            var accountsIds = db.Users.FirstOrDefault(
                u => u.UserName == User.Identity.Name)
                .Household.Accounts.Select(a => a.Id).ToList();
            if (accountsIds.Contains(transaction.AccountId))
            {
            return View(transaction);
            }
            return HttpNotFound();

        }

        // GET: Transactions/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            var houseId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId;
            ViewBag.Categories = new SelectList(db.Households.Find(houseId).Categories.ToList(), "Id", "Name");
          
            var model = new Transaction { AccountId = id };

            return View(model);
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AccountId,UserId,CategoryId,Descriptions,Type,Date,Amount,Reconciled,ReconciledAmount")] Transaction transaction)
        {
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            transaction.UserId = currentUser.Id;
            if (ModelState.IsValid)
            {
                transaction.Date = new DateTimeOffset(DateTime.Now);
                var account = db.Accounts.FirstOrDefault(a => a.Id == transaction.AccountId);
                if (transaction.Type == true)
                {
                    if (transaction.Reconciled == true)
                    {
                        account.Balance += transaction.Amount;
                        account.ReconciledBalance += transaction.Amount;
                    } else
                    {
                        account.Balance += transaction.Amount;
                    }
                } else
                {
                    if (transaction.Reconciled == true)
                    {
                        account.Balance -= transaction.Amount;
                        account.ReconciledBalance -= transaction.Amount;
                    } else
                    {
                        account.Balance -= transaction.Amount;
                    }

                }
                
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Details", "HouseholdAccounts", new { id = transaction.AccountId});
            }

            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name", transaction.CategoryId);
            var accountsIds = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)
                .Household
                .Accounts
                .Select(a => a.Id)
                .ToList();
            TempData["originalAmt"] = db.Transactions.FirstOrDefault(t => t.Id == transaction.Id).Amount;
            if (accountsIds.Contains(transaction.AccountId))
            {
                return View(transaction);
            }
            return HttpNotFound();
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AccountId,UserId,CategoryId,Descriptions,Type,Date,Amount,Reconciled,ReconciledAmount")] Transaction transaction)
        {
            transaction.UserId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
            var originalAmt = (decimal)TempData["originalAmt"];

            if (ModelState.IsValid)
            {
                var account = db.Accounts.FirstOrDefault(a => a.Id == transaction.AccountId);

                if (transaction.Type == true)
                {
                    if (transaction.Reconciled == true)
                    {
                        account.Balance -= originalAmt;
                        account.ReconciledBalance -= originalAmt;
                        account.Balance += transaction.Amount;
                        account.ReconciledBalance += transaction.Amount;
                    }
                    else
                    {                        
                        if (transaction.Amount == originalAmt)
                        {
                            account.Balance -= originalAmt;
                            account.Balance += transaction.Amount;
                            account.ReconciledBalance -= originalAmt;
                        }
                        else
                        {
                            account.ReconciledBalance -= transaction.Amount;
                        }
                    }
                }
                else
                {
                    if (transaction.Reconciled == true)
                    {
                        account.Balance += originalAmt;
                        account.ReconciledBalance += originalAmt;
                        account.Balance -= transaction.Amount;
                        account.ReconciledBalance -= transaction.Amount;
                    }
                    else
                    {
                        if (transaction.Amount == originalAmt)
                        {
                            account.Balance += originalAmt;
                            account.Balance -= transaction.Amount;
                            account.ReconciledBalance += originalAmt;
                        }
                        else
                        {
                            account.ReconciledBalance += transaction.Amount;
                        }                      
                    }
                }

                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "HouseholdAccounts", new { id = transaction.AccountId });
            }
            ViewBag.AccountId = new SelectList(db.Accounts, "Id", "Name", transaction.AccountId);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            var accountsIds = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name)
                .Household
                .Accounts
                .Select(a => a.Id)
                .ToList();
            if (accountsIds.Contains(transaction.AccountId))
            {
                return View(transaction);
            }
            return HttpNotFound();
        }

        // POST: Transactions/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            var account = db.Accounts.FirstOrDefault(a => a.Id == transaction.AccountId);

            if (transaction.Type == true)
            {
                if (transaction.Reconciled == true)
                {
                    account.Balance -= transaction.Amount;
                    account.ReconciledBalance -= transaction.Amount;
                }
                else
                {
                    account.Balance -= transaction.Amount;
                }
            }
            else
            {
                if (transaction.Reconciled == true)
                {
                    account.Balance += transaction.Amount;
                    account.ReconciledBalance += transaction.Amount;
                }
                else
                {
                    account.Balance += transaction.Amount;
                }

            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Details", "HouseholdAccounts", new { id = transaction.AccountId });
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
