namespace BugSquish.Migrations
{
    using BugSquish.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BugSquish.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        //check for roles, if not extant, creates them.
        protected override void Seed(BugSquish.Models.ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(context));

            //create admin role if it doesn't exist
            if (!roleManager.RoleExists("Admin"))
                if (!context.Roles.Any(r => r.Name == "Admin"))
                {
                    roleManager.Create(new IdentityRole { Name = "Admin" });
                }

            //create manager role if it doesn't exist
            if (!roleManager.RoleExists("ProjectManager"))
                if (!context.Roles.Any(r => r.Name == "ProjectManager"))
                {
                    roleManager.Create(new IdentityRole { Name = "ProjectManager" });
                }

            //create dev role if it doesn't exist
            if (!roleManager.RoleExists("Developer"))
                if (!context.Roles.Any(r => r.Name == "Developer"))
                {
                    roleManager.Create(new IdentityRole { Name = "Developer" });
                }

            //create submitter role if it doesn't exist
            if (!roleManager.RoleExists("Submitter"))
                if (!context.Roles.Any(r => r.Name == "Submitter"))
                {
                    roleManager.Create(new IdentityRole { Name = "Submitter" });
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
            if (userManager.FindByEmail("ajensen@coderfoundry.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "ajensen@coderfoundry.com",
                    Email = "ajensen@coderfoundry.com",
                    FirstName = "Andrew",
                    LastName = "Jensen"
                }, "Password-1");
            }
            //assigns person to given role (admin || moderator), if not already in it.
            userId = userManager.FindByEmail("ajensen@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "ProjectManager"))
            {
                userManager.AddToRole(userId, "ProjectManager");
            }

            //creates demo user 1 and assigns to admin role
            if (userManager.FindByEmail("demo1@coderfoundry.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demo1@coderfoundry.com",
                    Email = "demo1@coderfoundry.com",
                    FirstName = "Admin",
                    LastName = "User"
                }, "Password-1");
            }
            userId = userManager.FindByEmail("demo1@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "Admin"))
            {
                userManager.AddToRole(userId, "Admin");
            }
            //end demo user 1

            //creates demo user 2 and assigns to project manager role
            if (userManager.FindByEmail("demo2@coderfoundry.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demo2@coderfoundry.com",
                    Email = "demo2@coderfoundry.com",
                    FirstName = "Project Manager",
                    LastName = "User"
                }, "Password-1");
            }
            userId = userManager.FindByEmail("demo2@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "ProjectManager"))
            {
                userManager.AddToRole(userId, "ProjectManager");
            }
            //end demo user 2

            //creates demo user 3 and assigns to developer role
            if (userManager.FindByEmail("demo3@coderfoundry.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demo3@coderfoundry.com",
                    Email = "demo3@coderfoundry.com",
                    FirstName = "Developer",
                    LastName = "User"
                }, "Password-1");
            }
            userId = userManager.FindByEmail("demo3@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "Developer"))
            {
                userManager.AddToRole(userId, "Developer");
            }
            //end demo user 3

            //creates demo user 4 and assigns to submitter role
            if (userManager.FindByEmail("demo4@coderfoundry.com") == null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = "demo4@coderfoundry.com",
                    Email = "demo4@coderfoundry.com",
                    FirstName = "Submitter",
                    LastName = "User"
                }, "Password-1");
            }

            userId = userManager.FindByEmail("demo4@coderfoundry.com").Id;
            if (!userManager.IsInRole(userId, "Submitter"))
            {
                userManager.AddToRole(userId, "Submitter");
            }
            //end demo user 4

            //The following will be used to seed the database with default types, priorities, status, and demo projects.

            // add default types if no present
            if (context.TicketTypes.Count() == 0)
            {
                context.TicketTypes.Add(new TicketType { Name = "Bug" });
                context.TicketTypes.Add(new TicketType { Name = "Feature" });
                context.TicketTypes.Add(new TicketType { Name = "Enhancement" });
            }

            // add default priorities if no present
            if (context.Priorities.Count() == 0)
            {
                context.Priorities.Add(new Priority { Name = "Low" });
                context.Priorities.Add(new Priority { Name = "Moderate" });
                context.Priorities.Add(new Priority { Name = "High" });
                context.Priorities.Add(new Priority { Name = "Critical" });

            }
            // add default statuses if no present
            if (context.Statuses.Count() == 0)
            {
                context.Statuses.Add(new Status { Name = "Open" });
                context.Statuses.Add(new Status { Name = "Pending" });
                context.Statuses.Add(new Status { Name = "Closed" });
                context.Statuses.Add(new Status { Name = "Needs Review" });

            }
            context.SaveChanges();

            //check for demo projects, add if not present
            if (!context.Projects.Any(h => h.Name.Equals("Demo Project 1")))
            {
                var developer = context.Users.Single(u => u.UserName == "demo3@coderfoundry.com");
                var submitter = context.Users.Single(u => u.UserName == "demo4@coderfoundry.com");
                var manager = context.Users.Single(u => u.UserName == "demo2@coderfoundry.com");
                var project = context.Projects.Add(new Project
                {
                    Name = "Demo Project 1",
                    ManagerId = manager.Id,
                    Manager = manager,
                    Description = "This is a demo project meant to give an idea to potential users of what this app will look like."
                });

                //assigns demo dev to demo project
                project.Developers.Add(developer);

                //adds ticket to demo project with demo dev, submitter

                var tType = context.TicketTypes.Single(t => t.Name == "Bug").Id;
                var tPriority = context.Priorities.Single(p => p.Name == "Moderate").Id;
                var tStatus = context.Statuses.Single(s => s.Name == "Pending").Id;

                project.Tickets.Add(new Ticket
                {
                    AuthorId = submitter.Id,
                    Author = submitter,
                    DeveloperId = developer.Id,
                    Developer = developer,
                    Title = "Couldn't log in",
                    Notes = "I tried to log in, but was unable to! Help!",
                    Created = DateTime.Now,
                    TicketTypeId = tType,
                    PriorityId = tPriority,
                    StatusId = tStatus
                });
            }
            context.SaveChanges();
        }
    }
}
