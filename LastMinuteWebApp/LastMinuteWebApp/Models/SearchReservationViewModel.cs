﻿using LastMinuteWebApp.Lucene;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LastMinuteWebApp.Models
{
    public class SearchReservationViewModel
    {
        public IEnumerable<ReservationJoinOffert> Reservations { get; set; }
        public IList<SearchCategoryItem> SearchCategoryList { get; set; }
        public string SearchTerm { get; set; }
        public string SearchCategory { get; set; }
    }
}