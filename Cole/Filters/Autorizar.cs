using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Cole.Filters
{
    public class Autorizar : AuthorizeAttribute 
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(httpContext.Session["IsAuthenticated"] != null)
            {
                if ((bool)httpContext.Session["IsAuthenticated"])
                {
                    return this.Roles.Contains((String)httpContext.Session["Role"]);

                }
            }

            return false;
            
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectResult("~/Login/Index");
        }
    }
}