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
    public class ClientPrivateController : Controller
    {
        // ToDo: currently logged in private client id
        int constIdClientPrivate = 143;

        GrouponDBEntities2 DBConnect = new GrouponDBEntities2();

        public ActionResult MyReservations(string searchTerm, string searchCategory)
        {
            SearchOffert.AddUpdateLuceneIndex(OffertRepository.GetAll());

            if (!Directory.Exists(SearchOffert._luceneDir))
                Directory.CreateDirectory(SearchOffert._luceneDir);

            List<Offert> searchResults = (string.IsNullOrEmpty(searchCategory)
                           ? SearchOffert.Search(false, searchTerm)
                           : SearchOffert.Search(false, searchTerm, searchCategory)).ToList();

            if (string.IsNullOrEmpty(searchTerm) && !searchResults.Any())
                searchResults = SearchOffert.GetAllIndexRecords().ToList();

            var reservations = (from r in DBConnect.Reservation
                                 select r).Where(r => r.idClientPrivate == constIdClientPrivate).ToList();

            var _reservations = from r in reservations
                                join o in searchResults on r.idOffert equals o.id
                                select new ReservationJoinOffert { ReservationData = r, OffertData = o };

            var _searchCategoryList = new List<SearchCategoryItem> {
                new SearchCategoryItem {Text = "(All Fields)", Value = ""},
                new SearchCategoryItem {Text = "Title", Value = "title"},
                new SearchCategoryItem {Text = "Description", Value = "description"},
                new SearchCategoryItem {Text = "Price", Value = "price"}
            };

            return View(new SearchReservationViewModel
            {
                Reservations = _reservations,
                SearchCategoryList = _searchCategoryList
            });
        }

        public ActionResult SearchReservations(string searchTerm, string searchCategory)
        {
            return RedirectToAction("MyReservations", new { searchTerm, searchCategory });
        }

        public ActionResult MyFavouriteOfferts(string searchTerm, string searchCategory)
        {
            SearchOffert.AddUpdateLuceneIndex(OffertRepository.GetAll());

            if (!Directory.Exists(SearchOffert._luceneDir))
                Directory.CreateDirectory(SearchOffert._luceneDir);

            List<Offert> searchResults = (string.IsNullOrEmpty(searchCategory)
                           ? SearchOffert.Search(true, searchTerm)
                           : SearchOffert.Search(true, searchTerm, searchCategory)).ToList();

            if (string.IsNullOrEmpty(searchTerm) && !searchResults.Any())
                searchResults = SearchOffert.GetAllIndexRecords().ToList();

            var favouritess = (from f in DBConnect.FavouriteOffert
                                select f).Where(r => r.idClientPrivate == constIdClientPrivate).ToList();

            var _favourites = from f in favouritess
                                join o in searchResults on f.idOffert equals o.id
                                select new FavouriteOffertJoinOffert { FavouriteOffertData = f, OffertData = o };

            var _searchCategoryList = new List<SearchCategoryItem> {
                new SearchCategoryItem {Text = "(All Fields)", Value = ""},
                new SearchCategoryItem {Text = "Title", Value = "title"},
                new SearchCategoryItem {Text = "Description", Value = "description"},
                new SearchCategoryItem {Text = "Price", Value = "price"},
                new SearchCategoryItem {Text = "Quantity", Value = "quantity" },
                new SearchCategoryItem {Text = "Deadline Time", Value = "deadlineTime" }
            };

            return View(new SearchFavouriteOffertViewModel
            {
                Favourites = _favourites,
                SearchCategoryList = _searchCategoryList
            });
        }

        public ActionResult SearchOfferts(string searchTerm, string searchCategory)
        {
            return RedirectToAction("MyFavouriteOfferts", new { searchTerm, searchCategory });
        }
    }
}