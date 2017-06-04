using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LastMinuteWebApp.Models
{
    public class EventManagerViewModel
    {
        public Offert offert { get; set;}
        public List<ReservationClientViewModel> reservations { get; set; }
    }

    public class ReservationClientViewModel
    {
        public ClientPrivate client { get; set; }
        public Reservation reservation { get; set; }
    }
}