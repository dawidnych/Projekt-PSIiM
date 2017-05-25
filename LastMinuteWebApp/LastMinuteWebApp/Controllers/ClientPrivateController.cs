using LastMinuteWebApp.Lucene;
using LastMinuteWebApp.Models;
using LastMinuteWebApp.Repositories;
using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LastMinuteWebApp.Controllers
{
    [Authorize]
    [ValidateInput(false)]
    public class ClientPrivateController : Controller
    {

        GrouponDBEntities2 DBConnect = new GrouponDBEntities2();


        public ActionResult MyReservations(string searchTerm, string searchCategory, int? page)
        {
            SearchOffert.ClearLuceneIndex();
            SearchOffert.AddUpdateLuceneIndex(DBConnect.Offert.ToList());

            if (!Directory.Exists(SearchOffert._luceneDir))
                Directory.CreateDirectory(SearchOffert._luceneDir);

            List<Offert> searchResults = (string.IsNullOrEmpty(searchCategory)
                           ? SearchOffert.Search(false, searchTerm)
                           : SearchOffert.Search(false, searchTerm, searchCategory)).ToList();

            if (string.IsNullOrEmpty(searchTerm) && !searchResults.Any())
                searchResults = SearchOffert.GetAllIndexRecords().ToList();

            int clientPrivateId = User.Identity.GetUserId<Int32>();

            var reservations = (from r in DBConnect.Reservation
                                where r.idClientPrivate == clientPrivateId
                                select r).ToList();

            var _reservations = from r in reservations
                                join o in searchResults on r.idOffert equals o.id
                                select new ReservationJoinOffert { ReservationData = r, OffertData = o };

            var _searchCategoryList = new List<SearchCategoryItem> {
                new SearchCategoryItem {Text = "(All Fields)", Value = ""},
                new SearchCategoryItem {Text = "Title", Value = "title"},
                new SearchCategoryItem {Text = "Description", Value = "description"},
                new SearchCategoryItem {Text = "Price", Value = "price"},
                new SearchCategoryItem {Text = "Deadline Time", Value = "deadlineTime" }
            };

            int pageNumber = (page ?? 1);
            int pageSize = 6;

            return View(new SearchReservationViewModel
            {
                Reservations = _reservations.ToPagedList(pageNumber, pageSize),
                SearchCategoryList = _searchCategoryList,
                SearchTerm = searchTerm,
                SearchCategory = searchCategory
            });
        }

        public ActionResult SearchReservations(string searchTerm, string searchCategory)
        {
            return RedirectToAction("MyReservations", new { searchTerm, searchCategory });
        }

        [HttpPost]
        public ActionResult DeleteReservation(int reservationId)
        {
            Reservation reservation = DBConnect.Reservation.Find(reservationId);
            if (reservation != null)
            {
                Offert offert = DBConnect.Offert.Find(reservation.idOffert);
                if (offert != null)
                {
                    if (DateTime.Compare(offert.deadlineTime, DateTime.Now) > 0)
                    {
                        DBConnect.Reservation.Remove(reservation);

                        offert.quantity += 1;
                        DBConnect.Offert.AsEnumerable().Where(o => o.id == reservation.idOffert).ToList().ForEach(o => o.quantity = offert.quantity);

                        DBConnect.SaveChanges();
                        SearchOffert.AddUpdateLuceneIndex(offert);

                        TempData["message"] = "Reservation cancelled";
                    }
                    else
                    {
                        TempData["error"] = "Deadline time is up. You can't cancel reservation";
                    }
                }
            }

            return RedirectToAction("MyReservations");
        }

        public ActionResult ReservationDiteils(int offertId)
        {
            Offert offert = DBConnect.Offert.Find(offertId);
            
                
                if (offert != null)
                {
                    if (DateTime.Compare(offert.deadlineTime, DateTime.Now) > 0)
                    {
                        return View(offert);
                    }
                    else
                    {
                        TempData["error"] = "Deadline time is up. You can't see detils of the offert";
                    }
                }
            

            return RedirectToAction("Index", "Home");
        }

        public ActionResult MyFavouriteOfferts(string searchTerm, string searchCategory, int? page)
        {
            SearchOffert.ClearLuceneIndex();
            SearchOffert.AddUpdateLuceneIndex(DBConnect.Offert.ToList());

            if (!Directory.Exists(SearchOffert._luceneDir))
                Directory.CreateDirectory(SearchOffert._luceneDir);

            List<Offert> searchResults = (string.IsNullOrEmpty(searchCategory)
                           ? SearchOffert.Search(true, searchTerm)
                           : SearchOffert.Search(true, searchTerm, searchCategory)).ToList();

            if (string.IsNullOrEmpty(searchTerm) && !searchResults.Any())
                searchResults = SearchOffert.GetAllIndexRecords().ToList();

            int clientPrivateId = User.Identity.GetUserId<Int32>();

            var favourites = (from f in DBConnect.FavouriteOffert
                              where f.idClientPrivate == clientPrivateId
                              select f).ToList();

            var _favourites = from f in favourites
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

            int pageNumber = (page ?? 1);
            int pageSize = 6;

            return View(new SearchFavouriteOffertViewModel
            {
                Favourites = _favourites.ToPagedList(pageNumber, pageSize),
                SearchCategoryList = _searchCategoryList,
                SearchTerm = searchTerm,
                SearchCategory = searchCategory
            });
        }

        public ActionResult SearchOfferts(string searchTerm, string searchCategory)
        {
            return RedirectToAction("MyFavouriteOfferts", new { searchTerm, searchCategory });
        }

        [HttpPost]
        public ActionResult DeleteOffert(int favouriteOffertId)
        {
            FavouriteOffert favouriteOffert = DBConnect.FavouriteOffert.Find(favouriteOffertId);
            if (favouriteOffert != null)
            {
                DBConnect.FavouriteOffert.Remove(favouriteOffert);
                DBConnect.SaveChanges();

                TempData["message"] = "Deleted from favourites";
            }

            return RedirectToAction("MyFavouriteOfferts");
        }

        [HttpPost]
        public ActionResult ReserveOffert(int favouriteOffertId)
        {
            FavouriteOffert favouriteOffert = DBConnect.FavouriteOffert.Find(favouriteOffertId);
            if (favouriteOffert != null)
            {
                Offert offert = DBConnect.Offert.Find(favouriteOffert.idOffert);
                if (offert != null)
                {
                    if (DateTime.Compare(offert.deadlineTime, DateTime.Now) > 0 && offert.quantity > 0)
                    {
                        int clientPrivateId = User.Identity.GetUserId<Int32>();

                        Random random = new Random();
                        string chars = "1234567890";
                        string randomString = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());

                        Reservation reservation = new Reservation
                        {
                            idClientPrivate = clientPrivateId,
                            idOffert = offert.id,
                            Code = randomString
                        };
                        DBConnect.Reservation.Add(reservation);

                        offert.quantity -= 1;
                        DBConnect.Offert.AsEnumerable().Where(o => o.id == favouriteOffert.idOffert).ToList().ForEach(o => o.quantity = offert.quantity);

                        DBConnect.SaveChanges();
                        SearchOffert.AddUpdateLuceneIndex(offert);

                        TempData["message"] = "Reservation successful";
                    }
                    else
                    {
                        TempData["error"] = "You can't reserve this offert because time is up or all tickets have beed reserved";
                    }
                }
            }

            return RedirectToAction("MyReservations");
        }
    }
}