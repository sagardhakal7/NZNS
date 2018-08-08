
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
    public class PastExComMembersController : Controller
    
	{
		
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/PastExComMembers
        public ActionResult Index(string sortingOrder, string searchData, string filterValue, int? pageNo)
        {
            ViewBag.CurrentSortOrder = sortingOrder;
            
															ViewBag.SortingName = String.IsNullOrEmpty(sortingOrder) ? "Name" : "";
				            												ViewBag.SortingPosition = String.IsNullOrEmpty(sortingOrder) ? "Position" : "";
				            																					ViewBag.SortingImageUrl = String.IsNullOrEmpty(sortingOrder) ? "ImageUrl" : "";
				            												ViewBag.SortingYear = String.IsNullOrEmpty(sortingOrder) ? "Year" : "";
				            												ViewBag.SortingLinkedin = String.IsNullOrEmpty(sortingOrder) ? "Linkedin" : "";
				            												ViewBag.SortingFacebook = String.IsNullOrEmpty(sortingOrder) ? "Facebook" : "";
				            												ViewBag.SortingEmail = String.IsNullOrEmpty(sortingOrder) ? "Email" : "";
				            												ViewBag.SortingMobile = String.IsNullOrEmpty(sortingOrder) ? "Mobile" : "";
				            												ViewBag.SortingLandline = String.IsNullOrEmpty(sortingOrder) ? "Landline" : "";
				            												ViewBag.SortingAddress = String.IsNullOrEmpty(sortingOrder) ? "Address" : "";
				            												ViewBag.SortingDOB = String.IsNullOrEmpty(sortingOrder) ? "DOB" : "";
				            												ViewBag.SortingUniqueId = String.IsNullOrEmpty(sortingOrder) ? "UniqueId" : "";
				            			
			
			var items = from item in db.PastExComMembers select item;
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
																				
													
																				
													
															item.Year.ToUpper().Contains(searchData.ToUpper()) ||
																				
													
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
            			case "Year":
											items = items.OrderByDescending(item => item.Year);
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

        // GET: Admin/PastExComMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
           PastExComMember pastExComMember = db.PastExComMembers.Find(id);
			//Below Lines added for Community
            var userid = User.Identity.GetUserId();
            var pastExComMembera = from item in db.PastExComMembers where (item.PastExComMemberId == id) select item;
            pastExComMembera = pastExComMembera.Include(a => a.CreatedBy);
            pastExComMembera = pastExComMembera.Where(a => a.CreatedBy == userid);
            if (pastExComMembera.Count() == 0)
            {
                pastExComMember = null;
            }





            if (pastExComMember == null)
            {
                return HttpNotFound();
            }
            return View(pastExComMember);
        }

        // GET: Admin/PastExComMembers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/PastExComMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
        			public ActionResult Create([Bind(Include = "PastExComMemberId,Name,Position,ImageUrl,Year,Linkedin,Facebook,Email,Mobile,Landline,Address,DOB,UniqueId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] PastExComMember pastExComMember, HttpPostedFileBase ImageUrl)
		
        {
            if (ModelState.IsValid)
            {
					db.Entry(pastExComMember).State = EntityState.Added;
					pastExComMember.CreatedBy = User.Identity.GetUserId();
                    pastExComMember.CreatedDate = DateTime.Now;
               
			 if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/pastExComMember";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/pastExComMember/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
                    string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
                 "width=200;height=200;format=jpg;mode=max"));
					i.Build();
					
					pastExComMember.ImageUrl = imageUrl;
	                    
						db.PastExComMembers.Add(pastExComMember);

                }
                else
                {
					db.PastExComMembers.Add(pastExComMember);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
		
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(pastExComMember);
        }

        // GET: Admin/PastExComMembers/Edit/5
        
		public ActionResult Edit(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PastExComMember pastExComMember = db.PastExComMembers.Find(id);
			var userid = User.Identity.GetUserId();
            var pastExComMembera = from item in db.PastExComMembers where (item.PastExComMemberId == id) select item;
            pastExComMembera = pastExComMembera.Include(a => a.CreatedBy);
            pastExComMembera = pastExComMembera.Where(a => a.CreatedBy == userid);
            if (pastExComMembera.Count() == 0)
            {
                pastExComMember = null;
            }
            if (pastExComMember == null)
            {
                return HttpNotFound();
            }
            return View(pastExComMember);
        }

        // POST: Admin/PastExComMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		
					public ActionResult Edit([Bind(Include = "PastExComMemberId,Name,Position,ImageUrl,Year,Linkedin,Facebook,Email,Mobile,Landline,Address,DOB,UniqueId,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy,DelFlg")] PastExComMember pastExComMember,HttpPostedFileBase ImageUrl)
		
		
        {
            if (ModelState.IsValid)
            {
                db.Entry(pastExComMember).State = EntityState.Modified;
				
				pastExComMember.ModifiedBy = User.Identity.GetUserId();
                    pastExComMember.ModifiedDate = DateTime.Now;
				       
				if (ImageUrl != null)
                {
                    string pathToCreate = "~/Images/pastExComMember";
                    if (!Directory.Exists(Server.MapPath(pathToCreate)))
                    {
                        //Now you know it is ok, create it
                        Directory.CreateDirectory(Server.MapPath(pathToCreate));
                    }
                    string extension = Path.GetExtension(ImageUrl.FileName);
                    string fileName = Path.GetFileNameWithoutExtension(ImageUrl.FileName);
                    string id = Guid.NewGuid().ToString();
                    string imageUrl = "/Images/pastExComMember/" + fileName + "" + /*main.MainId.ToString()*/ id + extension;

                    ImageUrl.SaveAs(Path.Combine(Server.MapPath(pathToCreate), fileName + "" + /*main.MainId.ToString()*/id + extension));
					string DestinationPath = Path.Combine(Server.MapPath(pathToCreate));
                    DestinationPath += "\\" + fileName + id ;
					ImageResizer.ImageJob i = new ImageResizer.ImageJob(DestinationPath + extension , DestinationPath + "_thumb.jpg" , new ImageResizer.ResizeSettings(
					"width=200;height=200;format=jpg;mode=max"));
					i.Build();
					pastExComMember.ImageUrl = imageUrl;
                }
				                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(pastExComMember);
        }

        // GET: Admin/PastExComMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PastExComMember pastExComMember = db.PastExComMembers.Find(id);
			var userid = User.Identity.GetUserId();
            var pastExComMembera = from item in db.PastExComMembers where (item.PastExComMemberId == id) select item;
            pastExComMembera = pastExComMembera.Include(a => a.CreatedBy);
            pastExComMembera = pastExComMembera.Where(a => a.CreatedBy == userid);
            if (pastExComMembera.Count() == 0)
            {
                pastExComMember = null;
            }

            if (pastExComMember == null)
            {
                return HttpNotFound();
            }
            return View(pastExComMember);
        }

        // POST: Admin/PastExComMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PastExComMember pastExComMember = db.PastExComMembers.Find(id);
            db.PastExComMembers.Remove(pastExComMember);
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
