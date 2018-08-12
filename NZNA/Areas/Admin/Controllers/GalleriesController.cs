
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
using System.Web.Routing;

namespace NZNA.Areas.Admin.Controllers

{
	[Authorize]
    public class GalleriesController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();
        private RouteConfig routeConfig = new RouteConfig();
        private RouteCollection routeCol = new RouteCollection();

        // GET: Admin/Galleries
   
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo, RouteConfig routeConfiguration, RouteCollection routeCollection)
        {
            this.routeConfig = routeConfiguration;
            this.routeCol = routeCollection;

            var routeInfo = Request.RawUrl;

            ViewBag.CurrentSortOrder = sortingOrder;         
			ViewBag.SortingGalleryId = String.IsNullOrEmpty(sortingOrder) ? "GalleryId" : "";
			ViewBag.SortingTitle = String.IsNullOrEmpty(sortingOrder) ? "Title" : "";
			ViewBag.SortingTagline = String.IsNullOrEmpty(sortingOrder) ? "Tagline" : "";
				            					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
			ViewBag.SortingAlbumId = String.IsNullOrEmpty(sortingOrder) ? "AlbumId" : "";
				            				
			var items = from item in db.Galleries select item;
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
            items.Where( item =>item.Title.ToUpper().Contains(searchData.ToUpper()) || item.Tagline.ToUpper().Contains(searchData.ToUpper()) || item.AlbumId.ToUpper().Contains(searchData.ToUpper()));
	}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "GalleryId":
											items = items.OrderByDescending(item => item.GalleryId);
                    					break;
            			case "Title":
											items = items.OrderByDescending(item => item.Title);
                    					break;
            			case "Tagline":
											items = items.OrderByDescending(item => item.Tagline);
                    					break;
            			case "ImageUrl":
											items = items.OrderByDescending(item => item.ImageUrl);
                    					break;
            			case "AlbumId":
											items = items.OrderByDescending(item => item.AlbumId);
                    					break;
            			default:
                    items = items.OrderBy(item => item.GalleryId);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Galleries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Gallery gallery = db.Galleries.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var gallerya = from item in db.Galleries where (item.GalleryId == id) select item;
            gallerya = gallerya.Include(a => a.CreatedBy);
            gallerya = gallerya.Where(a => a.CreatedBy == userid);
            if (gallerya.Count() == 0)
            {
                gallery = null;
            }





            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // GET: Admin/Galleries/Create
        public ActionResult Create()
        {
            // var customer = db.Customer.Include(c => c.MembershipType); //using include for eager loading
            var galleries = db.Galleries.Include(o => o.Album);
            return View(galleries.ToList());
        }

        // POST: Admin/Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
        			public ActionResult Create([Bind(Include = "GalleryId,Title,Tagline,ImageUrl,AlbumId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Gallery gallery, HttpPostedFileBase ImageUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(gallery).State = EntityState.Added;
					gallery.CreatedBy = User.Identity.GetUserId();
                    gallery.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/gallery";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/gallery/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					gallery.ImageUrl = imageUrl;
	                    
						db.Galleries.Add(gallery);

                }
                else
                {
					db.Galleries.Add(gallery);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(gallery);
        }

        // GET: Admin/Galleries/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
			var userid = User.Identity.GetUserId();
            var gallerya = from item in db.Galleries where (item.GalleryId == id) select item;
            gallerya = gallerya.Include(a => a.CreatedBy);
            gallerya = gallerya.Where(a => a.CreatedBy == userid);
            if (gallerya.Count() == 0)
            {
                gallery = null;
            }
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Admin/Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
					public ActionResult Edit([Bind(Include = "GalleryId,Title,Tagline,ImageUrl,AlbumId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Gallery gallery,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallery).State = EntityState.Modified;
				
				gallery.ModifiedBy = User.Identity.GetUserId();
                    gallery.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/gallery";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/gallery/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					gallery.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(gallery);
        }

        // GET: Admin/Galleries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
			var userid = User.Identity.GetUserId();
            var gallerya = from item in db.Galleries where (item.GalleryId == id) select item;
            gallerya = gallerya.Include(a => a.CreatedBy);
            gallerya = gallerya.Where(a => a.CreatedBy == userid);
            if (gallerya.Count() == 0)
            {
                gallery = null;
            }

            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Admin/Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Gallery gallery = db.Galleries.Find(id);
            db.Galleries.Remove(gallery);
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
