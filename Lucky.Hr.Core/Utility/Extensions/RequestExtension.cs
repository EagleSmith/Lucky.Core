using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
