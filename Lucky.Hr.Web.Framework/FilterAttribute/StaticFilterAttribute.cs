using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Lucky.Hr.Web.Framework.FilterAttribute
{

    /// <summary>
    /// 生成静态页面过滤属性
    /// </summary>
    public class StaticFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {

            if (filterContext.HttpContext.Response.StatusCode == 200)
            {
                filterContext.HttpContext.Response.Filter = new StaticFileWriteResponseFilterWrapper(filterContext.HttpContext.Response.Filter, filterContext);
            }
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
    }

    internal class StaticFileWriteResponseFilterWrapper : System.IO.Stream
    {
        private Stream inner;
        private FileStream writer;
        private ControllerContext context;
        private int expireSconds;
        private bool filter;
        private string tempPath, path;

        public StaticFileWriteResponseFilterWrapper(System.IO.Stream s, ControllerContext context, int expireSeconds = 600)
        {
            this.filter = false;
            this.inner = s;
            this.context = context;
            this.expireSconds = expireSeconds;
            this.EnsureStaticFile();
        }

        private void EnsureStaticFile()
        {
            this.path = this.context.HttpContext.Server.MapPath(HttpContext.Current.Request.Path);

            if (!Path.HasExtension(path))
            {
                return;
            }
            if (!".html".Equals(Path.GetExtension(HttpContext.Current.Request.Path)))
            {
                return;
            }

            if (File.Exists(path))
            {
                var delay = DateTime.UtcNow - File.GetCreationTimeUtc(path);
                if (delay.TotalSeconds <= this.expireSconds)
                {
                    return;
                }
                File.Delete(path);
            }
            else
            {
                var dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                {

                    try
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                    }
                    catch
                    {
                    }
                }
            }
            this.filter = true;

            this.tempPath = this.path + "_" + DateTime.Now.Ticks;

            try
            {
                writer = new FileStream(tempPath, FileMode.Create, FileAccess.Write);
            }
            catch
            {
                this.filter = false;
            }
        }



        public override bool CanRead
        {
            get { return inner.CanRead; }
        }

        public override bool CanSeek
        {
            get { return inner.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return inner.CanWrite; }
        }

        public override void Flush()
        {
            inner.Flush();
        }



        public override long Length
        {
            get { return inner.Length; }
        }

        public override long Position
        {
            get { return inner.Position; }
            set { inner.Position = value; }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return inner.Read(buffer, offset, count);
        }

        public override long Seek(long offset, System.IO.SeekOrigin origin)
        {
            return inner.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            inner.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            try
            {
                inner.Write(buffer, offset, count);
            }
            catch (Exception ex)
            {
            }

            try
            {
                this.writer.Write(buffer, offset, count);
            }
            catch (Exception ex)
            {

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (this.filter)
            {
                try
                {
                    if (this.writer != null)
                    {
                        this.writer.Dispose();
                        this.writer = null;
                    }

                    File.Delete(this.path);
                    File.Move(this.tempPath, this.path);

                    #region 生成文件日志

                    #endregion
                }
                catch
                {
                }

            }
            base.Dispose(disposing);
        }
    }
}

