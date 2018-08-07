using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NZNA.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult AllMembers()
        {
            return View();
        }
        public ActionResult Events()
        {
            return View();
        }
        public ActionResult ExecutiveCommitte()
        {
            return View();
        }
        public ActionResult PastExecutive()
        {
            return View();
        }
        public ActionResult Resources()
        {
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}