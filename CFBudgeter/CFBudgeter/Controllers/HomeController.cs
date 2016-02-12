using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CFBudgeter.Models;

namespace CFBudgeter.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var db = new ApplicationDbContext();
            var model = new DashboardViewModel();
            model.Household = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).Household;
            model.RecentTransactions = db.Transactions.Take(5).ToList();
            model.Accounts = db.Accounts.Where(a => a.HouseholdId == model.Household.Id).ToList();
            model.Invitations = db.Invitations.Where(e => e.ToEmail == User.Identity.Name).ToList();
            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}