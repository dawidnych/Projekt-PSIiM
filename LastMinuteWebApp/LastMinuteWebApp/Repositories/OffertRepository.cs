using LastMinuteWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LastMinuteWebApp.Repositories
{
    public class OffertRepository
    {
        private static GrouponDBEntities2 DBOffert = new GrouponDBEntities2();

        public static List<Offert> GetAll()
        {
            return DBOffert.Offert.ToList();
        }
    }
}