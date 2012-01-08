using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, Exception exception, string format, params object[] args);
    }
}
