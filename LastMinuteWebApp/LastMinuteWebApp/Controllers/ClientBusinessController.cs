using LastMinuteWebApp.Lucene;
using LastMinuteWebApp.Models;
using LastMinuteWebApp.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LastMinuteWebApp.Controllers
{
    public class ClientBusinessController : Controller
    {
        // ToDo: currently logged in business client id
        int constIdClientBusiness = 88;

        GrouponDBEntities2 DBOffert = new GrouponDBEntities2();

        public ActionResult MyOfferts(string searchTerm, string searchCategory)
        {
            LuceneSearch.AddUpdateLuceneIndex(OffertRepository.GetAll());

            if (!Directory.Exists(LuceneSearch._luceneDir))
                Directory.CreateDirectory(LuceneSearch._luceneDir);

            List<Offert> _searchResults = (string.IsNullOrEmpty(searchCategory)
                           ? LuceneSearch.Search(searchTerm)
                           : LuceneSearch.Search(searchTerm, searchCategory)).ToList();

            if (string.IsNullOrEmpty(searchTerm) && !_searchResults.Any())
                _searchResults = LuceneSearch.GetAllIndexRecords().ToList();

            _searchResults = (from o in _searchResults select o)
                .Where(o => o.idClientBusiness == constIdClientBusiness).ToList();

            var _searchCategoryList = new List<SearchCategoryItem> {
                new SearchCategoryItem {Text = "(All Fields)", Value = ""},
                new SearchCategoryItem {Text = "Title", Value = "title"},
                new SearchCategoryItem {Text = "Description", Value = "description"},
                new SearchCategoryItem {Text = "Price", Value = "price"},
                new SearchCategoryItem {Text = "Quantity", Value = "quantity" },
                new SearchCategoryItem {Text = "Deadline Time", Value = "deadlineTime" }
            };

            return View(new SearchViewModel
            {
                SearchResults = _searchResults,
                SearchCategoryList = _searchCategoryList
            });
        }

        public ActionResult Search(string searchTerm, string searchCategory)
        {
            return RedirectToAction("MyOfferts", new { searchTerm, searchCategory });
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
                //  LuceneSearch.AddUpdateLuceneIndex(offert);

                TempData["message"] = string.Format("Dodano {0}", offert.title);

                return RedirectToAction("MyOfferts");
            }
            else
                return View();

        }
    }
}