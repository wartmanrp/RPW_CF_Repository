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
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (User.IsInRole("Admin"))
            {
                var tickets = db.Tickets.ToList();
                return View(tickets);
            }
            
            if (User.IsInRole("ProjectManager"))
            {
                var projects = db.Projects.Where(p => p.ManagerId == user.Id);


                var tickets = db.Tickets.Include(t => t.Author).Include(t => t.Developer).Include(t => t.Priority).Include(t => t.Project).Include(t => t.Status).Include(t => t.TicketType).Where(p => p.ProjectManagerId == user.Id).OrderByDescending(i => i.Created).ToList();
                return View(tickets);
            }
            
            if (User.IsInRole("Developer"))
            {
                var tickets = db.Tickets.Include(t => t.Author).Include(t => t.Developer).Include(t => t.Priority).Include(t => t.Project).Include(t => t.Status).Include(t => t.TicketType).Where(p => p.DeveloperId == user.Id).OrderByDescending(i => i.Created).ToList();
                return View(tickets);
            }

            if (User.IsInRole("Submitter"))
            {
                var tickets = db.Tickets.Where(t => t.AuthorId == user.Id).OrderByDescending(i => i.Created).ToList();
                return View(tickets);
            }

            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        public ActionResult Create()
        {
            var helper = new UserRolesHelper(db);
            var managers = helper.UsersInRole("ProjectManager").ToList();
            var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;
            


            //ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName");
            //ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name");
            //ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ManagerId");
            //ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name");
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name");
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "Name");

            var model = new Ticket
            {
                AuthorId = user
            };
                //ProjectManagerId = project.ManagerId,
                //ProjectManager = project.Manager};

            return View(model);
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,AuthorId,ProjectId,TicketTypeId,Title,Notes,Created,")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //finds the project in the database
                var manager = db.Projects.Single(p => p.Id == ticket.ProjectId);
                var status = db.Statuses.Single(s => s.Name == "Needs Review");
                //assigns manager to ticket
                ticket.ProjectManager = manager.Manager;
                ticket.ProjectManagerId = manager.ManagerId;
                //sets date
                ticket.Created = new DateTimeOffset(DateTime.Now);
                ticket.StatusId = status.Id;
                ticket.Status = status;
                
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticket.AuthorId);
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ManagerId", ticket.ProjectId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", ticket.StatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticket.AuthorId);
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ManagerId", ticket.ProjectId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", ticket.StatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AuthorId,DeveloperId,ProjectId,TicketTypeId,PriorityId,StatusId,Title,Notes,Created,Updated")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorId = new SelectList(db.Users, "Id", "FirstName", ticket.AuthorId);
            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.PriorityId);
            ViewBag.ProjectId = new SelectList(db.Projects, "Id", "ManagerId", ticket.ProjectId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", ticket.StatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
