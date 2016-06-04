using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Lucky.Web.Framework.Controllers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorizeAttribute :AuthorizeAttribute
    {
        private readonly bool _dontValidate;


        public AdminAuthorizeAttribute()
            : this(false)
        {
        }

        public AdminAuthorizeAttribute(bool dontValidate)
        {
            _dontValidate = dontValidate;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            RouteData route = RouteTable.Routes.GetRouteData(httpContext);
           // string action = route.Values["action"].ToString();
            if (_dontValidate)
            {
                return true;
            }
            if (httpContext.Request.IsAuthenticated)
            {
                if (route != null)
                {
                    string urlController = route.Values["controller"].ToString();
                    var item=httpContext.GetOwinContext().Authentication.User.Claims;
                    
                    switch (urlController)
                    {
                        case "Roles":
                            return false;

                    }
                }
                return true;
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}
