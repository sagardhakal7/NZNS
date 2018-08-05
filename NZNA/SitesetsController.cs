
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
    public class SitesetsController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Sitesets
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingPhone = String.IsNullOrEmpty(sortingOrder) ? "Phone" : "";
				            												ViewBag.SortingAddress = String.IsNullOrEmpty(sortingOrder) ? "Address" : "";
				            												ViewBag.SortingEmail = String.IsNullOrEmpty(sortingOrder) ? "Email" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            												ViewBag.SortingFbId = String.IsNullOrEmpty(sortingOrder) ? "FbId" : "";
				            												ViewBag.SortingTwitterId = String.IsNullOrEmpty(sortingOrder) ? "TwitterId" : "";
				            												ViewBag.SortingGoogleId = String.IsNullOrEmpty(sortingOrder) ? "GoogleId" : "";
				            												ViewBag.SortingInstagramId = String.IsNullOrEmpty(sortingOrder) ? "InstagramId" : "";
				            												ViewBag.SortingLinkedinId = String.IsNullOrEmpty(sortingOrder) ? "LinkedinId" : "";
				            												ViewBag.SortingCopyright = String.IsNullOrEmpty(sortingOrder) ? "Copyright" : "";
				            			
			
			var items = from item in db.Sitesets select item;
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
            										
															item.Phone.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Address.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Email.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
																				
													
															item.FbId.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.TwitterId.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.GoogleId.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.InstagramId.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.LinkedinId.ToUpper().Contains(searchData.ToUpper()) ||
																				
																								item.Copyright.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "Phone":
											items = items.OrderByDescending(item => item.Phone);
                    					break;
            			case "Address":
											items = items.OrderByDescending(item => item.Address);
                    					break;
            			case "Email":
											items = items.OrderByDescending(item => item.Email);
                    					break;
            			case "ImageUrl":
											items = items.OrderByDescending(item => item.ImageUrl);
                    					break;
            			case "FbId":
											items = items.OrderByDescending(item => item.FbId);
                    					break;
            			case "TwitterId":
											items = items.OrderByDescending(item => item.TwitterId);
                    					break;
            			case "GoogleId":
											items = items.OrderByDescending(item => item.GoogleId);
                    					break;
            			case "InstagramId":
											items = items.OrderByDescending(item => item.InstagramId);
                    					break;
            			case "LinkedinId":
											items = items.OrderByDescending(item => item.LinkedinId);
                    					break;
            			case "Copyright":
											items = items.OrderByDescending(item => item.Copyright);
                    					break;
            			default:
                    items = items.OrderBy(item => item.Phone);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Sitesets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Siteset siteset = db.Sitesets.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var siteseta = from item in db.Sitesets where (item.SitesetId == id) select item;
            siteseta = siteseta.Include(a => a.CreatedBy);
            siteseta = siteseta.Where(a => a.CreatedBy == userid);
            if (siteseta.Count() == 0)
            {
                siteset = null;
            }





            if (siteset == null)
            {
                return HttpNotFound();
            }
            return View(siteset);
        }

        // GET: Admin/Sitesets/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sitesets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
        			public ActionResult Create([Bind(Include = "SitesetId,Phone,Address,Email,ImageUrl,FbId,TwitterId,GoogleId,InstagramId,LinkedinId,Copyright,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Siteset siteset, HttpPostedFileBase ImageUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(siteset).State = EntityState.Added;
					siteset.CreatedBy = User.Identity.GetUserId();
                    siteset.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/siteset";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/siteset/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					siteset.ImageUrl = imageUrl;
	                    
						db.Sitesets.Add(siteset);

                }
                else
                {
					db.Sitesets.Add(siteset);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(siteset);
        }

        // GET: Admin/Sitesets/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Siteset siteset = db.Sitesets.Find(id);
			var userid = User.Identity.GetUserId();
            var siteseta = from item in db.Sitesets where (item.SitesetId == id) select item;
            siteseta = siteseta.Include(a => a.CreatedBy);
            siteseta = siteseta.Where(a => a.CreatedBy == userid);
            if (siteseta.Count() == 0)
            {
                siteset = null;
            }
            if (siteset == null)
            {
                return HttpNotFound();
            }
            return View(siteset);
        }

        // POST: Admin/Sitesets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
					public ActionResult Edit([Bind(Include = "SitesetId,Phone,Address,Email,ImageUrl,FbId,TwitterId,GoogleId,InstagramId,LinkedinId,Copyright,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Siteset siteset,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(siteset).State = EntityState.Modified;
				
				siteset.ModifiedBy = User.Identity.GetUserId();
                    siteset.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/siteset";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/siteset/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					siteset.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(siteset);
        }

        // GET: Admin/Sitesets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Siteset siteset = db.Sitesets.Find(id);
			var userid = User.Identity.GetUserId();
            var siteseta = from item in db.Sitesets where (item.SitesetId == id) select item;
            siteseta = siteseta.Include(a => a.CreatedBy);
            siteseta = siteseta.Where(a => a.CreatedBy == userid);
            if (siteseta.Count() == 0)
            {
                siteset = null;
            }

            if (siteset == null)
            {
                return HttpNotFound();
            }
            return View(siteset);
        }

        // POST: Admin/Sitesets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Siteset siteset = db.Sitesets.Find(id);
            db.Sitesets.Remove(siteset);
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
