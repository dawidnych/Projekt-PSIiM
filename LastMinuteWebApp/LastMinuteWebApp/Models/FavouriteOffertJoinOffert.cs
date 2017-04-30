using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LastMinuteWebApp.Models
{
    public class FavouriteOffertJoinOffert
    {
        public FavouriteOffert FavouriteOffertData { get; set; }
        public Offert OffertData { get; set; }
    }
}