using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LastMinuteWebApp.Models;
using PagedList;

namespace LastMinuteWebApp.Controllers
{
    [ValidateInput(false)]
    public class HomeController : Controller
    {
        GrouponDBEntities2 DbOffert = new GrouponDBEntities2();



        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            var offerts = from s in DbOffert.Offert
                           select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                offerts = offerts.Where(s => s.title.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    offerts = offerts.OrderByDescending(s => s.title);
                    break;
                case "Date":
                    offerts = offerts.OrderBy(s => s.deadlineTime);
                    break;
                case "date_desc":
                    offerts = offerts.OrderByDescending(s => s.deadlineTime);
                    break;
                default:
                    offerts = offerts.OrderByDescending(s => s.price);
                    break;
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);
            return View(offerts.ToPagedList(pageNumber, pageSize));

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