
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
    public class AlbumsController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Albums
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingTitle = String.IsNullOrEmpty(sortingOrder) ? "Title" : "";
				            												ViewBag.SortingTagline = String.IsNullOrEmpty(sortingOrder) ? "Tagline" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            			
			
			var items = from item in db.Albums select item;
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

        // GET: Admin/Albums/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Album album = db.Albums.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var albuma = from item in db.Albums where (item.AlbumId == id) select item;
            albuma = albuma.Include(a => a.CreatedBy);
            albuma = albuma.Where(a => a.CreatedBy == userid);
            if (albuma.Count() == 0)
            {
                album = null;
            }





            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // GET: Admin/Albums/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
        			public ActionResult Create([Bind(Include = "AlbumId,Title,Tagline,ImageUrl,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Album album, HttpPostedFileBase ImageUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(album).State = EntityState.Added;
					album.CreatedBy = User.Identity.GetUserId();
                    album.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/album";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/album/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					album.ImageUrl = imageUrl;
	                    
						db.Albums.Add(album);

                }
                else
                {
					db.Albums.Add(album);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(album);
        }

        // GET: Admin/Albums/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
			var userid = User.Identity.GetUserId();
            var albuma = from item in db.Albums where (item.AlbumId == id) select item;
            albuma = albuma.Include(a => a.CreatedBy);
            albuma = albuma.Where(a => a.CreatedBy == userid);
            if (albuma.Count() == 0)
            {
                album = null;
            }
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Admin/Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
					public ActionResult Edit([Bind(Include = "AlbumId,Title,Tagline,ImageUrl,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Album album,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
				
				album.ModifiedBy = User.Identity.GetUserId();
                    album.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/album";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/album/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					album.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(album);
        }

        // GET: Admin/Albums/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = db.Albums.Find(id);
			var userid = User.Identity.GetUserId();
            var albuma = from item in db.Albums where (item.AlbumId == id) select item;
            albuma = albuma.Include(a => a.CreatedBy);
            albuma = albuma.Where(a => a.CreatedBy == userid);
            if (albuma.Count() == 0)
            {
                album = null;
            }

            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Admin/Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Album album = db.Albums.Find(id);
            db.Albums.Remove(album);
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
