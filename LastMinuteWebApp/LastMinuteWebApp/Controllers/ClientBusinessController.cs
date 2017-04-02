using LastMinuteWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LastMinuteWebApp.Controllers
{
    public class ClientBusinessController : Controller
    {
        // ToDo: currently logged in business client id
        int constIdClientBusiness = 1;

        GrouponDBEntities2 DBOffert = new GrouponDBEntities2();


        public ActionResult MyOfferts()
        {
            var myOfferts = (from o in DBOffert.Offert select o)
                .Where(o => o.idClientBusiness == constIdClientBusiness);

            return View(myOfferts);
        }

        [HttpGet]
        public ActionResult AddOffert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddOffert(Offert offert)
        {
            if (ModelState.IsValid)
            {
                offert.idClientBusiness = constIdClientBusiness;
                //DBOffert.Offert.Add(offert);
                //DBOffert.SaveChanges();

                TempData["message"] = string.Format("Dodano {0}", offert.title);

                return RedirectToAction("MyOfferts");
            }
            else
                return View();

        }
    }
}