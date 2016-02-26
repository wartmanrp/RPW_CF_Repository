using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CFBudgeter.Models;
using System.Threading.Tasks;

namespace CFBudgeter.Controllers
{
    [RequireHttps]
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //// GET: Households
        //[Authorize]
        //public ActionResult Index()
        //{
        //    var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        //    return View(currentUser.Household);
        //}

        //GET: HouseholdId
        //[Authorize]
        //public string GetHouseholdId (string houseId)
        //{
        //    var houseId = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name).HouseholdId.ToString;
            
        //}

        // GET: Households/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                var userHouseId = db.Users.FirstOrDefault(u=> u.UserName == User.Identity.Name).HouseholdId;
                return RedirectToAction("Details", "Households", new { id = userHouseId});
            }
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            Household household = currentUser.Household;

            //Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // GET: Households/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Households.Add(household);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

            Household household = currentUser.Household;
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                var currentUser = db.Users.FirstOrDefault(u=> u.UserName == User.Identity.Name);
                db.Entry(household).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Households", new { id = currentUser.Household.Id});
            }
            return View(household);
        }

        //GET: Households/Join
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult> JoinHousehold(string JoinCode)
        {
            //set up the confirmation page so the user can confirm he wants to change households
            //might also need a view model for this page
            var invitation = await db.Invitations.FirstOrDefaultAsync(i => i.JoinCode.ToString() == JoinCode);
            if (JoinCode == invitation.JoinCode.ToString())
            {
                var model = new RegisterViewModel
                {
                    HouseholdId = invitation.Household.Id,
                    HouseholdName = invitation.Household.Name,
                    Email = invitation.ToEmail
                };
                var user = db.Users.FirstOrDefault(u => u.Email == invitation.ToEmail);
                if (user != null){
                    //user exists in the sytsem populate the rest of the user info
                    model.FirstName = user.FirstName;
                    model.LastName = user.LastName;
                }
                return View(model);
            }
            else
            {
                return RedirectToAction("Login", "Account", new { message = "Unauthorized" });
            }
        }

       
        //// GET: Households/Delete/5
        //[Authorize]
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Household household = db.Households.Find(id);
        //    if (household == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(household);
        //}

        //// POST: Households/Delete/5
        //[Authorize]
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Household household = db.Households.Find(id);
        //    db.Households.Remove(household);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

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
