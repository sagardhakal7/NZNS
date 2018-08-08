
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
    public class NewsController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/News
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingNewsId = String.IsNullOrEmpty(sortingOrder) ? "NewsId" : "";
				            												ViewBag.SortingTitle = String.IsNullOrEmpty(sortingOrder) ? "Title" : "";
				            																					ViewBag.SortingLinkUrl = String.IsNullOrEmpty(sortingOrder) ? "LinkUrl" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            												ViewBag.SortingDescription = String.IsNullOrEmpty(sortingOrder) ? "Description" : "";
													            			
			
			var items = from item in db.News select item;
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
																				
													
																				
																								item.Description.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "NewsId":
											items = items.OrderByDescending(item => item.NewsId);
                    					break;
            			case "Title":
											items = items.OrderByDescending(item => item.Title);
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
            			default:
                    items = items.OrderBy(item => item.NewsId);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/News/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           News news = db.News.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var newsa = from item in db.News where (item.NewsId == id) select item;
            newsa = newsa.Include(a => a.CreatedBy);
            newsa = newsa.Where(a => a.CreatedBy == userid);
            if (newsa.Count() == 0)
            {
                news = null;
            }





            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // GET: Admin/News/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/News/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
        			public ActionResult Create([Bind(Include = "NewsId,Title,LinkUrl,ImageUrl,Description,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] News news, HttpPostedFileBase ImageUrl, HttpPostedFileBase LinkUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(news).State = EntityState.Added;
					news.CreatedBy = User.Identity.GetUserId();
                    news.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/news";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/news/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					news.ImageUrl = imageUrl;
	                    
						db.News.Add(news);

                }
               else if (LinkUrl != null && LinkUrl.ContentLength > 0)
                {
                    string pathToCreate = "~/UploadedDocuments/News";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(LinkUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(LinkUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string linkurl = "/UploadedDocuments/News/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;
                    LinkUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    // extract only the filename
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id;
                    // store the file inside ~/App_Data/uploads folder
                    news.LinkUrl = linkurl;

                    db.News.Add(news);

                }
                else
                {
					db.News.Add(news);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(news);
        }

        // GET: Admin/News/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
			var userid = User.Identity.GetUserId();
            var newsa = from item in db.News where (item.NewsId == id) select item;
            newsa = newsa.Include(a => a.CreatedBy);
            newsa = newsa.Where(a => a.CreatedBy == userid);
            if (newsa.Count() == 0)
            {
                news = null;
            }
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/News/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
					[ValidateInput(false)] 
		
					public ActionResult Edit([Bind(Include = "NewsId,Title,LinkUrl,ImageUrl,Description,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] News news,HttpPostedFileBase ImageUrl, HttpPostedFileBase LinkUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
				
				news.ModifiedBy = User.Identity.GetUserId();
                    news.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/news";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/news/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					news.ImageUrl = imageUrl;
                }
                else if (LinkUrl != null && LinkUrl.ContentLength > 0)
                {
                    string pathToCreate = "~/UploadedDocuments/News";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(LinkUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(LinkUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string linkurl = "/UploadedDocuments/News/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;
                    LinkUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    // extract only the filename
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id;
                    // store the file inside ~/App_Data/uploads folder
                    news.LinkUrl = linkurl;

                    db.News.Add(news);

                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(news);
        }

        // GET: Admin/News/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            News news = db.News.Find(id);
			var userid = User.Identity.GetUserId();
            var newsa = from item in db.News where (item.NewsId == id) select item;
            newsa = newsa.Include(a => a.CreatedBy);
            newsa = newsa.Where(a => a.CreatedBy == userid);
            if (newsa.Count() == 0)
            {
                news = null;
            }

            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        // POST: Admin/News/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            News news = db.News.Find(id);
            db.News.Remove(news);
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
