using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugSquish.Models;
using Microsoft.AspNet.Identity;

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
                var tickets = db.Tickets.Where(t => t.DeveloperId == user.Id).ToList();
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
        public ActionResult Create([Bind(Include = "Id,AuthorId,ProjectId,TicketTypeId,ProjectManagerId,PriorityId,StatusId,Title,Notes,Created")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                //sets default user
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                //finds the project in the database, used to then get project manager for ticket
                var manager = db.Projects.Single(p => p.Id == ticket.ProjectId).ManagerId;

                //assigns default status and priority to the ticket
                var status = db.Statuses.Single(s => s.Name == "Needs Review");


                //assigns manager to ticket
                ticket.ProjectManagerId = manager;
                //sets properties
                ticket.Created = new DateTimeOffset(DateTime.Now);
                ticket.StatusId = status.Id;


 
                ticket.AuthorId = user.Id;
                
                
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
        [Authorize(Roles="Admin,ProjectManager,Developer")]
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


            ViewBag.DeveloperId = new SelectList(db.Users, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.PriorityId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", ticket.StatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }
        
        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,AuthorId,ProjectId,DeveloperID,TicketTypeId,ProjectManagerId,PriorityId,StatusId,Title,Notes,Created,Updated")]Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                var properties = new List<string>() {"Updated", "Description", "TicketTypeId", "TicketStatusId"};

                //gets ticket id and holds it for comparison
                var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.Id == ticket.Id);

                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Id;


                if (User.IsInRole("Admin") || User.IsInRole("ProjectManager"))
                {
                    //Check Ticket Priority
                    if (oldTicket.PriorityId != ticket.PriorityId)
                    {
                        var newUser = db.Users.Find(ticket.DeveloperId);
                        properties.Add("TicketPriorityId");
                        var ChangedPriority = new TicketChange{
                            TicketId = ticket.Id,
                            EditorId = user,
                            Property = "Ticket Priority",
                            OldValue = db.Priorities.FirstOrDefault(p => p.Id == oldTicket.PriorityId).Name,
                            NewValue = db.Priorities.FirstOrDefault(p => p.Id == ticket.PriorityId).Name,
                            EditedTime = System.DateTimeOffset.Now
                        };

                        db.TicketChanges.Add(ChangedPriority);

                        if(newUser != null{
                            var mailer = new EmailService();
                            var Notification = newUser != null ? new IdentityMessage(){
                                Subject= "You have a new Notification",
                                Destination = newUser.Email,
                                Body = "The priority for your ticket: " + ticket.Title + " has been changed.\n" + Environment.NewLine + " The new priority is: " + db.Priorities.FirstOrDefault(p => p.Id == ticket.PriorityId).Name } : null;
                                mailer.Send(Notification);
                        }
                    }
                }
                ticket.Updated = new DateTimeOffset(DateTime.Now);
                db.Entry(ticket).State = EntityState.Modified;
                //DbExtensions.Update(db,["Title","Notes"]);

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

        //GET: Tickets/Manage/5
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult Manage(int? id)
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
            var project = ticket.Project;
            var helper = new UserRolesHelper(db);

            var developers = project.Developers.ToList();

            ViewBag.DeveloperId = new SelectList(developers, "Id", "FirstName", ticket.DeveloperId);
            ViewBag.PriorityId = new SelectList(db.Priorities, "Id", "Name", ticket.PriorityId);
            ViewBag.StatusId = new SelectList(db.Statuses, "Id", "Name", ticket.StatusId);
            ViewBag.TicketTypeId = new SelectList(db.TicketTypes, "Id", "Name", ticket.TicketTypeId);
            return View(ticket);
        }

        //POST: Tickets/Manage/5
        [Authorize(Roles = "Admin,ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage([Bind(Include = "Id,AuthorId,ProjectId,TicketTypeId,DeveloperId,ProjectManagerId,PriorityId,StatusId,Title,Notes,Created,Updated")]Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.Updated = new DateTimeOffset(DateTime.Now);
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
        [Authorize(Roles = "Admin,ProjectManager")]
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
        [Authorize(Roles = "Admin,ProjectManager")]
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
