using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using LastMinuteWebApp.Models;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using System.Web.Security;
using System.Web.WebPages;
using LastMinuteWebApp.Extensions;
using PagedList;

namespace LastMinuteWebApp.Controllers
{
    [Authorize(Users = "admin@example.com")]
    public class AdminController : Controller
    {
        private readonly GrouponDBEntities2 _appDbContext;
        // GET: Admin
        public AdminController()
        {
            _appDbContext = new GrouponDBEntities2();
        }

        public ActionResult Index(string currentFilter, string searchString, int? page)
        {

            var uesrs = from user in UserManager.Users
                        orderby user.UserName ascending
                        select user;

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                uesrs = uesrs.Where(u => u.UserName.Contains(searchString) || u.Email.Contains(searchString)).OrderBy(u => u.UserName);
            }


            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(uesrs.ToPagedList(pageNumber, pageSize));
        }


      
        [HttpPost]
        public async Task<ActionResult> Delete(int id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "User Not Found" });
            }
        }



        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }


        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
    }
}