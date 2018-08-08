
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
    public class MembersController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Members
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingName = String.IsNullOrEmpty(sortingOrder) ? "Name" : "";
				            												ViewBag.SortingPosition = String.IsNullOrEmpty(sortingOrder) ? "Position" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            												ViewBag.SortingIsExcom = String.IsNullOrEmpty(sortingOrder) ? "IsExcom" : "";
				            												ViewBag.SortingLinkedin = String.IsNullOrEmpty(sortingOrder) ? "Linkedin" : "";
				            												ViewBag.SortingFacebook = String.IsNullOrEmpty(sortingOrder) ? "Facebook" : "";
				            												ViewBag.SortingEmail = String.IsNullOrEmpty(sortingOrder) ? "Email" : "";
				            												ViewBag.SortingMobile = String.IsNullOrEmpty(sortingOrder) ? "Mobile" : "";
				            												ViewBag.SortingLandline = String.IsNullOrEmpty(sortingOrder) ? "Landline" : "";
				            												ViewBag.SortingAddress = String.IsNullOrEmpty(sortingOrder) ? "Address" : "";
				            												ViewBag.SortingDOB = String.IsNullOrEmpty(sortingOrder) ? "DOB" : "";
				            												ViewBag.SortingUniqueId = String.IsNullOrEmpty(sortingOrder) ? "UniqueId" : "";
				            			
			
			var items = from item in db.Members select item;
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
																				
													
															item.Position.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
																				
													
															item.IsExcom.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Linkedin.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Facebook.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Email.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Mobile.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Landline.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.Address.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
															item.DOB.ToUpper().Contains(searchData.ToUpper()) ||
																				
																								item.UniqueId.ToUpper().Contains(searchData.ToUpper()));
																					
						}
			ViewBag.FilterValue = searchData;
			 switch (sortingOrder)
            {
						case "Name":
											items = items.OrderByDescending(item => item.Name);
                    					break;
            			case "Position":
											items = items.OrderByDescending(item => item.Position);
                    					break;
            			case "ImageUrl":
											items = items.OrderByDescending(item => item.ImageUrl);
                    					break;
            			case "IsExcom":
											items = items.OrderByDescending(item => item.IsExcom);
                    					break;
            			case "Linkedin":
											items = items.OrderByDescending(item => item.Linkedin);
                    					break;
            			case "Facebook":
											items = items.OrderByDescending(item => item.Facebook);
                    					break;
            			case "Email":
											items = items.OrderByDescending(item => item.Email);
                    					break;
            			case "Mobile":
											items = items.OrderByDescending(item => item.Mobile);
                    					break;
            			case "Landline":
											items = items.OrderByDescending(item => item.Landline);
                    					break;
            			case "Address":
											items = items.OrderByDescending(item => item.Address);
                    					break;
            			case "DOB":
											items = items.OrderByDescending(item => item.DOB);
                    					break;
            			case "UniqueId":
											items = items.OrderByDescending(item => item.UniqueId);
                    					break;
            			default:
                    items = items.OrderBy(item => item.Name);
                    break;
			}
			const int totalPageSize = 10;
			var noOfPage = (pageNo ?? 1);
			return View(items.ToPagedList(noOfPage ,totalPageSize ));
        }

        // GET: Admin/Members/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           Member member = db.Members.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var membera = from item in db.Members where (item.MemberId == id) select item;
            membera = membera.Include(a => a.CreatedBy);
            membera = membera.Where(a => a.CreatedBy == userid);
            if (membera.Count() == 0)
            {
                member = null;
            }





            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: Admin/Members/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Members/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
        			public ActionResult Create([Bind(Include = "MemberId,Name,Position,ImageUrl,IsExcom,Linkedin,Facebook,Email,Mobile,Landline,Address,DOB,UniqueId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Member member, HttpPostedFileBase ImageUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(member).State = EntityState.Added;
					member.CreatedBy = User.Identity.GetUserId();
                    member.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/member";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/member/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					member.ImageUrl = imageUrl;
	                    
						db.Members.Add(member);

                }
                else
                {
					db.Members.Add(member);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }

        // GET: Admin/Members/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
			var userid = User.Identity.GetUserId();
            var membera = from item in db.Members where (item.MemberId == id) select item;
            membera = membera.Include(a => a.CreatedBy);
            membera = membera.Where(a => a.CreatedBy == userid);
            if (membera.Count() == 0)
            {
                member = null;
            }
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Admin/Members/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
					public ActionResult Edit([Bind(Include = "MemberId,Name,Position,ImageUrl,IsExcom,Linkedin,Facebook,Email,Mobile,Landline,Address,DOB,UniqueId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] Member member,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(member).State = EntityState.Modified;
				
				member.ModifiedBy = User.Identity.GetUserId();
                    member.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/member";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/member/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					member.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        // GET: Admin/Members/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Member member = db.Members.Find(id);
			var userid = User.Identity.GetUserId();
            var membera = from item in db.Members where (item.MemberId == id) select item;
            membera = membera.Include(a => a.CreatedBy);
            membera = membera.Where(a => a.CreatedBy == userid);
            if (membera.Count() == 0)
            {
                member = null;
            }

            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: Admin/Members/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Member member = db.Members.Find(id);
            db.Members.Remove(member);
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
