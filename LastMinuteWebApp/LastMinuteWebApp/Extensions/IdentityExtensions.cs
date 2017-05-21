using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace LastMinuteWebApp.Extensions
{
    public static class IdentityExtensions
    {
        public static string GetidClientBusiness(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity) identity).FindFirst("idClientBusiness");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static bool isClientBusiness(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("idClientBusiness");
            // Test for null to avoid issues during local testing
            return (claim != null);
        }
    }
}