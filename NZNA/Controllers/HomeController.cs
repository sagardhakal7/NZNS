using NZNA.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NZNA.Models;

namespace NZNA.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult AllMembers()
        {
            return View();
        }
        public ActionResult Events()
        {
            return View();
        }
        public ActionResult ExecutiveCommitte()
        {
            return View();
        }
        public ActionResult PastExecutive()
        {
            return View();
        }
        public ActionResult Resources()
        {
            return View();
        }
        public ActionResult Contact()
        {
            
            return View();
        }

        public ActionResult EventDetails(int id)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            var eventdetails = db.Events.Find(id);
            ViewBag.postid = Convert.ToString(id);
            ModelState.Clear();
            return View(eventdetails);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Contact ([Bind(Include = "ContactId,Name,Phone,Email,Subject,Message,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Contact contact)

        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Added;
                contact.CreatedBy = "32d11990-beb5-4bac-b952-4a08c9e78a8b";
                contact.CreatedDate = DateTime.Now;

                db.Contacts.Add(contact);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(contact);
        }
    }
}