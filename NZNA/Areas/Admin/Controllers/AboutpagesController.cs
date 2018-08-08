
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
    public class AboutpagesController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Aboutpages
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingTagline = String.IsNullOrEmpty(sortingOrder) ? "Tagline" : "";
				            												ViewBag.SortingDescription = String.IsNullOrEmpty(sortingOrder) ? "Description" : "";
													            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            			
			
			var items = from item in db.Aboutpages select item;
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
            										
															item.Tagline.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Description.ToUpper().Contains(searchData.ToUpper()) ||
																				
																								item.ImageUrl.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "Tagline":
											items = items.OrderByDescending(item => item.Tagline);
                    					break;
            			case "Description":
											items = items.OrderByDescending(item => item.Description);
                    					break;
            			case "ImageUrl":
											items = items.OrderByDescending(item => item.ImageUrl);
                    					break;
            			default:
                    items = items.OrderBy(item => item.Tagline);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Aboutpages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Aboutpage aboutpage = db.Aboutpages.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var aboutpagea = from item in db.Aboutpages where (item.AboutpageId == id) select item;
            aboutpagea = aboutpagea.Include(a => a.CreatedBy);
            aboutpagea = aboutpagea.Where(a => a.CreatedBy == userid);
            if (aboutpagea.Count() == 0)
            {
                aboutpage = null;
            }





            if (aboutpage == null)
            {
                return HttpNotFound();
            }
            return View(aboutpage);
        }

        // GET: Admin/Aboutpages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Aboutpages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
        			public ActionResult Create([Bind(Include = "AboutpageId,Tagline,Description,ImageUrl,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Aboutpage aboutpage, HttpPostedFileBase ImageUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(aboutpage).State = EntityState.Added;
					aboutpage.CreatedBy = User.Identity.GetUserId();
                    aboutpage.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/aboutpage";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/aboutpage/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					aboutpage.ImageUrl = imageUrl;
	                    
						db.Aboutpages.Add(aboutpage);

                }
                else
                {
					db.Aboutpages.Add(aboutpage);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aboutpage);
        }

        // GET: Admin/Aboutpages/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aboutpage aboutpage = db.Aboutpages.Find(id);
			var userid = User.Identity.GetUserId();
            var aboutpagea = from item in db.Aboutpages where (item.AboutpageId == id) select item;
            aboutpagea = aboutpagea.Include(a => a.CreatedBy);
            aboutpagea = aboutpagea.Where(a => a.CreatedBy == userid);
            if (aboutpagea.Count() == 0)
            {
                aboutpage = null;
            }
            if (aboutpage == null)
            {
                return HttpNotFound();
            }
            return View(aboutpage);
        }

        // POST: Admin/Aboutpages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
					public ActionResult Edit([Bind(Include = "AboutpageId,Tagline,Description,ImageUrl,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Aboutpage aboutpage,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(aboutpage).State = EntityState.Modified;
				
				aboutpage.ModifiedBy = User.Identity.GetUserId();
                    aboutpage.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/aboutpage";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/aboutpage/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					aboutpage.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aboutpage);
        }

        // GET: Admin/Aboutpages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Aboutpage aboutpage = db.Aboutpages.Find(id);
			var userid = User.Identity.GetUserId();
            var aboutpagea = from item in db.Aboutpages where (item.AboutpageId == id) select item;
            aboutpagea = aboutpagea.Include(a => a.CreatedBy);
            aboutpagea = aboutpagea.Where(a => a.CreatedBy == userid);
            if (aboutpagea.Count() == 0)
            {
                aboutpage = null;
            }

            if (aboutpage == null)
            {
                return HttpNotFound();
            }
            return View(aboutpage);
        }

        // POST: Admin/Aboutpages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Aboutpage aboutpage = db.Aboutpages.Find(id);
            db.Aboutpages.Remove(aboutpage);
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
