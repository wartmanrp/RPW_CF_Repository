using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BugSquish.Models
{
    public class ProjectUsersHelper
    {
        

        //private ApplicationDbContext db;

        //public ProjectUsersHelper(ApplicationDbContext context)
        //{
        //    this.db = context;
        //}

        //public bool IsDeveloperInProject(string userId, int ProjectId)
        //{
        //    db.Projects.Where(p => p.Id == ProjectId);
        //    var helper = new UserRolesHelper(db);

        //    var devs = db.Users.Where(u => u.Roles.Select("ProjectManager"))

        //    db.Projects.Where(p => p.Developers.Any)
        //    return IsDeveloperInProject();
        //}

        //public string GetUserRole(string userId)
        //{
        //    return userManager.GetRoles(userId).First();
        //}

        public void AddUserToProject(string userId, int projectId)
        {
            var db = new ApplicationDbContext();
            var project = db.Projects.Single(p => p.Id == projectId);
            var user = db.Users.Single(u => u.Id == userId);
            var helper = new UserRolesHelper(db);

            if (helper.IsUserInRole(userId, "ProjectManager") == true){
                project.ManagerId = userId;
                db.Entry(project).State = EntityState.Modified;
                db.SaveChanges();
            }

            if (helper.IsUserInRole(userId, "Developer") == true)
            {
                project.Developers.Add(user);
                db.SaveChanges();
            }
        }

        public void RemoveUserFromProject(string userId, int projectId)
        {
            var db = new ApplicationDbContext();
            var project = db.Projects.Single(p => p.Id == projectId);
            var user = db.Users.Single(u => u.Id == userId);
            var helper = new UserRolesHelper(db);
        
            project.Developers.Remove(user);
            db.SaveChanges();

        }

        //public ICollection<UserDropDownViewModel> UsersInRole(string roleName)
        //{
        //    var userIDs = roleManager.FindByName(roleName).Users.Select(r => r.UserId);
        //    return userManager.Users.Where(u => userIDs.Contains(u.Id)).Select(u =>
        //        new UserDropDownViewModel { Id = u.Id, Name = u.UserName }).ToList();
        //}

        //public IList<UserDropDownViewModel> UsersNotInRole(string roleName)
        //{
        //    var userIDs = System.Web.Security.Roles.GetUsersInRole(roleName);
        //    return userManager.Users.Where(u => !userIDs.Contains(u.Id)).Select(u =>
        //        new UserDropDownViewModel { Id = u.Id, Name = u.UserName }).ToList();
        //}

    }
}