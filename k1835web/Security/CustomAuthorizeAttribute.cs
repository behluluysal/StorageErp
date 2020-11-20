using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using k1835web.Models;

namespace k1835web.Security
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (string.IsNullOrEmpty(SessionPersister.Username))
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admins", action = "Login" }));
            else
            {
                OurDbContext db = new OurDbContext();

                Kullanici test = db.Admins.Where(u => u.Kullaniciadi == SessionPersister.Username).FirstOrDefault();
                CustomPrincipal mp = new CustomPrincipal(test);
                if (!mp.IsInRole(Roles))
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admins", action = "Login" }));
                //  Account test = db.userAccounts.Where(u => u.Username == SessionPersister.Username).FirstOrDefault();
                //   CustomPrincipal mp = new CustomPrincipal(test);am.find(SessionPersister.Username)
            }
        }
    }
}