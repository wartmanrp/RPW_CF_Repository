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
    public class ProjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Projects
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                var projects = db.Projects.ToList();
                return View(projects);
            }

            if (User.IsInRole("ProjectManager"))
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                var projects = db.Projects.Where(p => p.Manager.Id == user.Id).ToList();
                return View(projects);
            }

            if (User.IsInRole("Developer"))
            {
                var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
                
                var projects = user.Projects.ToList();
                return View(projects);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: Projects/Details/5
        [Authorize(Roles = "Admin,ProjectManager,Developer")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            var helper = new UserRolesHelper(db);
            var managers = helper.UsersInRole("ProjectManager").ToList();


            ViewBag.ManagerId = new SelectList(managers, "Id", "Name");
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ManagerId,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Projects.Add(project);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ManagerId = new SelectList(db.Users, "Id", "FirstName", project.ManagerId);
            return View(project);
        }

        // GET: Projects/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (!User.IsInRole("Admin"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            var helper = new UserRolesHelper(db);
            var managers = helper.UsersInRole("ProjectManager").ToList();


            ViewBag.ManagerId = new SelectList(managers, "Id", "Name", project.ManagerId);
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ManagerId,Name,Description")] Project project)
        {
            if (ModelState.IsValid)
            {
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ManagerId = new SelectList(db.Users, "Id", "FirstName", project.ManagerId);
            return View(project);
        }

        // GET: Projects/ProjectUsers
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult ProjectUsers(int id)
        {
            var helper = new UserRolesHelper(db);
            var managers = helper.UsersInRole("ProjectManager").ToList();
            var developers = helper.UsersInRole("Developer").ToList();
            var currentproject = db.Projects.Single(p => p.Id == id);

            var project = new ProjectUsersViewModel
            {
                ProjectId = currentproject.Id,
                Name = currentproject.Name,
                Description = currentproject.Description,
                ProjectDevelopers = currentproject.Developers.ToList(),
                ProjectManager = currentproject.Manager,
                CurrentDevelopers = currentproject.Developers.Select(d => d.Id).ToArray(),
                CurrentManager = currentproject.Manager.Id,
                AvailableDevelopers = new SelectList(developers, "Id", "Name"),
                AvailableManagers = new SelectList(managers, "Id", "Name") 
            };

            return View(project);

        }

        //POST: Projects/ProjectUsers
        [Authorize(Roles = "Admin,ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProjectUsers(ProjectUsersViewModel project)
        {
            var helper = new ProjectUsersHelper();
            foreach (var id in project.CurrentDevelopers)
            {
                helper.AddUserToProject(id, project.ProjectId);
            }                
            helper.AddUserToProject(project.CurrentManager, project.ProjectId);
            return RedirectToAction("Details", "Projects", new { id = project.ProjectId });
        }


        //GET: Projects/RemoveUsers
        [Authorize(Roles = "Admin,ProjectManager")]
        public ActionResult RemoveUsers(int id)
        {
            var helper = new UserRolesHelper(db);
            var managers = helper.UsersInRole("ProjectManager").ToList();
            var developers = helper.UsersInRole("Developer").ToList();
            var currentproject = db.Projects.Single(p => p.Id == id);

            var project = new ProjectUsersViewModel
            {
                ProjectId = currentproject.Id,
                Name = currentproject.Name,
                Description = currentproject.Description,
                ProjectDevelopers = currentproject.Developers.ToList(),
                ProjectManager = currentproject.Manager,
                CurrentDevelopers = currentproject.Developers.Select(d => d.Id).ToArray(),
                AvailableDevelopers = new SelectList(developers, "Id", "Name"),
            };

            return View(project);
        }

        //POST: Projects/RemoveUsers
        [Authorize(Roles = "Admin,ProjectManager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RemoveUsers(ProjectUsersViewModel project)
        {
            var helper = new ProjectUsersHelper();
            foreach (var id in project.CurrentDevelopers)
            {
                helper.RemoveUserFromProject(id, project.ProjectId);
            }
            return RedirectToAction("Details", "Projects", new { id = project.ProjectId});
        }


        // GET: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Project project = db.Projects.Find(id);
            if (project == null)
            {
                return HttpNotFound();
            }
            return View(project);
        }

        // POST: Projects/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Project project = db.Projects.Find(id);
            db.Projects.Remove(project);
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
