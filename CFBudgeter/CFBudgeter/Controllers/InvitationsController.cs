using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CFBudgeter.Models;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace CFBudgeter.Controllers
{
    [RequireHttps]
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Invitations
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Invitations.ToList());
        }

        // GET: Invitations/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ToEmail")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                var currentUser = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);

                invitation.UserId = currentUser.Id;
                invitation.HouseholdId = currentUser.HouseholdId;
                invitation.JoinCode = Guid.NewGuid();
                db.Invitations.Add(invitation);
                db.SaveChanges();

                var sendGridCredentials = db.SendGridCredentials.First();
                try
                {
                    MailMessage mailMsg = new MailMessage();

                    // To
                    mailMsg.To.Add(new MailAddress(invitation.ToEmail, "To"));

                    // From
                    mailMsg.From = new MailAddress(currentUser.Email, "From");

                    // Subject and multipart/alternative Body
                    var callBackUrl = Url.Action("JoinHousehold", "Households", new{JoinCode = invitation.JoinCode}, protocol: Request.Url.Scheme);
                    StringBuilder str = new StringBuilder();
                    str.Append("<p>I would like to invite you to join my household on the Budget Maseter household budgeting platform.</p><p>Please click the following link to join/register: <a href='");
                    str.Append(callBackUrl);
                    str.Append("'>Click here (you will be redirected)</a></p>");

                    mailMsg.Subject = currentUser.FirstName + " " + "has invited you to the" + " " + currentUser.Household.Name + " " + "household on the Budget Master website."; 

                    mailMsg.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(str.ToString(), null, MediaTypeNames.Text.Html));

                    // Init SmtpClient and send
                    SmtpClient smtpClient = new SmtpClient("smtp.sendgrid.net", Convert.ToInt32(587));
                    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(sendGridCredentials.UserName, sendGridCredentials.Password);
                    smtpClient.Credentials = credentials;

                    smtpClient.Send(mailMsg);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }


                return RedirectToAction("Index");
            }

            return View(invitation);
        }

        // GET: Invitations/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ToEmail")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invitation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
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
