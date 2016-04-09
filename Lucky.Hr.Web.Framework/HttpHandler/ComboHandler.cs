using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Routing;
using Lucky.Core.Utility;

namespace Lucky.Hr.Web.Framework.HttpHandler
{
    public class ComboRouteHandler : IRouteHandler
    {
        public IHttpHandler GetHttpHandler(RequestContext requestContext)
        {
            return new ComboHandler();
        }
    }
    public class ComboHandler : IHttpHandler
    {
        private string _encoding, _hash;
        private readonly List<string> _fileNames = new List<string>();

        public void ProcessRequest(HttpContext context)
        {
            var url = context.Request.Url.AbsoluteUri.ToLower();
            if (url.IndexOf(".js", StringComparison.Ordinal) > 0 || url.IndexOf(".css", StringComparison.Ordinal) > 0)
            {
                var cache = url;
                if (context.Request.UrlReferrer != null)
                {
                    var urlReferrer = context.Request.UrlReferrer.AbsoluteUri;
                    var debug = false;
                    if (urlReferrer.IndexOf("debug", StringComparison.Ordinal) > 0)
                    {
                        debug = true;
                        cache += "?debug";
                    }
                    var sb = new StringBuilder();
                    _encoding = Globals.SetEncoding(context);
                    var contentType = url.IndexOf(".js", StringComparison.Ordinal) > 0 ? "application/x-javascript" : "text/css";
                    _hash = Globals.GetMd5Sum(cache);
                    if (context.Cache[cache] == null)
                    {
                        var baseUri = new Uri(url);
                        var tempUrl = url;
                        var urlArray = Regex.Split(url, @"\?\?");
                        if (urlArray.Length > 1)
                        {
                            tempUrl = urlArray[1];
                        }
                        var tempFiles = Regex.Split(tempUrl, ",");
                        foreach (var file in tempFiles)
                        {
                            if (debug && file.IndexOf("seajs-combo") > 0) continue;
                            var tempFile = file;
                            if (debug)
                            {
                                tempFile = tempFile.IndexOf("-debug.js", StringComparison.Ordinal) > 0 || tempFile.IndexOf("jquery.js", StringComparison.Ordinal) > 0 ? tempFile : tempFile.Replace(".js", "-debug.js");
                            }
                            var fileStr = Globals.GetLocalFile(new Uri(baseUri, tempFile), context, _fileNames);
                            sb.AppendLine(fileStr);
                        }
                        if (_fileNames.Count > 0)
                        {
                            context.Cache.Insert(cache, sb.ToString(), new CacheDependency(_fileNames.ToArray()));
                        }
                        else
                        {
                            context.Cache.Insert(cache, sb.ToString(), null, Cache.NoAbsoluteExpiration, new TimeSpan(3, 0, 0, 0));
                        }
                    }
                    else if (Globals.IsCachedOnBrowser(context, _hash, contentType))
                    {
                        return;
                    }
                    context.Response.ClearHeaders();
                    context.Response.AppendHeader("Vary", "Accept-Encoding");
                    context.Response.AppendHeader("Cache-Control", "max-age=604800");
                    context.Response.AppendHeader("Expires", DateTime.Now.AddYears(1).ToString("R"));
                    context.Response.AppendHeader("ETag", _hash);
                    context.Response.Write(context.Cache[cache]);
                    context.Response.ContentType = contentType;
                }
                if (_encoding == "none") return;
                if (_encoding == "gzip")
                {
                    context.Response.Filter = new GZipStream(context.Response.Filter, CompressionMode.Compress);
                }
                else
                {
                    context.Response.Filter = new DeflateStream(context.Response.Filter, CompressionMode.Compress);
                }
                context.Response.AppendHeader("Content-encoding", _encoding);
            }
            else
            {
                var cache = context.Request.Url.AbsoluteUri;
                var file = context.Server.MapPath(context.Request.Path);
                var s = Path.GetExtension(file);
                if (s == null) return;
                var extension = s.ToLower().Remove(0, 1);
                _encoding = Globals.SetEncoding(context);
                _hash = Globals.GetMd5Sum(cache);
                if (Globals.IsCachedOnBrowser(context, _hash, "image/" + extension)) return;
                context.Response.AppendHeader("Vary", "Accept-Encoding");
                context.Response.AppendHeader("Cache-Control", "max-age=604800");
                context.Response.AppendHeader("Expires", DateTime.Now.AddYears(1).ToString("R"));
                context.Response.AppendHeader("ETag", _hash);
                context.Response.WriteFile(file);
                context.Response.ContentType = Globals.GetContentType(extension);
                context.Response.Charset = "utf-8";
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}
