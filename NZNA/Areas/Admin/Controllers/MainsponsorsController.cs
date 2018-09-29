﻿
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
    public class MainsponsorsController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Mainsponsors
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
																								ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            												ViewBag.SortingTitle = String.IsNullOrEmpty(sortingOrder) ? "Title" : "";
				            												ViewBag.SortingPhone = String.IsNullOrEmpty(sortingOrder) ? "Phone" : "";
				            			
			
			var items = from item in db.Mainsponsors select item;
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
																				
																								item.Phone.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "ImageUrl":
											items = items.OrderByDescending(item => item.ImageUrl);
                    					break;
            			case "Title":
											items = items.OrderByDescending(item => item.Title);
                    					break;
            			case "Phone":
											items = items.OrderByDescending(item => item.Phone);
                    					break;
            			default:
                    items = items.OrderBy(item => item.ImageUrl);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Mainsponsors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Mainsponsor mainsponsor = db.Mainsponsors.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var mainsponsora = from item in db.Mainsponsors where (item.MainsponsorId == id) select item;
            mainsponsora = mainsponsora.Include(a => a.CreatedBy);
            mainsponsora = mainsponsora.Where(a => a.CreatedBy == userid);
            if (mainsponsora.Count() == 0)
            {
                mainsponsor = null;
            }





            if (mainsponsor == null)
            {
                return HttpNotFound();
            }
            return View(mainsponsor);
        }

        // GET: Admin/Mainsponsors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Mainsponsors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
        			public ActionResult Create([Bind(Include = "MainsponsorId,ImageUrl,Title,Phone,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Mainsponsor mainsponsor, HttpPostedFileBase ImageUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(mainsponsor).State = EntityState.Added;
					mainsponsor.CreatedBy = User.Identity.GetUserId();
                    mainsponsor.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/mainsponsor";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/mainsponsor/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=308;height=270;format=jpg;mode=max"));
					i.Build();
					
					mainsponsor.ImageUrl = imageUrl;
	                    
						db.Mainsponsors.Add(mainsponsor);

                }
                else
                {
					db.Mainsponsors.Add(mainsponsor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mainsponsor);
        }

        // GET: Admin/Mainsponsors/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mainsponsor mainsponsor = db.Mainsponsors.Find(id);
			var userid = User.Identity.GetUserId();
            var mainsponsora = from item in db.Mainsponsors where (item.MainsponsorId == id) select item;
            mainsponsora = mainsponsora.Include(a => a.CreatedBy);
            mainsponsora = mainsponsora.Where(a => a.CreatedBy == userid);
            if (mainsponsora.Count() == 0)
            {
                mainsponsor = null;
            }
            if (mainsponsor == null)
            {
                return HttpNotFound();
            }
            return View(mainsponsor);
        }

        // POST: Admin/Mainsponsors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
					public ActionResult Edit([Bind(Include = "MainsponsorId,ImageUrl,Title,Phone,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Mainsponsor mainsponsor,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(mainsponsor).State = EntityState.Modified;
				
				mainsponsor.ModifiedBy = User.Identity.GetUserId();
                    mainsponsor.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/mainsponsor";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/mainsponsor/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                    "width=308;height=270;format=jpg;mode=max"));
					i.Build();
					mainsponsor.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mainsponsor);
        }

        // GET: Admin/Mainsponsors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mainsponsor mainsponsor = db.Mainsponsors.Find(id);
			var userid = User.Identity.GetUserId();
            var mainsponsora = from item in db.Mainsponsors where (item.MainsponsorId == id) select item;
            mainsponsora = mainsponsora.Include(a => a.CreatedBy);
            mainsponsora = mainsponsora.Where(a => a.CreatedBy == userid);
            if (mainsponsora.Count() == 0)
            {
                mainsponsor = null;
            }

            if (mainsponsor == null)
            {
                return HttpNotFound();
            }
            return View(mainsponsor);
        }

        // POST: Admin/Mainsponsors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mainsponsor mainsponsor = db.Mainsponsors.Find(id);
            db.Mainsponsors.Remove(mainsponsor);
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
