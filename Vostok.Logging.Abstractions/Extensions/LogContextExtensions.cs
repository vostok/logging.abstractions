using System;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class LogContextExtensions
    {
        [Pure]
        public static ILog ForContext(this ILog log, Type type)
        {
            return log.ForContext(type.FullName);
        }

        [Pure]
        public static ILog ForContext<T>(this ILog log)
        {
            return log.ForContext(typeof (T));
        }

        [Pure]
        public static ILog ForContext<T>(this ILog log, T _)
        {
            return log.ForContext<T>();
        }
    }
}
