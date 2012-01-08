using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PronoFoot.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(Type type);
    }
}
