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
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invitations
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Invitations.ToList());
        }

        // GET: Invitations/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ToEmail")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Invitations.Add(invitation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(invitation);
        }

        // GET: Invitations/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ToEmail")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invitation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
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
