
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
    public class RelatedLinksController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/RelatedLinks
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingTitle = String.IsNullOrEmpty(sortingOrder) ? "Title" : "";
				            												ViewBag.SortingTitleLink = String.IsNullOrEmpty(sortingOrder) ? "TitleLink" : "";
				            																					ViewBag.SortingLinkUrl = String.IsNullOrEmpty(sortingOrder) ? "LinkUrl" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            												ViewBag.SortingDescription = String.IsNullOrEmpty(sortingOrder) ? "Description" : "";
													            												ViewBag.SortingPhone = String.IsNullOrEmpty(sortingOrder) ? "Phone" : "";
				            												ViewBag.SortingAddress = String.IsNullOrEmpty(sortingOrder) ? "Address" : "";
				            			
			
			var items = from item in db.RelatedLinks select item;
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
																				
													
															item.TitleLink.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.LinkUrl.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
																				
													
															item.Description.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Phone.ToUpper().Contains(searchData.ToUpper()) ||
																				
																								item.Address.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "Title":
											items = items.OrderByDescending(item => item.Title);
                    					break;
            			case "TitleLink":
											items = items.OrderByDescending(item => item.TitleLink);
                    					break;
            			case "LinkUrl":
											items = items.OrderByDescending(item => item.LinkUrl);
                    					break;
            			case "ImageUrl":
											items = items.OrderByDescending(item => item.ImageUrl);
                    					break;
            			case "Description":
											items = items.OrderByDescending(item => item.Description);
                    					break;
            			case "Phone":
											items = items.OrderByDescending(item => item.Phone);
                    					break;
            			case "Address":
											items = items.OrderByDescending(item => item.Address);
                    					break;
            			default:
                    items = items.OrderBy(item => item.Title);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/RelatedLinks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           RelatedLink relatedLink = db.RelatedLinks.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var relatedLinka = from item in db.RelatedLinks where (item.RelatedLinkId == id) select item;
            relatedLinka = relatedLinka.Include(a => a.CreatedBy);
            relatedLinka = relatedLinka.Where(a => a.CreatedBy == userid);
            if (relatedLinka.Count() == 0)
            {
                relatedLink = null;
            }





            if (relatedLink == null)
            {
                return HttpNotFound();
            }
            return View(relatedLink);
        }

        // GET: Admin/RelatedLinks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/RelatedLinks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
        			public ActionResult Create([Bind(Include = "RelatedLinkId,Title,TitleLink,LinkUrl,ImageUrl,Description,Phone,Address,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] RelatedLink relatedLink, HttpPostedFileBase ImageUrl, HttpPostedFileBase LinkUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(relatedLink).State = EntityState.Added;
					relatedLink.CreatedBy = User.Identity.GetUserId();
                    relatedLink.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/relatedLink";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/relatedLink/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					relatedLink.ImageUrl = imageUrl;
	                    
						db.RelatedLinks.Add(relatedLink);

                }
                else if (LinkUrl != null && LinkUrl.ContentLength > 0)
                {
                    string pathToCreate = "~/UploadedDocuments/relatedLink";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(LinkUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(LinkUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string linkurl = "/UploadedDocuments/relatedLink/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;
                    LinkUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    // extract only the filename
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id;
                    // store the file inside ~/App_Data/uploads folder
                    relatedLink.LinkUrl = linkurl;

                    db.RelatedLinks.Add(relatedLink);

                }
                else
                {
					db.RelatedLinks.Add(relatedLink);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(relatedLink);
        }

        // GET: Admin/RelatedLinks/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelatedLink relatedLink = db.RelatedLinks.Find(id);
			var userid = User.Identity.GetUserId();
            var relatedLinka = from item in db.RelatedLinks where (item.RelatedLinkId == id) select item;
            relatedLinka = relatedLinka.Include(a => a.CreatedBy);
            relatedLinka = relatedLinka.Where(a => a.CreatedBy == userid);
            if (relatedLinka.Count() == 0)
            {
                relatedLink = null;
            }
            if (relatedLink == null)
            {
                return HttpNotFound();
            }
            return View(relatedLink);
        }

        // POST: Admin/RelatedLinks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
					public ActionResult Edit([Bind(Include = "RelatedLinkId,Title,TitleLink,LinkUrl,ImageUrl,Description,Phone,Address,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] RelatedLink relatedLink,HttpPostedFileBase ImageUrl, HttpPostedFileBase LinkUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(relatedLink).State = EntityState.Modified;
				
				relatedLink.ModifiedBy = User.Identity.GetUserId();
                    relatedLink.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/relatedLink";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/relatedLink/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					relatedLink.ImageUrl = imageUrl;
                }
                else if (LinkUrl != null && LinkUrl.ContentLength > 0)
                {
                    string pathToCreate = "~/UploadedDocuments/relatedLink";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(LinkUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(LinkUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string linkurl = "/UploadedDocuments/relatedLink/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;
                    LinkUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    // extract only the filename
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id;
                    // store the file inside ~/App_Data/uploads folder
                    relatedLink.LinkUrl = linkurl;

                    db.RelatedLinks.Add(relatedLink);

                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(relatedLink);
        }

        // GET: Admin/RelatedLinks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RelatedLink relatedLink = db.RelatedLinks.Find(id);
			var userid = User.Identity.GetUserId();
            var relatedLinka = from item in db.RelatedLinks where (item.RelatedLinkId == id) select item;
            relatedLinka = relatedLinka.Include(a => a.CreatedBy);
            relatedLinka = relatedLinka.Where(a => a.CreatedBy == userid);
            if (relatedLinka.Count() == 0)
            {
                relatedLink = null;
            }

            if (relatedLink == null)
            {
                return HttpNotFound();
            }
            return View(relatedLink);
        }

        // POST: Admin/RelatedLinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RelatedLink relatedLink = db.RelatedLinks.Find(id);
            db.RelatedLinks.Remove(relatedLink);
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
