using System;

namespace Vostok.Logging.Abstractions
{
    public static class LogContextExtensions
    {
        public static ILog ForContext(this ILog log, Type type)
        {
            return log.ForContext(type.FullName);
        }

        public static ILog ForContext<T>(this ILog log)
        {
            return log.ForContext(typeof (T));
        }

        public static ILog ForContext<T>(this ILog log, T _)
        {
            return log.ForContext<T>();
        }
    }
}
