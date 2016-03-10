using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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

        public bool AddUserToProject(string userId, int projectId)
        {
            var projects = userManager.GetRoles(userId);
            foreach (var role in roles)
            {
                RemoveUserFromRole(userId, role);
            }
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        //public bool RemoveUserFromRole(string userId, string roleName)
        //{
        //    var result = userManager.RemoveFromRole(userId, roleName);
        //    return result.Succeeded;
        //}

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

        public class ProjectUsersViewModel
        {
            public string Id { get; set; }
            public List<ApplicationUser> Developers { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Role { get; set; }
        }
    }
}