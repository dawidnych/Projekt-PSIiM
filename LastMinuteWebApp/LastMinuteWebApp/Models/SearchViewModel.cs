using LastMinuteWebApp.Lucene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LastMinuteWebApp.Models
{
    public class SearchViewModel
    {
        public IEnumerable<Offert> SearchResults { get; set; }
        public IList<SearchCategoryItem> SearchCategoryList { get; set; }
        public string SearchTerm { get; set; }
        public string SearchCategory { get; set; }
    }
}