using System.Data.Entity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using CFblog.Models;

namespace CFblog.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<CFblog.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }


        //checks if roles exist, if not runs them.
        protected override void Seed(CFblog.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            if (!roleManager.RoleExists("Admin"))
            if(!context.Roles.Any(r => r.Name == "Admin"))
            {
                 roleManager.Create(new IdentityRole { Name = "Admin" });
            }
            if (!context.Roles.Any(r => r.Name == "Moderator")) 
            {
                roleManager.Create(new IdentityRole { Name = "Moderator" });
            }            

            var uStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(uStore);

            
            //creates new user
            if (userManager.FindByEmail("powers.wartman@gmail.com") == null)
            {
                userManager.Create(new ApplicationUser
                    {
                        UserName = "powers.wartman@gmail.com",
                        Email = "powers.wartman@gmail.com",
                        FirstName = "Powers",
                        LastName = "Wartman"
                    }, "Password-1");
            }

            //assigns person to given role (admin || moderator), if not already in it.
            var userId = userManager.FindByEmail("powers.wartman@gmail.com").Id;
            if (!userManager.IsInRole(userId, "Admin"))
            {
                userManager.AddToRole(userId, "Admin");
            }


            //creates new user
            if (userManager.FindByEmail("rmanglani@coderfoundry.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "rmanglani@coderfoundry.com",
                    Email = "rmanglani@coderfoundry.com",
                    FirstName = "Ria",
                    LastName = "Manglani"
                }, "Password-1");
            }

            //assigns person to given role (admin || moderator), if not already in it.
            userId = userManager.FindByEmail("rmanglani@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "Moderator"))
            {
                userManager.AddToRole(userId, "Moderator");
            }  
        }
    }
}