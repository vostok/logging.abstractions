using System;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class LogContextExtensions
    {
        [Pure]
        public static ILog ForContext([NotNull] this ILog log, [NotNull] Type type)
        {
            return log.ForContext(type, false);
        }

        [Pure]
        public static ILog ForContext([NotNull] this ILog log, [NotNull] Type type, bool useFullTypeName)
        {
            return log.ForContext((useFullTypeName ? type.FullName : type.Name) ?? string.Empty);
        }

        [Pure]
        public static ILog ForContext<T>([NotNull] this ILog log)
        {
            return log.ForContext<T>(false);
        }

        [Pure]
        public static ILog ForContext<T>([NotNull] this ILog log, bool useFullTypeName)
        {
            return log.ForContext(typeof(T), useFullTypeName);
        }
    }
}
