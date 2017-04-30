using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LastMinuteWebApp.Models
{
    public class ReservationJoinOffert
    {
        public Reservation ReservationData { get; set; }
        public Offert OffertData { get; set; }
    }
}