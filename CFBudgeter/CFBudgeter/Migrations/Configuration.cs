namespace CFBudgeter.Migrations
{
    using CFBudgeter.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;


    internal sealed class Configuration : DbMigrationsConfiguration<CFBudgeter.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(CFBudgeter.Models.ApplicationDbContext context)
        {
            //create default categories
            string[] categories = { "Automobile", "Bank Charges", "Charity", "Childcare", "CLothing", "Credit Car Fees", "Education",
                                  "Events", "Food", "Gifts", "Healthcare", "Household", "Insurance", "Job Expenses", "Leisure (daily)", 
                                  "Hobbies", "Loans", "Pet Care", "Savings", "Taxes", "Utilities", "Vacation" };

            // add default categories if no present
            if (context.Categories.Count() == 0)
            {
                foreach (var c in categories)
                {
                    context.Categories.Add(new Category { Name = c });
                }
            }

            //check for demo household, add if not present
                        if (!context.Households.Any(h => h.Name.Equals("Demo Household")))
            {
                var household = context.Households.Add(new Household { Name = "Demo Household"});
            }

            //check for primary household, add if not present
            if (!context.Households.Any(h => h.Name.Equals("Pelly-Wartman Household")))
            {
                var household = context.Households.Add(new Household { Name = "Pelly-Wartman Household"});
            


            var uStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(uStore);



            //creates new user
            if (!context.Users.Any(u => u.Email == "powers.wartman@gmail.com"))
            {
               
                userManager.Create(new ApplicationUser
                    {
                        UserName = "powers.wartman@gmail.com",
                        Email = "powers.wartman@gmail.com",
                        FirstName = "Powers",
                        LastName = "Wartman",
                        HouseholdId = household.Id,
                        Household = household
                    }, "Password-1");
            }
        }
            

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
