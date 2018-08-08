
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
    public class BannersController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Banners
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingTitle = String.IsNullOrEmpty(sortingOrder) ? "Title" : "";
				            												ViewBag.SortingTagline = String.IsNullOrEmpty(sortingOrder) ? "Tagline" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            			
			
			var items = from item in db.Banners select item;
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
																				
													
															item.Tagline.ToUpper().Contains(searchData.ToUpper()) ||
																				
																								item.ImageUrl.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "Title":
											items = items.OrderByDescending(item => item.Title);
                    					break;
            			case "Tagline":
											items = items.OrderByDescending(item => item.Tagline);
                    					break;
            			case "ImageUrl":
											items = items.OrderByDescending(item => item.ImageUrl);
                    					break;
            			default:
                    items = items.OrderBy(item => item.Title);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Banners/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Banner banner = db.Banners.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var bannera = from item in db.Banners where (item.BannerId == id) select item;
            bannera = bannera.Include(a => a.CreatedBy);
            bannera = bannera.Where(a => a.CreatedBy == userid);
            if (bannera.Count() == 0)
            {
                banner = null;
            }





            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // GET: Admin/Banners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banners/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
        			public ActionResult Create([Bind(Include = "BannerId,Title,Tagline,ImageUrl,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Banner banner, HttpPostedFileBase ImageUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(banner).State = EntityState.Added;
					banner.CreatedBy = User.Identity.GetUserId();
                    banner.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/banner";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/banner/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					banner.ImageUrl = imageUrl;
	                    
						db.Banners.Add(banner);

                }
                else
                {
					db.Banners.Add(banner);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(banner);
        }

        // GET: Admin/Banners/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
			var userid = User.Identity.GetUserId();
            var bannera = from item in db.Banners where (item.BannerId == id) select item;
            bannera = bannera.Include(a => a.CreatedBy);
            bannera = bannera.Where(a => a.CreatedBy == userid);
            if (bannera.Count() == 0)
            {
                banner = null;
            }
            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banners/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
					public ActionResult Edit([Bind(Include = "BannerId,Title,Tagline,ImageUrl,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Banner banner,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(banner).State = EntityState.Modified;
				
				banner.ModifiedBy = User.Identity.GetUserId();
                    banner.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/banner";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/banner/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					banner.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(banner);
        }

        // GET: Admin/Banners/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Banner banner = db.Banners.Find(id);
			var userid = User.Identity.GetUserId();
            var bannera = from item in db.Banners where (item.BannerId == id) select item;
            bannera = bannera.Include(a => a.CreatedBy);
            bannera = bannera.Where(a => a.CreatedBy == userid);
            if (bannera.Count() == 0)
            {
                banner = null;
            }

            if (banner == null)
            {
                return HttpNotFound();
            }
            return View(banner);
        }

        // POST: Admin/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Banner banner = db.Banners.Find(id);
            db.Banners.Remove(banner);
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
