﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugSquish.Models
{
    public class UserRolesHelper
    {

        private ApplicationDbContext db;
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;

        public UserRolesHelper(ApplicationDbContext context)
        {
            this.userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            this.roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));
            this.db = context;
        }

        public bool IsUserInRole(string userId, string roleName)
        {
            return userManager.IsInRole(userId, roleName);
        }

        public string GetUserRole(string userId)
        {
            return userManager.GetRoles(userId).First();
        }

        public bool AddUserToRole(string userId, string roleName)
        {
            var roles = userManager.GetRoles(userId);
            foreach (var role in roles)
            {
                RemoveUserFromRole(userId, role);
            }
            var result = userManager.AddToRole(userId, roleName);
            return result.Succeeded;
        }

        public bool RemoveUserFromRole(string userId, string roleName)
        {
            var result = userManager.RemoveFromRole(userId, roleName);
            return result.Succeeded;
        }

        public ICollection<UserDropDownViewModel> UsersInRole(string roleName)
        {
            var userIDs = roleManager.FindByName(roleName).Users.Select(r => r.UserId);
            return userManager.Users.Where(u => userIDs.Contains(u.Id)).Select(u =>
                new UserDropDownViewModel { Id = u.Id, Name = u.UserName }).ToList();
        }

        public IList<UserDropDownViewModel> UsersNotInRole(string roleName)
        {
            var userIDs = System.Web.Security.Roles.GetUsersInRole(roleName);
            return userManager.Users.Where(u => !userIDs.Contains(u.Id)).Select(u =>
                new UserDropDownViewModel { Id = u.Id, Name = u.UserName }).ToList();
        }
    }
}