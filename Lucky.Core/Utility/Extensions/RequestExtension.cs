using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Lucky.Core.Utility.Extensions
{
    public static class RequestExtension
    {
        #region "获取用户Form提交字段值"
        /// <summary>
        /// 获取post和get提交值
        /// </summary>
        /// <param name="ValueName">字段名</param>
        /// <returns></returns>
        public static T RequestValue<T>(this HttpRequest request, string ValueName)
        {
            T TempValue;

            if (request.QueryString[ValueName] != null)
            {
                TempValue = (T)Convert.ChangeType(request.QueryString[ValueName], typeof(T));
            }
            else
            {
                TempValue = default(T);
            }

            return TempValue;
        }
        #endregion

        public static string RelativeFromAbsolutePath(this HttpContextBase context, string path)
        {
            var request = context.Request;
            var applicationPath = request.PhysicalApplicationPath;
            var virtualDir = request.ApplicationPath;
            virtualDir = virtualDir == "/" ? virtualDir : (virtualDir + "/");
            return path.Replace(applicationPath, virtualDir).Replace(@"\", "/");
        }
        /// <summary>
        /// 在同步上下文中查找当前会话<see cref="System.Web.HttpContext" />对象
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HttpContext FindHttpContext(this SynchronizationContext context)
        {
            if (context == null)
            {
                return null;
            }
            var factory = GetFindApplicationDelegate(context);
            if (factory == null)
            {
                return null;
            }
            return factory(context).Context;
        }
        /// <summary>
        /// 确定异步状态的上下文可用
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static HttpContext Check(this HttpContext context)
        {
            if (context == null)
            {
                context = SynchronizationContext.Current.FindHttpContext();
            }
            return context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Func<System.Threading.SynchronizationContext, System.Web.HttpApplication> GetFindApplicationDelegate(SynchronizationContext context)
        {
            Func<System.Threading.SynchronizationContext, System.Web.HttpApplication> factory = null;
            Type type = context.GetType();
            if (!type.FullName.Equals("System.Web.LegacyAspNetSynchronizationContext"))
            {
                return null;
            }
            ParameterExpression sourceExpression = Expression.Parameter(typeof(System.Threading.SynchronizationContext), "context");
            Expression sourceInstance = Expression.Convert(sourceExpression, type);
            FieldInfo applicationFieldInfo = type.GetField("_application", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            Expression fieldExpression = Expression.Field(sourceInstance, applicationFieldInfo);
            factory = Expression.Lambda<Func<System.Threading.SynchronizationContext, System.Web.HttpApplication>>(fieldExpression, sourceExpression).Compile();
            return factory;
        }
    }
}
