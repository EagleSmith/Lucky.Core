using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lucky.Hr.ViewModels
{
    public class ViewModelsBase : IViewModelsBase
    {
        /// <summary>
        /// 默认初始值
        /// </summary>
        /// <value>The GUID default.</value>
        [ScaffoldColumn(false)]
        public Guid GuidDefault
        {
            get { return new Guid("00000000-0000-0000-0000-000000000000"); }
        }

        /// <summary>
        /// Gets the app id default value.
        /// </summary>
        /// <value>The app id default value.</value>
        [ScaffoldColumn(false)]
        public Guid AppIdDefaultValue
        {
            get
            {
                return new Guid("294E7791-5756-4B6C-BABC-A9228F02331D");
            }
        }
        /// <summary>
        /// 根据获得URL   controller
        /// </summary>
        [ScaffoldColumn(false)]
        public string UrlController
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();
            }
        }

        /// <summary>
        /// 根据获得URL action
        /// </summary>
        [ScaffoldColumn(false)]
        public string UrlAction
        {
            get
            {
                return HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
            }
        }
        [ScaffoldColumn(false)]
        public Guid AppID
        {
            get
            {
                try
                {
                    return new Guid(HttpContext.Current.Request.RequestContext.RouteData.Values["appid"].ToString());
                }
                catch (Exception)
                {
                    return new Guid("294E7791-5756-4B6C-BABC-A9228F02331D");
                }
            }
        }
        [ScaffoldColumn(false)]
        public object KeyID
        {
            get
            {
                try
                {
                    if (HttpContext.Current.Request.RequestContext.RouteData.Values.ContainsKey("id"))
                    {
                        return HttpContext.Current.Request.RequestContext.RouteData.Values["id"];
                    }
                    return null;
                }
                catch (Exception)
                {
                    return new Guid("a536b4f5-093a-41b9-8fd1-3bbf936c165b");
                }
            }
        }
        // /// <summary>
        // /// 取得模块编号
        // /// </summary>
        // /// <value>The module id value.</value>
        //[ScaffoldColumn(false)]
        // public Guid Module_Id
        // {
        //     get
        //     {
        //         string t = System.Web.HttpContext.Current.Request.RequestValue<string>("ModuleID");
        //         if (string.IsNullOrEmpty(t))
        //         {
        //             return GuidDefault;
        //         }
        //         return new Guid(t);
        //     }
        // }
        /// <summary>
        ///     执行客户端JS
        /// </summary>
        [ScaffoldColumn(false)]
        public bool ExeJavaScript { get; set; }
    }
}
