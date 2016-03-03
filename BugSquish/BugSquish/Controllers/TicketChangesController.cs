using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugSquish.Models;

namespace BugSquish.Controllers
{
    public class TicketChangesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketChanges
        public ActionResult Index()
        {
            var ticketChanges = db.TicketChanges.Include(t => t.Editor).Include(t => t.Ticket);
            return View(ticketChanges.ToList());
        }

        // GET: TicketChanges/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketChange ticketChange = db.TicketChanges.Find(id);
            if (ticketChange == null)
            {
                return HttpNotFound();
            }
            return View(ticketChange);
        }

        // GET: TicketChanges/Create
        public ActionResult Create()
        {
            ViewBag.EditorId = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "AuthorId");
            return View();
        }

        // POST: TicketChanges/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TicketId,EditorId,EditedTime,Property,OldValue,NewValue")] TicketChange ticketChange)
        {
            if (ModelState.IsValid)
            {
                db.TicketChanges.Add(ticketChange);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.EditorId = new SelectList(db.Users, "Id", "FirstName", ticketChange.EditorId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "AuthorId", ticketChange.TicketId);
            return View(ticketChange);
        }

        // GET: TicketChanges/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketChange ticketChange = db.TicketChanges.Find(id);
            if (ticketChange == null)
            {
                return HttpNotFound();
            }
            ViewBag.EditorId = new SelectList(db.Users, "Id", "FirstName", ticketChange.EditorId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "AuthorId", ticketChange.TicketId);
            return View(ticketChange);
        }

        // POST: TicketChanges/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TicketId,EditorId,EditedTime,Property,OldValue,NewValue")] TicketChange ticketChange)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketChange).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.EditorId = new SelectList(db.Users, "Id", "FirstName", ticketChange.EditorId);
            ViewBag.TicketId = new SelectList(db.Tickets, "Id", "AuthorId", ticketChange.TicketId);
            return View(ticketChange);
        }

        // GET: TicketChanges/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketChange ticketChange = db.TicketChanges.Find(id);
            if (ticketChange == null)
            {
                return HttpNotFound();
            }
            return View(ticketChange);
        }

        // POST: TicketChanges/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketChange ticketChange = db.TicketChanges.Find(id);
            db.TicketChanges.Remove(ticketChange);
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
