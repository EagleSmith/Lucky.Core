using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using Lucky.Hr.Core.Infrastructure;

namespace Lucky.Hr.Core
{
    /// <summary>
    /// web帮助类
    /// </summary>
    public partial class WebHelper : IWebHelper
    {
        private readonly HttpContextBase _httpContext;

        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="httpContext">HTTP 上下文</param>
        public WebHelper(HttpContextBase httpContext)
        {
            this._httpContext = httpContext;
        }

        /// <summary>
        /// 获取referrer网址
        /// </summary>
        /// <returns>referrer网址</returns>
        public virtual string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;

            //有些情况下referrer网址为空 (例如, 有 IE 8)
            if (_httpContext != null &&
                _httpContext.Request != null &&
                _httpContext.Request.UrlReferrer != null)
                referrerUrl = _httpContext.Request.UrlReferrer.PathAndQuery;

            return referrerUrl;
        }

        /// <summary>
        /// 获取上下文的IP地址
        /// </summary>
        /// <returns>上下文的IP地址</returns>
        public virtual string GetCurrentIpAddress()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return string.Empty;

            var result = "";
            if (_httpContext.Request.Headers != null)
            {
                //查找 X-Forwarded-For (XFF) HTTP消息头字段
                //它是用于识别一个客户端通过HTTP代理服务器或负载平衡器连接到Web服务器的起始IP地址
                string xff = _httpContext.Request.Headers.AllKeys
                    .Where(x => "X-FORWARDED-FOR".Equals(x, StringComparison.InvariantCultureIgnoreCase))
                    .Select(k => _httpContext.Request.Headers[k])
                    .FirstOrDefault();

                //如果要排除私有IP地址, 再看看 http://stackoverflow.com/questions/2577496/how-can-i-get-the-clients-ip-address-in-asp-net-mvc

                if (!String.IsNullOrEmpty(xff))
                {
                    string lastIp = xff.Split(new char[] { ',' }).FirstOrDefault();
                    result = lastIp;
                }
            }

            if (String.IsNullOrEmpty(result) && _httpContext.Request.UserHostAddress != null)
            {
                result = _httpContext.Request.UserHostAddress;
            }

            //一些验证
            if (result == "::1")
                result = "127.0.0.1";
            //删除端口
            if (!String.IsNullOrEmpty(result))
            {
                int index = result.IndexOf(":", StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                    result = result.Substring(0, index);
            }
            return result;

        }

        ///// <summary>
        ///// 获取页面地址
        ///// </summary>
        ///// <param name="includeQueryString">是否包含查询字符串</param>
        ///// <returns>获取页面地址</returns>
        //public virtual string GetThisPageUrl(bool includeQueryString)
        //{
        //    bool useSsl = IsCurrentConnectionSecured();
        //    return GetThisPageUrl(includeQueryString, useSsl);
        //}

        ///// <summary>
        ///// 获取页面地址
        ///// </summary>
        ///// <param name="includeQueryString">是否包含查询字符串</param>
        ///// <param name="useSsl">是否使用Ssl</param>
        ///// <returns>Page name</returns>
        //public virtual string GetThisPageUrl(bool includeQueryString, bool useSsl)
        //{
        //    string url = string.Empty;
        //    if (_httpContext == null || _httpContext.Request == null)
        //        return url;

        //    if (includeQueryString)
        //    {
        //        string storeHost = GetStoreHost(useSsl);
        //        if (storeHost.EndsWith("/"))
        //            storeHost = storeHost.Substring(0, storeHost.Length - 1);
        //        url = storeHost + _httpContext.Request.RawUrl;
        //    }
        //    else
        //    {
        //        if (_httpContext.Request.Url != null)
        //        {
        //            url = _httpContext.Request.Url.GetLeftPart(UriPartial.Path);
        //        }
        //    }
        //    url = url.ToLowerInvariant();
        //    return url;
        //}

        /// <summary>
        /// 获取一个值，指示当前连接是否安全
        /// </summary>
        /// <returns>true - secured, false - not secured</returns>
        public virtual bool IsCurrentConnectionSecured()
        {
            bool useSsl = false;
            if (_httpContext != null && _httpContext.Request != null)
            {
                useSsl = _httpContext.Request.IsSecureConnection;
                //当你的主机使用了负载均衡的服务器上，那么Request.IsSecureConnection永远不会得到设置为true，则使用下面的语句
                //只是取消注释
                //useSSL = _httpContext.Request.ServerVariables["HTTP_CLUSTER_HTTPS"] == "on" ? true : false;
            }

            return useSsl;
        }

        /// <summary>
        /// 通过名字获取服务器变量
        /// </summary>
        /// <param name="name">变量名</param>
        /// <returns>服务器变量</returns>
        public virtual string ServerVariables(string name)
        {
            string result = string.Empty;

            try
            {
                if (_httpContext == null || _httpContext.Request == null)
                    return result;

                //把这个方法放到try-catch
                //如这里所描述 http://www.nopcommerce.com/boards/t/21356/multi-store-roadmap-lets-discuss-update-done.aspx?p=6#90196
                if (_httpContext.Request.ServerVariables[name] != null)
                {
                    result = _httpContext.Request.ServerVariables[name];
                }
            }
            catch
            {
                result = string.Empty;
            }
            return result;
        }

        ///// <summary>
        ///// 获取商店主机地址
        ///// </summary>
        ///// <param name="useSsl">使用SSL</param>
        ///// <returns>商店主机地址</returns>
        //public virtual string GetStoreHost(bool useSsl)
        //{
        //    var result = "";
        //    var httpHost = ServerVariables("HTTP_HOST");
        //    if (!String.IsNullOrEmpty(httpHost))
        //    {
        //        result = "http://" + httpHost;
        //        if (!result.EndsWith("/"))
        //            result += "/";
        //    }

        //    if (DataSettingsHelper.DatabaseIsInstalled())
        //    {
        //        #region 数据库已安装

        //        //让我们在这里解析IWorkContext
        //        //不通过构造器注入，因为这会导致循环引用
        //        var storeContext = EngineContext.Current.Resolve<IStoreContext>();
        //        var currentStore = storeContext.CurrentStore;
        //        if (currentStore == null)
        //            throw new Exception("Current store cannot be loaded");

        //        if (String.IsNullOrWhiteSpace(httpHost))
        //        {
        //            //HTTP_HOST 变量不可用
        //            //这种情况是可能的，当HttpContext不可用时(例如，在一个计划任务运行)
        //            //在这种情况下，使用配置管理领域一个实体店的网址
        //            result = currentStore.Url;
        //            if (!result.EndsWith("/"))
        //                result += "/";
        //        }

        //        if (useSsl)
        //        {
        //            if (!String.IsNullOrWhiteSpace(currentStore.SecureUrl))
        //            {
        //                //指定安全的URL
        //                //因为店老板不希望它被自动检测到
        //                //在这种情况下，让我们使用指定的安全网址
        //                result = currentStore.SecureUrl;
        //            }
        //            else
        //            {
        //                //未指定安全的URL
        //                //因为店老板希望它能够自动检测到
        //                result = result.Replace("http:/", "https:/");
        //            }
        //        }
        //        else
        //        {
        //            if (currentStore.SslEnabled && !String.IsNullOrWhiteSpace(currentStore.SecureUrl))
        //            {
        //                //SSL是在这家店，并在指定安全的URL启用
        //                //因为店老板不希望它被自动检测到
        //                //在这种情况下，让我们使用指定的安全网址
        //                result = currentStore.Url;
        //            }
        //        }
        //        #endregion
        //    }
        //    else
        //    {
        //        #region 数据库没有安装
        //        if (useSsl)
        //        {
        //            //未指定安全的URL
        //            //因为店老板希望它能够自动检测到
        //            result = result.Replace("http:/", "https:/");
        //        }
        //        #endregion
        //    }


        //    if (!result.EndsWith("/"))
        //        result += "/";
        //    return result.ToLowerInvariant();
        //}

        ///// <summary>
        ///// 商店主机地址
        ///// </summary>
        ///// <returns>商店主机地址</returns>
        //public virtual string GetStoreLocation()
        //{
        //    bool useSsl = IsCurrentConnectionSecured();
        //    return GetStoreLocation(useSsl);
        //}

        ///// <summary>
        ///// 商店主机地址
        ///// </summary>
        ///// <param name="useSsl">使用 SSL</param>
        ///// <returns>商店主机地址</returns>
        //public virtual string GetStoreLocation(bool useSsl)
        //{
        //    //return HostingEnvironment.ApplicationVirtualPath;

        //    string result = GetStoreHost(useSsl);
        //    if (result.EndsWith("/"))
        //        result = result.Substring(0, result.Length - 1);
        //    if (_httpContext != null && _httpContext.Request != null)
        //        result = result + _httpContext.Request.ApplicationPath;
        //    if (!result.EndsWith("/"))
        //        result += "/";

        //    return result.ToLowerInvariant();
        //}

        /// <summary>
        /// 判断是否静态资源
        /// </summary>
        /// <param name="request">HTTP 请求</param>
        /// <returns>是否静态资源</returns>
        /// <remarks>
        /// 这些都被认为是静态资源的文件扩展名：
        /// .css
        ///	.gif
        /// .png 
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        public virtual bool IsStaticResource(HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            string path = request.Path;
            string extension = VirtualPathUtility.GetExtension(path);

            if (extension == null) return false;

            switch (extension.ToLower())
            {
                case ".axd":
                case ".ashx":
                case ".bmp":
                case ".css":
                case ".gif":
                case ".htm":
                case ".html":
                case ".ico":
                case ".jpeg":
                case ".jpg":
                case ".js":
                case ".png":
                case ".rar":
                case ".zip":
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 映射一个虚拟路径到物理磁盘路径
        /// </summary>
        /// <param name="path">映射路径。例如 "~/bin"</param>
        /// <returns>物理磁盘路径。例如 "c:\inetpub\wwwroot\bin"</returns>
        public virtual string MapPath(string path)
        {
            if (HostingEnvironment.IsHosted)
            {
                //托管
                return HostingEnvironment.MapPath(path);
            }
            else
            {
                //未托管。例如，在运行单元测试
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
                return Path.Combine(baseDirectory, path);
            }
        }

        /// <summary>
        /// 修改查询字符串
        /// </summary>
        /// <param name="url">网址修改</param>
        /// <param name="queryStringModification">查询字符串修改</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>New url</returns>
        public virtual string ModifyQueryString(string url, string queryStringModification, string anchor)
        {
            if (url == null)
                url = string.Empty;
            url = url.ToLowerInvariant();

            if (queryStringModification == null)
                queryStringModification = string.Empty;
            queryStringModification = queryStringModification.ToLowerInvariant();

            if (anchor == null)
                anchor = string.Empty;
            anchor = anchor.ToLowerInvariant();


            string str = string.Empty;
            string str2 = string.Empty;
            if (url.Contains("#"))
            {
                str2 = url.Substring(url.IndexOf("#") + 1);
                url = url.Substring(0, url.IndexOf("#"));
            }
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryStringModification))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    foreach (string str4 in queryStringModification.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str4))
                        {
                            string[] strArray2 = str4.Split(new char[] { '=' });
                            if (strArray2.Length == 2)
                            {
                                dictionary[strArray2[0]] = strArray2[1];
                            }
                            else
                            {
                                dictionary[str4] = null;
                            }
                        }
                    }
                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
                else
                {
                    str = queryStringModification;
                }
            }
            if (!string.IsNullOrEmpty(anchor))
            {
                str2 = anchor;
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)) + (string.IsNullOrEmpty(str2) ? "" : ("#" + str2))).ToLowerInvariant();
        }

        /// <summary>
        /// 从URL中移除的查询字符串
        /// </summary>
        /// <param name="url">网址修改</param>
        /// <param name="queryString">查询字符串移除</param>
        /// <returns>New url</returns>
        public virtual string RemoveQueryString(string url, string queryString)
        {
            if (url == null)
                url = string.Empty;
            url = url.ToLowerInvariant();

            if (queryString == null)
                queryString = string.Empty;
            queryString = queryString.ToLowerInvariant();


            string str = string.Empty;
            if (url.Contains("?"))
            {
                str = url.Substring(url.IndexOf("?") + 1);
                url = url.Substring(0, url.IndexOf("?"));
            }
            if (!string.IsNullOrEmpty(queryString))
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var dictionary = new Dictionary<string, string>();
                    foreach (string str3 in str.Split(new char[] { '&' }))
                    {
                        if (!string.IsNullOrEmpty(str3))
                        {
                            string[] strArray = str3.Split(new char[] { '=' });
                            if (strArray.Length == 2)
                            {
                                dictionary[strArray[0]] = strArray[1];
                            }
                            else
                            {
                                dictionary[str3] = null;
                            }
                        }
                    }
                    dictionary.Remove(queryString);

                    var builder = new StringBuilder();
                    foreach (string str5 in dictionary.Keys)
                    {
                        if (builder.Length > 0)
                        {
                            builder.Append("&");
                        }
                        builder.Append(str5);
                        if (dictionary[str5] != null)
                        {
                            builder.Append("=");
                            builder.Append(dictionary[str5]);
                        }
                    }
                    str = builder.ToString();
                }
            }
            return (url + (string.IsNullOrEmpty(str) ? "" : ("?" + str)));
        }

        ///// <summary>
        ///// 按名称获取查询字符串值
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="name">参数名称</param>
        ///// <returns>查询字符串值</returns>
        //public virtual T QueryString<T>(string name)
        //{
        //    string queryParam = null;
        //    if (_httpContext != null && _httpContext.Request.QueryString[name] != null)
        //        queryParam = _httpContext.Request.QueryString[name];

        //    if (!String.IsNullOrEmpty(queryParam))
        //        return CommonHelper.To<T>(queryParam);

        //    return default(T);
        //}

        ///// <summary>
        ///// 重新启动应用程序域
        ///// </summary>
        ///// <param name="makeRedirect">该值表示是否 </param>
        ///// <param name="redirectUrl">重定向URL;空字符串，如果你想重定向到当前页面的URL</param>
        //public virtual void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "")
        //{
        //    if (CommonHelper.GetTrustLevel() > AspNetHostingPermissionLevel.Medium)
        //    {
        //        //完全信任
        //        HttpRuntime.UnloadAppDomain();

        //        TryWriteGlobalAsax();
        //    }
        //    else
        //    {
        //        //中等信任
        //        bool success = TryWriteWebConfig();
        //        if (!success)
        //        {
        //            throw new Lucky.HrException("nopCommerce needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
        //                "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
        //                "- run the application in a full trust environment, or" + Environment.NewLine +
        //                "- give the application write access to the 'web.config' file.");
        //        }

        //        success = TryWriteGlobalAsax();
        //        if (!success)
        //        {
        //            throw new Lucky.HrException("nopCommerce needs to be restarted due to a configuration change, but was unable to do so." + Environment.NewLine +
        //                "To prevent this issue in the future, a change to the web server configuration is required:" + Environment.NewLine +
        //                "- run the application in a full trust environment, or" + Environment.NewLine +
        //                "- give the application write access to the 'Global.asax' file.");
        //        }
        //    }

        //    // 如果设置的扩展/模块需要一个应用程序域重新启动，这是不太可能的
        //    // 当前请求能够被正确处理。所以，我们重定向到相同的URL
        //    // 从而使新的请求将来到新开工的应用程序域
        //    if (_httpContext != null && makeRedirect)
        //    {
        //        if (String.IsNullOrEmpty(redirectUrl))
        //            redirectUrl = GetThisPageUrl(true);
        //        _httpContext.Response.Redirect(redirectUrl, true /*endResponse*/);
        //    }
        //}

        private bool TryWriteWebConfig()
        {
            try
            {
                // 在中等信任，“UnloadAppDomain”不支持。设置web.config
                // 迫使一个应用程序域重新启动
                File.SetLastWriteTimeUtc(MapPath("~/web.config"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool TryWriteGlobalAsax()
        {
            try
            {
                //当一个新的插件被丢弃在Plugins文件夹，并安装到nopCommerce
                //即使插件已经注册了其控制器的路由
                //这些路由将不会作为工作的MVC框架
                //寻找新的控制器类型，不能实例化所要求的控制器
                //这就是为什么你得到这些讨厌的错误
                //即“控制器不实现一个IController”
                //这个问题是这里描述: http://www.nopcommerce.com/boards/t/10969/nop-20-plugin.aspx?p=4#51318
                //解决的办法是设置Global.asax文件
                File.SetLastWriteTimeUtc(MapPath("~/global.asax"), DateTime.UtcNow);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 获取一个值，指示由搜索引擎的请求是否是由（网络爬虫）
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>Result</returns>
        public virtual bool IsSearchEngine(HttpContextBase context)
        {
            //我们接受，而不是HttpRequest的HttpContext的，并把所需的逻辑放在try-catch块
            //更多信息: http://www.nopcommerce.com/boards/t/17711/unhandled-exception-request-is-not-available-in-this-context.aspx
            if (context == null)
                return false;

            bool result = false;
            try
            {
                result = context.Request.Browser.Crawler;
                if (!result)
                {
                    //把任何额外的称为爬虫在下面的正则表达式的一些自定义验证
                    //var regEx = new Regex("Twiceler|twiceler|BaiDuSpider|baduspider|Slurp|slurp|ask|Ask|Teoma|teoma|Yahoo|yahoo");
                    //result = regEx.Match(request.UserAgent).Success;
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }
            return result;
        }

        /// <summary>
        /// 获取一个值，该值指示客户端是否被重定向到一个新的位置
        /// </summary>
        public virtual bool IsRequestBeingRedirected
        {
            get
            {
                var response = _httpContext.Response;
                return response.IsRequestBeingRedirected;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否在客户端使用POST重定向到一个新的位置
        /// </summary>
        public virtual bool IsPostBeingDone
        {
            get
            {
                if (_httpContext.Items["nop.IsPOSTBeingDone"] == null)
                    return false;
                return Convert.ToBoolean(_httpContext.Items["nop.IsPOSTBeingDone"]);
            }
            set
            {
                _httpContext.Items["nop.IsPOSTBeingDone"] = value;
            }
        }

        string IWebHelper.GetUrlReferrer()
        {
            throw new NotImplementedException();
        }

        string IWebHelper.GetCurrentIpAddress()
        {
            throw new NotImplementedException();
        }

        string IWebHelper.GetThisPageUrl(bool includeQueryString)
        {
            throw new NotImplementedException();
        }

        string IWebHelper.GetThisPageUrl(bool includeQueryString, bool useSsl)
        {
            throw new NotImplementedException();
        }

        bool IWebHelper.IsCurrentConnectionSecured()
        {
            throw new NotImplementedException();
        }

        string IWebHelper.ServerVariables(string name)
        {
            throw new NotImplementedException();
        }

        string IWebHelper.GetStoreHost(bool useSsl)
        {
            throw new NotImplementedException();
        }

        string IWebHelper.GetStoreLocation()
        {
            throw new NotImplementedException();
        }

        string IWebHelper.GetStoreLocation(bool useSsl)
        {
            throw new NotImplementedException();
        }

        bool IWebHelper.IsStaticResource(HttpRequest request)
        {
            throw new NotImplementedException();
        }

        string IWebHelper.MapPath(string path)
        {
            throw new NotImplementedException();
        }

        string IWebHelper.ModifyQueryString(string url, string queryStringModification, string anchor)
        {
            throw new NotImplementedException();
        }

        string IWebHelper.RemoveQueryString(string url, string queryString)
        {
            throw new NotImplementedException();
        }

        T IWebHelper.QueryString<T>(string name)
        {
            throw new NotImplementedException();
        }

        void IWebHelper.RestartAppDomain(bool makeRedirect, string redirectUrl)
        {
            throw new NotImplementedException();
        }

        bool IWebHelper.IsSearchEngine(HttpContextBase context)
        {
            throw new NotImplementedException();
        }

        bool IWebHelper.IsRequestBeingRedirected
        {
            get { throw new NotImplementedException(); }
        }

        bool IWebHelper.IsPostBeingDone
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
