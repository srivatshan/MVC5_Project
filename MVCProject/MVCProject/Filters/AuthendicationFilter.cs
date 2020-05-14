using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace MVCProject.Filters
{
    public class AuthendicationFilter : ActionFilterAttribute, IAuthenticationFilter
    {
        public void OnAuthentication(AuthenticationContext filterContext)
        {
            var session = filterContext.HttpContext.Session["UserDetails"] ?? filterContext.HttpContext.Request.Cookies.Get("UserDetails");
            if ((string.IsNullOrEmpty(Convert.ToString(session))))
            {
                filterContext.Result = new HttpUnauthorizedResult();
            }
        }

        public void OnAuthenticationChallenge(AuthenticationChallengeContext filterContext)
        {
            if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
            {
                filterContext.Result = new RedirectToRouteResult(
                 new RouteValueDictionary
                 {
                     { "controller", "Account" },
                     { "action", "Login" }
                 });
            }
        }
    }
}