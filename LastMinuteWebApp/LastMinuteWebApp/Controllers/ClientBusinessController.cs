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
    [ClientBusinessAuthorizeAttribute]
    [ValidateInput(false)]
    public class ClientBusinessController : Controller
    {

        GrouponDBEntities2 DBConnect = new GrouponDBEntities2();

        public ActionResult OffertManager(int offerId, string searchString)
        {
            var query =
                DBConnect.ClientPrivate.Join(DBConnect.Reservation, c => c.id, r => r.idClientPrivate,
                    (c, r) => new {ClientPrivate = c, Reservation = r}).Where(r=>r.Reservation.idOffert.Equals(offerId));

            List<ReservationClientViewModel> reservationModels = new List<ReservationClientViewModel>();

            foreach (var q in query)
            {
                reservationModels.Add(new ReservationClientViewModel {client = q.ClientPrivate, reservation = q.Reservation});
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                reservationModels = reservationModels.Where(s => s.reservation.Code.Contains(searchString) || s.client.UserName.Contains(searchString)).ToList();
            }

            EventManagerViewModel model = new EventManagerViewModel();
            model.offert = DBConnect.Offert.First(o => o.id.Equals(offerId));
            model.reservations = reservationModels;
            return View(model);
        }

        public ActionResult BookReservation(int rezervationId, int Id)
        {
            var rezervation = DBConnect.Reservation.First(r => r.id.Equals(rezervationId));
            rezervation.active = 1;
            DBConnect.SaveChanges();
            return RedirectToAction("OffertManager", new { offerId = Id});
        }

        public ActionResult MyOfferts(string searchTerm, string searchCategory, int? page)
        {
            SearchOffert.ClearLuceneIndex();
            SearchOffert.AddUpdateLuceneIndex(DBConnect.Offert.ToList());

            if (!Directory.Exists(SearchOffert._luceneDir))
                Directory.CreateDirectory(SearchOffert._luceneDir);

            List<Offert> _searchResults = (string.IsNullOrEmpty(searchCategory)
                           ? SearchOffert.Search(true, searchTerm)
                           : SearchOffert.Search(true, searchTerm, searchCategory)).ToList();

            if (string.IsNullOrEmpty(searchTerm) && !_searchResults.Any())
                _searchResults = SearchOffert.GetAllIndexRecords().ToList();

            int clientPrivateId = User.Identity.GetUserId<Int32>();
            ClientPrivate clientPrivate = DBConnect.ClientPrivate.Find(clientPrivateId);
            int clientBusinessId = (int)clientPrivate.idClientBusiness;

            _searchResults = (from o in _searchResults
                              where o.idClientBusiness == clientBusinessId
                              select o).ToList();

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

            return View(new SearchOffertViewModel
            {
                SearchResults = _searchResults.ToPagedList(pageNumber, pageSize),
                SearchCategoryList = _searchCategoryList,
                SearchTerm = searchTerm,
                SearchCategory = searchCategory
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
                    foreach (var reservation in reservations)
                    {
                        DBConnect.Reservation.Remove(reservation);
                    }

                    var favourites = (from f in DBConnect.FavouriteOffert
                                      where f.idOffert == offert.id
                                      select f).ToList();
                    foreach (var favourite in favourites)
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
                int clientPrivateId = User.Identity.GetUserId<Int32>();
                ClientPrivate clientPrivate = DBConnect.ClientPrivate.Find(clientPrivateId);
                int clientBusinessId = (int)clientPrivate.idClientBusiness;

                offert.idClientBusiness = clientBusinessId;
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