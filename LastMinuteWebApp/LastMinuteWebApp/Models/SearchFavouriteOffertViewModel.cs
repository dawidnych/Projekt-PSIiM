using LastMinuteWebApp.Lucene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace LastMinuteWebApp.Models
{
    public class SearchFavouriteOffertViewModel
    {
        public IPagedList<FavouriteOffertJoinOffert> Favourites { get; set; }
        public IList<SearchCategoryItem> SearchCategoryList { get; set; }
        public string SearchTerm { get; set; }
        public string SearchCategory { get; set; }
    }
}