//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LastMinuteWebApp.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Reservation
    {
        public long id { get; set; }
        public long idClientPrivate { get; set; }
        public long idOffert { get; set; }
        public string Code { get; set; }
        public int active { get; set; }
    }
}
