using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lucky.Core.Utility;
using NLog;

namespace Lucky.Core.Logging
{
    public class HrLogger:ILogger
    {
        private readonly NLog.Logger _logger;
        public HrLogger()
        {
            _logger = LogManager.GetLogger("c");
        }
        public bool IsEnabled(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    return _logger.IsDebugEnabled;
                case LogLevel.Information:
                    return _logger.IsInfoEnabled;
                case LogLevel.Warning:
                    return _logger.IsWarnEnabled;
                case LogLevel.Error:
                    return _logger.IsErrorEnabled;
                case LogLevel.Fatal:
                    return _logger.IsFatalEnabled;
            }
            return false;
        }

        public void Log(LogLevel level, Exception exception, string format, params object[] args)
        {
            if (args == null)
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        _logger.Debug(format, exception);
                        break;
                    case LogLevel.Information:
                        _logger.Info(format, exception);
                        break;
                    case LogLevel.Warning:
                        _logger.Warn(format, exception);
                        break;
                    case LogLevel.Error:
                        _logger.Error(format, exception);
                        break;
                    case LogLevel.Fatal:
                        _logger.Fatal(format, exception);
                        break;
                }
            }
            else
            {
                switch (level)
                {
                    case LogLevel.Debug:
                        DebugFormat(exception, format, args);
                        break;
                    case LogLevel.Information:
                        InfoFormat(exception, format, args);
                        break;
                    case LogLevel.Warning:
                        WarnFormat(exception, format, args);
                        break;
                    case LogLevel.Error:
                        ErrorFormat(exception, format, args);
                        break;
                    case LogLevel.Fatal:
                        FatalFormat(exception, format, args);
                        break;
                }
            }
        }
        protected void DebugFormat(Exception exception, String format, params Object[] args)
        {
            if (_logger.IsDebugEnabled)
            {
                _logger.Debug(new SystemStringFormat(CultureInfo.InvariantCulture, format, args).ToString(), exception);
            }
        }

        protected void ErrorFormat(Exception exception, String format, params Object[] args)
        {
            if (_logger.IsErrorEnabled)
            {
                _logger.Error(new SystemStringFormat(CultureInfo.InvariantCulture, format, args).ToString(), exception);
            }
        }
        protected void FatalFormat(Exception exception, String format, params Object[] args)
        {
            if (_logger.IsFatalEnabled)
            {
                _logger.Fatal( new SystemStringFormat(CultureInfo.InvariantCulture, format, args).ToString(), exception);
            }
        }
        protected void InfoFormat(Exception exception, String format, params Object[] args)
        {
            if (_logger.IsInfoEnabled)
            {
                _logger.Info( new SystemStringFormat(CultureInfo.InvariantCulture, format, args).ToString(), exception);
            }
        }
        protected void WarnFormat(Exception exception, String format, params Object[] args)
        {
            if (_logger.IsWarnEnabled)
            {
                _logger.Warn( new SystemStringFormat(CultureInfo.InvariantCulture, format, args).ToString(), exception);
            }
        }

    }
}
