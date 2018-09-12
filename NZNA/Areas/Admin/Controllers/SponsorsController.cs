
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
    public class SponsorsController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Sponsors
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingName = String.IsNullOrEmpty(sortingOrder) ? "Name" : "";
				            												ViewBag.SortingPhone = String.IsNullOrEmpty(sortingOrder) ? "Phone" : "";
				            												ViewBag.SortingEmail = String.IsNullOrEmpty(sortingOrder) ? "Email" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            												ViewBag.SortingTagline = String.IsNullOrEmpty(sortingOrder) ? "Tagline" : "";
				            			
			
			var items = from item in db.Sponsors select item;
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
																				
													
																				
																								item.Tagline.ToUpper().Contains(searchData.ToUpper()));
																					
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
            			case "ImageUrl":
											items = items.OrderByDescending(item => item.ImageUrl);
                    					break;
            			case "Tagline":
											items = items.OrderByDescending(item => item.Tagline);
                    					break;
            			default:
                    items = items.OrderBy(item => item.Name);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Sponsors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Sponsor sponsor = db.Sponsors.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var sponsora = from item in db.Sponsors where (item.SponsorId == id) select item;
            sponsora = sponsora.Include(a => a.CreatedBy);
            sponsora = sponsora.Where(a => a.CreatedBy == userid);
            if (sponsora.Count() == 0)
            {
                sponsor = null;
            }





            if (sponsor == null)
            {
                return HttpNotFound();
            }
            return View(sponsor);
        }

        // GET: Admin/Sponsors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sponsors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
        			public ActionResult Create([Bind(Include = "SponsorId,Name,Phone,Email,ImageUrl,Tagline,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Sponsor sponsor, HttpPostedFileBase ImageUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(sponsor).State = EntityState.Added;
					sponsor.CreatedBy = User.Identity.GetUserId();
                    sponsor.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/sponsor";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/sponsor/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					sponsor.ImageUrl = imageUrl;
	                    
						db.Sponsors.Add(sponsor);

                }
                else
                {
					db.Sponsors.Add(sponsor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sponsor);
        }

        // GET: Admin/Sponsors/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sponsor sponsor = db.Sponsors.Find(id);
			var userid = User.Identity.GetUserId();
            var sponsora = from item in db.Sponsors where (item.SponsorId == id) select item;
            sponsora = sponsora.Include(a => a.CreatedBy);
            sponsora = sponsora.Where(a => a.CreatedBy == userid);
            if (sponsora.Count() == 0)
            {
                sponsor = null;
            }
            if (sponsor == null)
            {
                return HttpNotFound();
            }
            return View(sponsor);
        }

        // POST: Admin/Sponsors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
					public ActionResult Edit([Bind(Include = "SponsorId,Name,Phone,Email,ImageUrl,Tagline,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Sponsor sponsor,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(sponsor).State = EntityState.Modified;
				
				sponsor.ModifiedBy = User.Identity.GetUserId();
                    sponsor.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/sponsor";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/sponsor/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					sponsor.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sponsor);
        }

        // GET: Admin/Sponsors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sponsor sponsor = db.Sponsors.Find(id);
			var userid = User.Identity.GetUserId();
            var sponsora = from item in db.Sponsors where (item.SponsorId == id) select item;
            sponsora = sponsora.Include(a => a.CreatedBy);
            sponsora = sponsora.Where(a => a.CreatedBy == userid);
            if (sponsora.Count() == 0)
            {
                sponsor = null;
            }

            if (sponsor == null)
            {
                return HttpNotFound();
            }
            return View(sponsor);
        }

        // POST: Admin/Sponsors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sponsor sponsor = db.Sponsors.Find(id);
            db.Sponsors.Remove(sponsor);
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
