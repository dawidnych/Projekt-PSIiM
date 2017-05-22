using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LastMinuteWebApp.Models;
using PagedList;
using LastMinuteWebApp.Lucene;
using System.IO;
using Microsoft.AspNet.Identity;

namespace LastMinuteWebApp.Controllers
{
    [ValidateInput(false)]
    public class HomeController : Controller
    {

        GrouponDBEntities2 DbOffert = new GrouponDBEntities2();


        public ActionResult Index(string searchTerm, string searchCategory, int? page)
        {
            SearchOffert.ClearLuceneIndex();
            SearchOffert.AddUpdateLuceneIndex(DbOffert.Offert.ToList());

            if (!Directory.Exists(SearchOffert._luceneDir))
                Directory.CreateDirectory(SearchOffert._luceneDir);

            List<Offert> _searchResults = (string.IsNullOrEmpty(searchCategory)
                           ? SearchOffert.Search(true, searchTerm)
                           : SearchOffert.Search(true, searchTerm, searchCategory)).ToList();

            if (string.IsNullOrEmpty(searchTerm) && !_searchResults.Any())
                _searchResults = SearchOffert.GetAllIndexRecords().ToList();

            var _offerts = (from o in _searchResults
                            where (o.deadlineTime > DateTime.Now && o.quantity > 0)
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
                SearchResults = _offerts.ToPagedList(pageNumber, pageSize),
                SearchCategoryList = _searchCategoryList,
                SearchTerm = searchTerm,
                SearchCategory = searchCategory
            });

        }

        public ActionResult Search(string searchTerm, string searchCategory)
        {
            return RedirectToAction("Index", new { searchTerm, searchCategory });
        }

        [HttpPost]
        public ActionResult Reserve(int offertId)
        {
            Offert offert = DbOffert.Offert.Find(offertId);
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
                    DbOffert.Reservation.Add(reservation);

                    offert.quantity -= 1;
                    DbOffert.Offert.AsEnumerable().Where(o => o.id == offert.id).ToList().ForEach(o => o.quantity = offert.quantity);

                    DbOffert.SaveChanges();
                    SearchOffert.AddUpdateLuceneIndex(offert);

                    TempData["message"] = "Reservation successful";
                }
                else
                {
                    TempData["error"] = "You can't reserve this offert because time is up or all tickets have beed reserved";
                }
            }

            return RedirectToAction("MyReservations", "ClientPrivate");
        }

        [HttpPost]
        public ActionResult AddToFavourites(int offertId)
        {
            Offert offert = DbOffert.Offert.Find(offertId);
            if (offert != null)
            {
                if (DateTime.Compare(offert.deadlineTime, DateTime.Now) > 0 && offert.quantity > 0)
                {
                    int clientPrivateId = User.Identity.GetUserId<Int32>();

                    var sameOffert = (from f in DbOffert.FavouriteOffert
                                      where (f.idOffert == offert.id && f.idClientPrivate == clientPrivateId)
                                      select f).ToList();

                    if (!sameOffert.Any())
                    {
                        FavouriteOffert favourite = new FavouriteOffert
                        {
                            idClientPrivate = clientPrivateId,
                            idOffert = offert.id
                        };
                        DbOffert.FavouriteOffert.Add(favourite);
                        DbOffert.SaveChanges();

                        TempData["message"] = "Successfully added to favourites";
                    }
                    else
                    {
                        TempData["error"] = "This offert is already in your favourites";
                    }
                }
                else
                {
                    TempData["error"] = "You can't add this offert to favourites because time is up or all tickets have beed reserved";
                }
            }

            return RedirectToAction("MyFavouriteOfferts", "ClientPrivate");
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