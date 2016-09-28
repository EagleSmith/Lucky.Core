using Lucky.Core.Infrastructure;
using Lucky.Core.Logging;
using System.Web;
using System.Web.Mvc;

namespace Lucky.Hr.WebSite
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LoggingFilterAttribute(EngineContext.Current.Resolve<ILogger>()));//日志处理

            filters.Add(new HandleErrorAttribute());
        }
    }
}
