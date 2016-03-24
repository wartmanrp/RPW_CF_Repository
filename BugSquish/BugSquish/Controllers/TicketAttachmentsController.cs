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
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        public ActionResult Index()
        {
            var ticketAttachments = db.TicketAttachments.Include(t => t.Author).Include(t => t.Ticket);
            return View(ticketAttachments.ToList());
        }

        // GET: TicketAttachments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        public ActionResult Create(int id)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;

            var attachment = new TicketAttachment
            {
                AuthorId = user,
                TicketId = id

            };
            return RedirectToAction("Details", "Tickets", id);
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AuthorId,TicketId,AttachmentUrl,Notes,Created")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                ticketAttachment.Created = new DateTimeOffset(DateTime.Now);
                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ticketAttachment);
        }

        //// GET: TicketAttachments/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
        //    if (ticketAttachment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.AuthorId);
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "AuthorId", ticketAttachment.TicketId);
        //    return View(ticketAttachment);
        //}

        //// POST: TicketAttachments/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "Id,AuthorId,TicketId,AttachmentUrl,Notes,Created")] TicketAttachment ticketAttachment)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(ticketAttachment).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.AuthorId);
        //    ViewBag.TicketId = new SelectList(db.Tickets, "Id", "AuthorId", ticketAttachment.TicketId);
        //    return View(ticketAttachment);
        //}

        // GET: TicketAttachments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
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
