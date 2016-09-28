using Lucky.Core.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Lucky.Hr.WebSite
{
    public class LoggingFilterAttribute : ActionFilterAttribute
    {
        private ILogger _logger;
        private const string StopwatchKey = "DebugLoggingStopWatch";

        public LoggingFilterAttribute(ILogger logger) : base()
        {
            _logger = logger;
        }

        #region overriding
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            LogEnd(filterContext);
            base.OnActionExecuted(filterContext);
        }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LogBegin(filterContext);
            base.OnActionExecuting(filterContext);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            base.OnResultExecuting(filterContext);
        }
        #endregion

        #region implementation
        private  void LogBegin(ActionExecutingContext filterContext)
        {
            try
            {
                var loggingWatch = Stopwatch.StartNew();
                filterContext.HttpContext.Items.Add(StopwatchKey, loggingWatch);
            }catch(Exception ex)
            {
                _logger.Error($"跟踪API调用的日志服务失败", ex);
            }
        }

        private  void LogEnd(ActionExecutedContext filterContext)
        {
            try
            {
                if (filterContext.HttpContext.Items[StopwatchKey] != null)
                {
                    var loggingWatch = (Stopwatch)filterContext.HttpContext.Items[StopwatchKey];
                    loggingWatch.Stop();

                    long timeSpent = loggingWatch.ElapsedMilliseconds;

                    string url = filterContext?.HttpContext?.Request?.RawUrl ?? "na";
                    string remoteIp = filterContext?.HttpContext?.Request?.UserHostAddress ?? "na";
                    string sessionId = filterContext?.HttpContext?.Session?.SessionID ?? "na";

                    if (timeSpent > 1000)//处理时间大于1秒，用警告，在调试时比较容易看出来。
                    {
                        _logger.Warning($@"url:[{url}]; clientIP:[{remoteIp}]; sessionID:[{sessionId}]");
                    }
                    else
                    {
                        _logger.Debug($@"url:[{url}]; clientIP:[{remoteIp}]; sessionID:[{sessionId}]");
                    }
                    filterContext.HttpContext.Items.Remove(StopwatchKey);
                }
            }catch(Exception ex)
            {
                _logger.Error($"跟踪API调用的日志服务失败", ex);
            }
        }

        #endregion
    }
}