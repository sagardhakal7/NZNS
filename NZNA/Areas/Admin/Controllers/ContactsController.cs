
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
    public class ContactsController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Contacts
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingName = String.IsNullOrEmpty(sortingOrder) ? "Name" : "";
				            												ViewBag.SortingPhone = String.IsNullOrEmpty(sortingOrder) ? "Phone" : "";
				            												ViewBag.SortingEmail = String.IsNullOrEmpty(sortingOrder) ? "Email" : "";
				            												ViewBag.SortingSubject = String.IsNullOrEmpty(sortingOrder) ? "Subject" : "";
				            												ViewBag.SortingMessage = String.IsNullOrEmpty(sortingOrder) ? "Message" : "";
				            			
			
			var items = from item in db.Contacts select item;
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
            										
															item.Name.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Phone.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Email.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Subject.ToUpper().Contains(searchData.ToUpper()) ||
																				
																								item.Message.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "Name":
											items = items.OrderByDescending(item => item.Name);
                    					break;
            			case "Phone":
											items = items.OrderByDescending(item => item.Phone);
                    					break;
            			case "Email":
											items = items.OrderByDescending(item => item.Email);
                    					break;
            			case "Subject":
											items = items.OrderByDescending(item => item.Subject);
                    					break;
            			case "Message":
											items = items.OrderByDescending(item => item.Message);
                    					break;
            			default:
                    items = items.OrderBy(item => item.Name);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Contacts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Contact contact = db.Contacts.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var contacta = from item in db.Contacts where (item.ContactId == id) select item;
            contacta = contacta.Include(a => a.CreatedBy);
            contacta = contacta.Where(a => a.CreatedBy == userid);
            if (contacta.Count() == 0)
            {
                contact = null;
            }





            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // GET: Admin/Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
        			public ActionResult Create([Bind(Include = "ContactId,Name,Phone,Email,Subject,Message,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Contact contact)
		
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

        // GET: Admin/Contacts/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
			var userid = User.Identity.GetUserId();
            var contacta = from item in db.Contacts where (item.ContactId == id) select item;
            contacta = contacta.Include(a => a.CreatedBy);
            contacta = contacta.Where(a => a.CreatedBy == userid);
            if (contacta.Count() == 0)
            {
                contact = null;
            }
            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Admin/Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
					public ActionResult Edit([Bind(Include = "ContactId,Name,Phone,Email,Subject,Message,CreatedDate,CreatedBy,DelFlg")] Contact contact)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(contact).State = EntityState.Modified;
				
				contact.ModifiedBy = User.Identity.GetUserId();
                    contact.ModifiedDate = DateTime.Now;
					                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(contact);
        }

        // GET: Admin/Contacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Contact contact = db.Contacts.Find(id);
			var userid = User.Identity.GetUserId();
            var contacta = from item in db.Contacts where (item.ContactId == id) select item;
            contacta = contacta.Include(a => a.CreatedBy);
            contacta = contacta.Where(a => a.CreatedBy == userid);
            if (contacta.Count() == 0)
            {
                contact = null;
            }

            if (contact == null)
            {
                return HttpNotFound();
            }
            return View(contact);
        }

        // POST: Admin/Contacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Contact contact = db.Contacts.Find(id);
            db.Contacts.Remove(contact);
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
