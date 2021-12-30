#if NET6_0

using System;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Vostok.Commons.Time;
using Vostok.Logging.Abstractions.Helpers;
using Vostok.Logging.Abstractions.Interpolated;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class LogExtensions_Interpolated
    {
        public static void Info(this ILog log, [InterpolatedStringHandlerArgument("log")] ref InterpolatedStringHandler message)
        {
            if (!message.IsEnabled)
                return;
            
            log.Log(new LogEvent(LogLevel.Info, PreciseDateTime.Now, message.MessageTemplate.ToString(), message.Properties, null));
        }
        
        public static void Info(this ILog log, [CanBeNull] Exception exception, [InterpolatedStringHandlerArgument("log")] ref InterpolatedStringHandler message)
        {
            if (!message.IsEnabled)
                return;
            
            log.Log(new LogEvent(LogLevel.Info, PreciseDateTime.Now, message.MessageTemplate.ToString(), message.Properties, exception));
        }
    }
}

#endif