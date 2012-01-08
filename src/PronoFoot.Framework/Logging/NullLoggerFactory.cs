using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Logging
{
    public class NullLoggerFactory : ILoggerFactory
    {
        #region ILoggerFactory Members

        public ILogger CreateLogger(Type type)
        {
            return NullLogger.Instance;
        }

        #endregion
    }
}
