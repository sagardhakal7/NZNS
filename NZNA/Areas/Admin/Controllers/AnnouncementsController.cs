
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Microsoft.AspNet.Identity;
using NZNA.Areas.Admin.Models;
using NZNA.Models;

namespace NZNA.Areas.Admin.Controllers

{
	[Authorize]
    public class AnnouncementsController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Announcements
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingDescription = String.IsNullOrEmpty(sortingOrder) ? "Description" : "";
													            			
			
			var items = from item in db.Announcements select item;
			if ((searchData != null && searchData.ToString() != "")|| (filterValue !=null && filterValue.ToString() != ""))
			{
			if (filterValue != null)
                {
                    searchData = filterValue;}
                else
                {
                    pageNo = 1;
                }
				 items =
                items.Where(
                    item =>
            																					item.Description.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "Description":
											items = items.OrderByDescending(item => item.Description);
                    					break;
            			default:
                    items = items.OrderBy(item => item.Description);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Announcements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Announcement announcement = db.Announcements.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var announcementa = from item in db.Announcements where (item.AnnouncementId == id) select item;
            announcementa = announcementa.Include(a => a.CreatedBy);
            announcementa = announcementa.Where(a => a.CreatedBy == userid);
            if (announcementa.Count() == 0)
            {
                announcement = null;
            }





            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // GET: Admin/Announcements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
        			public ActionResult Create([Bind(Include = "AnnouncementId,Description,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Announcement announcement)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(announcement).State = EntityState.Added;
					announcement.CreatedBy = User.Identity.GetUserId();
                    announcement.CreatedDate = DateTime.Now;
        			    
				db.Announcements.Add(announcement);
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(announcement);
        }

        // GET: Admin/Announcements/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
			var userid = User.Identity.GetUserId();
            var announcementa = from item in db.Announcements where (item.AnnouncementId == id) select item;
            announcementa = announcementa.Include(a => a.CreatedBy);
            announcementa = announcementa.Where(a => a.CreatedBy == userid);
            if (announcementa.Count() == 0)
            {
                announcement = null;
            }
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Admin/Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
					public ActionResult Edit([Bind(Include = "AnnouncementId,Description,CreatedDate,CreatedBy,DelFlg")] Announcement announcement)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(announcement).State = EntityState.Modified;
				
				announcement.ModifiedBy = User.Identity.GetUserId();
                    announcement.ModifiedDate = DateTime.Now;
					                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(announcement);
        }

        // GET: Admin/Announcements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
			var userid = User.Identity.GetUserId();
            var announcementa = from item in db.Announcements where (item.AnnouncementId == id) select item;
            announcementa = announcementa.Include(a => a.CreatedBy);
            announcementa = announcementa.Where(a => a.CreatedBy == userid);
            if (announcementa.Count() == 0)
            {
                announcement = null;
            }

            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Admin/Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = db.Announcements.Find(id);
            db.Announcements.Remove(announcement);
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
