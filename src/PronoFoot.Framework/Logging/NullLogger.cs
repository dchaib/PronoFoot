using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Logging
{
    public class NullLogger : ILogger
    {
        private static readonly ILogger instance = new NullLogger();

        public static ILogger Instance
        {
            get { return instance; }
        }


        #region ILogger Members

        public void Log(LogLevel level, Exception exception, string format, params object[] args)
        {
        }

        #endregion
    }
}
