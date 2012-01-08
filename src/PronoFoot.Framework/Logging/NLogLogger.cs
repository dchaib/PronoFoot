using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace PronoFoot.Logging
{
    public class NLogLogger : ILogger
    {
        private readonly Logger logger;

        public NLogLogger(Logger logger)
        {
            this.logger = logger;
        }

        #region ILogger Members

        public void Log(LogLevel level, Exception exception, string format, params object[] args)
        {
            var logEvent = new LogEventInfo(GetNLogLogLevel(level), logger.Name, null, format, args, exception);
            logger.Log(logEvent);
        }

        #endregion

        private static NLog.LogLevel GetNLogLogLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                    return NLog.LogLevel.Trace;
                case LogLevel.Debug:
                    return NLog.LogLevel.Debug;
                case LogLevel.Information:
                    return NLog.LogLevel.Info;
                case LogLevel.Warning:
                    return NLog.LogLevel.Warn;
                case LogLevel.Error:
                    return NLog.LogLevel.Error;
                case LogLevel.Fatal:
                    return NLog.LogLevel.Fatal;
                default:
                    return NLog.LogLevel.Off;
            }
        }
    }
}
