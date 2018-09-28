
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
    public class SaugatsController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Saugats
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingTitle = String.IsNullOrEmpty(sortingOrder) ? "Title" : "";
				            												ViewBag.SortingDescription = String.IsNullOrEmpty(sortingOrder) ? "Description" : "";
													            																					ViewBag.SortingLinkUrl = String.IsNullOrEmpty(sortingOrder) ? "LinkUrl" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            			
			
			var items = from item in db.Saugats select item;
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
																				
													
															item.Description.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.LinkUrl.ToUpper().Contains(searchData.ToUpper()) ||
																				
																								item.ImageUrl.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "Title":
											items = items.OrderByDescending(item => item.Title);
                    					break;
            			case "Description":
											items = items.OrderByDescending(item => item.Description);
                    					break;
            			case "LinkUrl":
											items = items.OrderByDescending(item => item.LinkUrl);
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

        // GET: Admin/Saugats/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Saugat saugat = db.Saugats.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var saugata = from item in db.Saugats where (item.SaugatId == id) select item;
            saugata = saugata.Include(a => a.CreatedBy);
            saugata = saugata.Where(a => a.CreatedBy == userid);
            if (saugata.Count() == 0)
            {
                saugat = null;
            }





            if (saugat == null)
            {
                return HttpNotFound();
            }
            return View(saugat);
        }

        // GET: Admin/Saugats/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Saugats/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
        			public ActionResult Create([Bind(Include = "SaugatId,Title,Description,LinkUrl,ImageUrl,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Saugat saugat, HttpPostedFileBase ImageUrl, HttpPostedFileBase LinkUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(saugat).State = EntityState.Added;
					saugat.CreatedBy = User.Identity.GetUserId();
                    saugat.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/saugat";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/saugat/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					saugat.ImageUrl = imageUrl;
	                    
						db.Saugats.Add(saugat);

                }
               

               if (LinkUrl != null)
                {
                    string pathToCreate = "~/Documents/Saugat";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(LinkUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(LinkUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string LinkUrlToSave = "/Documents/Saugat/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;
                    LinkUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    saugat.LinkUrl = LinkUrlToSave;
                }
               
                db.Saugats.Add(saugat);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(saugat);
        }

        // GET: Admin/Saugats/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saugat saugat = db.Saugats.Find(id);
			var userid = User.Identity.GetUserId();
            var saugata = from item in db.Saugats where (item.SaugatId == id) select item;
            saugata = saugata.Include(a => a.CreatedBy);
            saugata = saugata.Where(a => a.CreatedBy == userid);
            if (saugata.Count() == 0)
            {
                saugat = null;
            }
            if (saugat == null)
            {
                return HttpNotFound();
            }
            return View(saugat);
        }

        // POST: Admin/Saugats/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
					public ActionResult Edit([Bind(Include = "SaugatId,Title,Description,LinkUrl,ImageUrl,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Saugat saugat,HttpPostedFileBase ImageUrl, HttpPostedFileBase LinkUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(saugat).State = EntityState.Modified;
				
				saugat.ModifiedBy = User.Identity.GetUserId();
                    saugat.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/saugat";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/saugat/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					saugat.ImageUrl = imageUrl;
                }
                if(LinkUrl != null)
                {
                    string pathToCreate = "~/Documents/Saugat";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(LinkUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(LinkUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string LinkUrlToSave = "/Documents/Saugat/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;
                    LinkUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    saugat.LinkUrl = LinkUrlToSave;
                }
                db.Saugats.Add(saugat);
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(saugat);
        }

        // GET: Admin/Saugats/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Saugat saugat = db.Saugats.Find(id);
			var userid = User.Identity.GetUserId();
            var saugata = from item in db.Saugats where (item.SaugatId == id) select item;
            saugata = saugata.Include(a => a.CreatedBy);
            saugata = saugata.Where(a => a.CreatedBy == userid);
            if (saugata.Count() == 0)
            {
                saugat = null;
            }

            if (saugat == null)
            {
                return HttpNotFound();
            }
            return View(saugat);
        }

        // POST: Admin/Saugats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Saugat saugat = db.Saugats.Find(id);
            db.Saugats.Remove(saugat);
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
