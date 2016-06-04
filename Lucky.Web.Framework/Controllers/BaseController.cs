using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Lucky.Core.Cache;
using Lucky.Core.Infrastructure;
using Lucky.Core.Logging;

namespace Lucky.Web.Framework.Controllers
{
    public abstract class BaseController:Controller
    {
        public ICacheManager RedisCacheManager
        {
            get { return EngineContext.Current.ContainerManager.Resolve<ICacheManager>("RedisCacheManager"); }
        }

        /// <summary>
        /// 呈现局部视图
        /// </summary>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString()
        {
            return RenderPartialViewToString(null, null);
        }
        /// <summary>
        /// 呈现局部视图串
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName)
        {
            return RenderPartialViewToString(viewName, null);
        }
        /// <summary>
        /// 呈现局部视图串
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(object model)
        {
            return RenderPartialViewToString(null, model);
        }
        /// <summary>
        /// Log exception
        /// </summary>
        /// <param name="exc">Exception</param>
        protected void LogException(Exception exc)
        {
            //var workContext = EngineContext.Current.Resolve<IWorkContext>();
            //var logger = EngineContext.Current.Resolve<ILogger>();

            //var customer = workContext.CurrentCustomer;
            //logger.Error(exc.Message, exc, customer);
        }
        /// <summary>
        /// 呈现局部视图串
        /// </summary>
        /// <param name="viewName">视图名称</param>
        /// <param name="model">模型</param>
        /// <returns>Result</returns>
        public virtual string RenderPartialViewToString(string viewName, object model)
        {
            //原来的源代码: 
            if (string.IsNullOrEmpty(viewName))
                viewName = this.ControllerContext.RouteData.GetRequiredString("action");

            this.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindPartialView(this.ControllerContext, viewName);
                var viewContext = new ViewContext(this.ControllerContext, viewResult.View, this.ViewData, this.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
