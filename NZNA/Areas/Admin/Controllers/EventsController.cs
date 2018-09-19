
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
    public class EventsController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Events
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingTitle = String.IsNullOrEmpty(sortingOrder) ? "Title" : "";
				            																					ViewBag.SortingLinkUrl = String.IsNullOrEmpty(sortingOrder) ? "LinkUrl" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            												ViewBag.SortingEventDate = String.IsNullOrEmpty(sortingOrder) ? "EventDate" : "";
				            												ViewBag.SortingDescription = String.IsNullOrEmpty(sortingOrder) ? "Description" : "";
													            												ViewBag.SortingPhone = String.IsNullOrEmpty(sortingOrder) ? "Phone" : "";
				            												ViewBag.SortingTiming = String.IsNullOrEmpty(sortingOrder) ? "Timing" : "";
				            												ViewBag.SortingAddress = String.IsNullOrEmpty(sortingOrder) ? "Address" : "";
				            												ViewBag.SortingIsCompleted = String.IsNullOrEmpty(sortingOrder) ? "IsCompleted" : "";
				            			
			
			var items = from item in db.Events select item;
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
            										
															item.Title.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.LinkUrl.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
																				
													
																				
													
															item.Description.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Phone.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Timing.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Address.ToUpper().Contains(searchData.ToUpper()) ||
																				
																								item.IsCompleted.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "Title":
											items = items.OrderByDescending(item => item.Title);
                    					break;
            			case "LinkUrl":
											items = items.OrderByDescending(item => item.LinkUrl);
                    					break;
            			case "ImageUrl":
											items = items.OrderByDescending(item => item.ImageUrl);
                    					break;
            			case "EventDate":
											items = items.OrderBy(item => item.EventDate);	
										break;
            			case "Description":
											items = items.OrderByDescending(item => item.Description);
                    					break;
            			case "Phone":
											items = items.OrderByDescending(item => item.Phone);
                    					break;
            			case "Timing":
											items = items.OrderByDescending(item => item.Timing);
                    					break;
            			case "Address":
											items = items.OrderByDescending(item => item.Address);
                    					break;
            			case "IsCompleted":
											items = items.OrderByDescending(item => item.IsCompleted);
                    					break;
            			default:
                    items = items.OrderBy(item => item.Title);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Events/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Event @event = db.Events.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var @eventa = from item in db.Events where (item.EventId == id) select item;
            @eventa = @eventa.Include(a => a.CreatedBy);
            @eventa = @eventa.Where(a => a.CreatedBy == userid);
            if (@eventa.Count() == 0)
            {
                @event = null;
            }





            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // GET: Admin/Events/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
        			public ActionResult Create([Bind(Include = "EventId,Title,LinkUrl,ImageUrl,EventDate,Description,Phone,Timing,Address,IsCompleted,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Event @event, HttpPostedFileBase ImageUrl, HttpPostedFileBase LinkUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(@event).State = EntityState.Added;
					@event.CreatedBy = User.Identity.GetUserId();
                    @event.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/@event";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/@event/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
                    ImageResizer.ImageJob j = new ImageResizer.ImageJob(DestinationPath + extension, DestinationPath + "_list.jpg", new ImageResizer.ResizeSettings(
                 "width=500;height=350;format=jpg;mode=crop"));
                    j.Build();

                    @event.ImageUrl = imageUrl;
	                    
						db.Events.Add(@event);

                }
               else if (LinkUrl != null && LinkUrl.ContentLength > 0)
                {
                    string pathToCreate = "~/UploadedDocuments/Events";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(LinkUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(LinkUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string linkurl = "/UploadedDocuments/Events/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;
                    LinkUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    // extract only the filename
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id;
                    // store the file inside ~/App_Data/uploads folder
                    @event.LinkUrl = linkurl;

                    db.Events.Add(@event);

                }
                else
                {
					db.Events.Add(@event);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(@event);
        }

        // GET: Admin/Events/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
			var userid = User.Identity.GetUserId();
            var @eventa = from item in db.Events where (item.EventId == id) select item;
            @eventa = @eventa.Include(a => a.CreatedBy);
            @eventa = @eventa.Where(a => a.CreatedBy == userid);
            if (@eventa.Count() == 0)
            {
                @event = null;
            }
            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Admin/Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
					public ActionResult Edit([Bind(Include = "EventId,Title,LinkUrl,ImageUrl,EventDate,Description,Phone,Timing,Address,IsCompleted,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Event @event,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(@event).State = EntityState.Modified;
				
				@event.ModifiedBy = User.Identity.GetUserId();
                    @event.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/@event";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/@event/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					@event.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(@event);
        }

        // GET: Admin/Events/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Event @event = db.Events.Find(id);
			var userid = User.Identity.GetUserId();
            var @eventa = from item in db.Events where (item.EventId == id) select item;
            @eventa = @eventa.Include(a => a.CreatedBy);
            @eventa = @eventa.Where(a => a.CreatedBy == userid);
            if (@eventa.Count() == 0)
            {
                @event = null;
            }

            if (@event == null)
            {
                return HttpNotFound();
            }
            return View(@event);
        }

        // POST: Admin/Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Event @event = db.Events.Find(id);
            db.Events.Remove(@event);
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
