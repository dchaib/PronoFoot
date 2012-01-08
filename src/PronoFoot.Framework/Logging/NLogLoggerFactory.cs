using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

namespace PronoFoot.Logging
{
    public class NLogLoggerFactory : ILoggerFactory
    {
        #region ILoggerFactory Members

        public ILogger CreateLogger(Type type)
        {
            return new NLogLogger(LogManager.GetLogger(type.FullName));
        }

        #endregion
    }
}
