using System;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class LogContextExtensions
    {
        [Pure]
        public static ILog ForContext([NotNull] this ILog log, [NotNull] Type type) =>
            log.ForContext(type.FullName);

        [Pure]
        public static ILog ForContext<T>([NotNull] this ILog log) =>
            log.ForContext(typeof(T));
    }
}