using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Autofac;

namespace Lucky.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// 一个 <see cref="IHttpModule"/> 和 <see cref="ILifetimeScopeProvider"/> 实现 
    /// 创建整个HTTP请求生命周期的模块
    /// </summary>
    public class AutofacRequestLifetimeHttpModule : IHttpModule
    {
        /// <summary>
        /// 标签用于标识被限定在HTTP请求级别的注册
        /// </summary>
        //在Autofac(用于MVC3)以前的版本中，它被设置为“HttpRequest”
        public static readonly object HttpRequestTag = "AutofacWebRequest";

        /// <summary>
        /// 初始化模块，并准备处理请求
        /// </summary>
        /// <param name="context">一个 <see cref="T:System.Web.HttpApplication"/> 访问提供
        /// 方法，属性和常见的ASP.NET应用程序中的所有应用程序对象的事件</param>
        public void Init(HttpApplication context)
        {
            context.EndRequest += ContextEndRequest;
        }

        /// <summary>
        /// 获取一个生命周期作用域
        /// </summary>
        /// <param name="container">父容器</param>
        /// <param name="configurationAction">行为 <see cref="ContainerBuilder"/> </param>
        /// <returns>新的或现有的生命周期作用域</returns>
        public static ILifetimeScope GetLifetimeScope(ILifetimeScope container, Action<ContainerBuilder> configurationAction)
        {
            //这里的HttpContext有时候不可用
            if (HttpContext.Current != null)
            {
                return LifetimeScope ?? (LifetimeScope = InitializeLifetimeScope(configurationAction, container));
            }
            else
            {
                //throw new InvalidOperationException("HttpContextNotAvailable");
                return InitializeLifetimeScope(configurationAction, container);
            }
        }

        /// <summary>
        /// 用于实现<see cref="T:System.Web.IHttpModule"/>模块的资源（内存除外）的处置
        /// </summary>
        public void Dispose()
        {
        }

        static ILifetimeScope LifetimeScope
        {
            get
            {
                return (ILifetimeScope)HttpContext.Current.Items[typeof(ILifetimeScope)];
            }
            set
            {
                HttpContext.Current.Items[typeof(ILifetimeScope)] = value;
            }
        }

        public static void ContextEndRequest(object sender, EventArgs e)
        {
            ILifetimeScope lifetimeScope = LifetimeScope;
            if (lifetimeScope != null)
                lifetimeScope.Dispose();
        }

        static ILifetimeScope InitializeLifetimeScope(Action<ContainerBuilder> configurationAction, ILifetimeScope container)
        {
            return (configurationAction == null)
                ? container.BeginLifetimeScope(HttpRequestTag)
                : container.BeginLifetimeScope(HttpRequestTag, configurationAction);
        }
    }
}
