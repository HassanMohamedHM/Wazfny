using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using Wazfny.Models;
namespace Wazfny.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View(db.Categories.ToList());
        }

        public ActionResult Details(int? jobId)
        {
            if (jobId==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var job = db.Jobs.Find(jobId);
            if (job==null)
            {
                return HttpNotFound();
            }
            Session["JobId"] = jobId;
            return View(job);
        }
        [Authorize]
        [HttpGet]
        public ActionResult Apply()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Apply(string Message)
        {
            if (ModelState.IsValid)
            {
                var UserId = User.Identity.GetUserId();
                var JobId = (int)Session["JobId"];
                var check = db.ApplyForJobs.Where(a=>a.JobId==JobId&&a.UserId==UserId);
                if (check.Count() < 1)
                {
                    var applyforjob = new ApplyForJob()
                    {
                        Message = Message,
                        ApplyDate = DateTime.Now,
                        UserId = UserId,
                        JobId = JobId
                    };
                    db.ApplyForJobs.Add(applyforjob);
                    db.SaveChanges();
                    //ViewBag.Result = "تمت الإضافة بنجاح";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Result", "لقد تم التقدم إلى هذه الوظيفة من قبل.");
                    ViewBag.Result = "لقد تم التقدم إلى هذه الوظيفة من قبل.";

                }
            }
            return View(Message);
        }
        [Authorize]
        public ActionResult GetJobsByUser()
        {
            var UserId = User.Identity.GetUserId();
            return View(db.ApplyForJobs.Where(u => u.UserId == UserId).ToList());
        }
        [Authorize]
        public ActionResult DetailsOfJob(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var job = db.ApplyForJobs.Find(Id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        // POST: Role/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ApplyForJob job)
        {
            if (ModelState.IsValid)
            {
                job.ApplyDate = DateTime.Now;
                db.Entry(job).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("GetJobsByUser");
            }
            return View(job);
        }
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var job = db.ApplyForJobs.Find(id);
            if (job == null)
            {
                return HttpNotFound();
            }
            return View(job);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            var job = db.ApplyForJobs.Find(id);
            db.ApplyForJobs.Remove(job);
            db.SaveChanges();
            return RedirectToAction("GetJobsByUser");
        }



        [Authorize]
        public ActionResult GetJobsByPublisher()
        {
            var UserId = User.Identity.GetUserId();
            return View(db.Jobs.Where(u=>u.UserId==UserId).ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(ContactModel contact)
        {
            var mail = new MailMessage();
            var LoginInfo = new NetworkCredential("HassanMohamed_HM@Hotmail.com", "dodohm1234");
            mail.From = new MailAddress(contact.Email);
            mail.To.Add("HassanMohamed_HM@Hotmail.com");
            mail.Subject = contact.Subject;
            mail.IsBodyHtml = true;
            var body = "إسم المرسل :" + contact.Name + "<br/>" +
                "بريد المرسل :" + contact.Email + "<br/>" +
                "عنوان الرسالة :" + contact.Subject + "<br/>" +
                "نص الرسالة : <b>" + contact.Message + "</b>";
            mail.Body = body;//contact.Message;
            var smtpClient = new SmtpClient("smtp.live.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.Credentials = LoginInfo;
            smtpClient.Send(mail);

            return RedirectToAction("Index");
        }
        public ActionResult Search()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Search(string search)
        {
            var results = db.Jobs.Where(a=>a.JobTitle.Contains(search)
            || a.JobContent.Contains(search)
            || a.Category.CategoryName.Contains(search)
            || a.Category.CategoryDescription.Contains(search)).ToList();
            return View(results);
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