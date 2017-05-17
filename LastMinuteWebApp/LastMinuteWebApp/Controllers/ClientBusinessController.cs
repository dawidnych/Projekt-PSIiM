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

        GrouponDBEntities2 DBConnect = new GrouponDBEntities2();



        public ActionResult MyOfferts(string searchTerm, string searchCategory)
        {
            SearchOffert.AddUpdateLuceneIndex(OffertRepository.GetAll());

            if (!Directory.Exists(SearchOffert._luceneDir))
                Directory.CreateDirectory(SearchOffert._luceneDir);

            List<Offert> _searchResults = (string.IsNullOrEmpty(searchCategory)
                           ? SearchOffert.Search(true, searchTerm)
                           : SearchOffert.Search(true, searchTerm, searchCategory)).ToList();

            if (string.IsNullOrEmpty(searchTerm) && !_searchResults.Any())
                _searchResults = SearchOffert.GetAllIndexRecords().ToList();

            _searchResults = (from o in _searchResults
                              where o.idClientBusiness == constIdClientBusiness
                              select o).ToList();

            var _searchCategoryList = new List<SearchCategoryItem> {
                new SearchCategoryItem {Text = "(All Fields)", Value = ""},
                new SearchCategoryItem {Text = "Title", Value = "title"},
                new SearchCategoryItem {Text = "Description", Value = "description"},
                new SearchCategoryItem {Text = "Price", Value = "price"},
                new SearchCategoryItem {Text = "Quantity", Value = "quantity" },
                new SearchCategoryItem {Text = "Deadline Time", Value = "deadlineTime" }
            };

            return View(new SearchOffertViewModel
            {
                SearchResults = _searchResults,
                SearchCategoryList = _searchCategoryList
            });
        }

        public ActionResult Search(string searchTerm, string searchCategory)
        {
            return RedirectToAction("MyOfferts", new { searchTerm, searchCategory });
        }

        [HttpPost]
        public ActionResult DeleteOffert(int offertId)
        {
            Offert offert = DBConnect.Offert.Find(offertId);
            if (offert != null)
            {
                if (DateTime.Compare(offert.deadlineTime, DateTime.Now) > 0)
                {
                    var reservations = (from r in DBConnect.Reservation
                                       where r.idOffert == offert.id
                                       select r).ToList();
                    foreach(var reservation in reservations)
                    {
                        DBConnect.Reservation.Remove(reservation);
                    }

                    var favourites = (from f in DBConnect.FavouriteOffert
                                      where f.idOffert == offert.id
                                      select f).ToList();
                    foreach(var favourite in favourites)
                    {
                        DBConnect.FavouriteOffert.Remove(favourite);
                    }

                    DBConnect.Offert.Remove(offert);
                    SearchOffert.ClearLuceneIndexRecord(offertId);

                    DBConnect.SaveChanges();

                    TempData["message"] = "Offert deleted";
                }
                else
                {
                    TempData["error"] = "You can't delete this offert because time is up";
                }
            }

            return RedirectToAction("MyOfferts");
        }



        public ActionResult AddOffert()
        {
            return View(new Offert());
        }

        [HttpPost]
        public ActionResult AddOffert(Offert offert)
        {
            if (ModelState.IsValid)
            {
                offert.idClientBusiness = constIdClientBusiness;
                DBConnect.Offert.Add(offert);
                DBConnect.SaveChanges();
                SearchOffert.AddUpdateLuceneIndex(offert);

                TempData["message"] = string.Format("{0} added", offert.title);

                return RedirectToAction("MyOfferts");
            }
            else
                return View();

        }
    }
}