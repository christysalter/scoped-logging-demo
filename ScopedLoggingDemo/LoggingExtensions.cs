using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopedLoggingDemo
{
    public static class LoggingExtensions
    {
        public static IDisposable BeginLogScope(this ILogger logger, string scopeName)
        {
            return logger.BeginScope(new Dictionary<string, object> { { "ScopeName", scopeName }, { "ScopeBeginTime", DateTime.UtcNow }, { "ScopeId", Guid.NewGuid() } });
        }
    }
}
