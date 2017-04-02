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
        GrouponDBEntities2 DBOffert = new GrouponDBEntities2();

        public ActionResult MyOfferts()
        {
            // ToDo: currently logged in client business id
            int constIDClientBusiness = 1;

            var myOfferts = (from o in DBOffert.Offert select o)
                .Where(o => o.idClientBusiness == constIDClientBusiness);

            return View(myOfferts);
        }
    }
}