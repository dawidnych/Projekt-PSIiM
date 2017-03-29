using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LastMinuteWebApp.Models;

namespace LastMinuteWebApp.Controllers
{
    public class HomeController : Controller
    {
        GrouponDBEntities DbOffert = new GrouponDBEntities();

        public ActionResult Index()
        {
            return View(DbOffert.Offert.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}