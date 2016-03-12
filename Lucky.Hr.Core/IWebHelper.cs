// =================================================================== 
// 项目说明
//====================================================================
// 幸运草工作室 @ CopyRight 2014-2020
// 文件： IWebHelper.cs
// 项目名称： 
// 创建时间：2014/6/4
// 创建人：丁富升
// ===================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Lucky.Hr.Core
{
    /// <summary>
    /// 常用帮助类
    /// </summary>
    public partial interface IWebHelper
    {
        /// <summary>
        /// 获取referrer网址
        /// </summary>
        /// <returns>referrer网址</returns>
        string GetUrlReferrer();

        /// <summary>
        /// 获取上下文的IP地址
        /// </summary>
        /// <returns>上下文的IP地址</returns>
        string GetCurrentIpAddress();

        /// <summary>
        /// 获取页面地址
        /// </summary>
        /// <param name="includeQueryString">是否包含查询字符串</param>
        /// <returns>获取页面地址</returns>
        string GetThisPageUrl(bool includeQueryString);

        /// <summary>
        /// 获取页面地址
        /// </summary>
        /// <param name="includeQueryString">是否包含查询字符串</param>
        /// <param name="useSsl">是否使用Ssl</param>
        /// <returns>Page name</returns>
        string GetThisPageUrl(bool includeQueryString, bool useSsl);

        /// <summary>
        /// 获取一个值，指示当前连接是否安全
        /// </summary>
        /// <returns>true - secured, false - not secured</returns>
        bool IsCurrentConnectionSecured();

        /// <summary>
        /// 通过名字获取服务器变量
        /// </summary>
        /// <param name="name">变量名</param>
        /// <returns>服务器变量</returns>
        string ServerVariables(string name);

        /// <summary>
        /// 获取商店主机地址
        /// </summary>
        /// <param name="useSsl">使用SSL</param>
        /// <returns>商店主机地址</returns>
        string GetStoreHost(bool useSsl);

        /// <summary>
        /// 商店主机地址
        /// </summary>
        /// <returns>商店主机地址</returns>
        string GetStoreLocation();

        /// <summary>
        /// 商店主机地址
        /// </summary>
        /// <param name="useSsl">使用 SSL</param>
        /// <returns>商店主机地址</returns>
        string GetStoreLocation(bool useSsl);

        /// <summary>
        /// 判断是否静态资源
        /// </summary>
        /// <param name="request">HTTP 请求</param>
        /// <returns>是否静态资源</returns>
        /// <remarks>
        /// These are the file extensions considered to be static resources:
        /// .css
        ///	.gif
        /// .png 
        /// .jpg
        /// .jpeg
        /// .js
        /// .axd
        /// .ashx
        /// </remarks>
        bool IsStaticResource(HttpRequest request);

        /// <summary>
        /// 映射一个虚拟路径到物理磁盘路径
        /// </summary>
        /// <param name="path">映射路径。例如 "~/bin"</param>
        /// <returns>物理磁盘路径。例如 "c:\inetpub\wwwroot\bin"</returns>
        string MapPath(string path);


        /// <summary>
        /// 修改查询字符串
        /// </summary>
        /// <param name="url">网址修改</param>
        /// <param name="queryStringModification">查询字符串修改</param>
        /// <param name="anchor">Anchor</param>
        /// <returns>New url</returns>
        string ModifyQueryString(string url, string queryStringModification, string anchor);

        /// <summary>
        /// 从URL中移除的查询字符串
        /// </summary>
        /// <param name="url">网址修改</param>
        /// <param name="queryString">查询字符串移除</param>
        /// <returns>New url</returns>
        string RemoveQueryString(string url, string queryString);

        /// <summary>
        /// 按名称获取查询字符串值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">Parameter name</param>
        /// <returns>Query string value</returns>
        T QueryString<T>(string name);

        /// <summary>
        /// 重新启动应用程序域
        /// </summary>
        /// <param name="makeRedirect">A value indicating whether </param>
        /// <param name="redirectUrl">Redirect URL; empty string if you want to redirect to the current page URL</param>
        void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "");

        /// <summary>
        /// 获取一个值，指示由搜索引擎的请求是否是由（网络爬虫）
        /// </summary>
        /// <param name="context">HTTP context</param>
        /// <returns>Result</returns>
        bool IsSearchEngine(HttpContextBase context);

        /// <summary>
        /// 获取一个值，该值指示客户端是否被重定向到一个新的位置
        /// </summary>
        bool IsRequestBeingRedirected { get; }

        /// <summary>
        /// 获取或设置一个值，该值指示是否在客户端使用POST重定向到一个新的位置
        /// </summary>
        bool IsPostBeingDone { get; set; }
    }
}
