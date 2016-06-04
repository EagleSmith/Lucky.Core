using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lucky.Hr.SiteManager
{
    public class SiteManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SiteManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SiteManager_default",
                "SiteManager/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", area = "SiteManager", id = UrlParameter.Optional },
                new[] { "Lucky.Hr.SiteManager.*" }
            );
        }
    }
}