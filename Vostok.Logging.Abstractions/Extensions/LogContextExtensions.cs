using System;
using System.Text;
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
            return log.ForContext((useFullTypeName ? type.FullName : GetShortTypeName(type)) ?? string.Empty);
        }

        [Pure]
        public static ILog ForContext<T>([NotNull] this ILog log)
        {
            return log.ForContext(Holder<T>.ShortTypeName);
        }

        [Pure]
        public static ILog ForContext<T>([NotNull] this ILog log, bool useFullTypeName)
        {
            if (useFullTypeName)
                return log.ForContext(typeof(T).FullName);

            return log.ForContext<T>();
        }

        private static string GetShortTypeName(Type type)
        {
            var typeName = type.Name;
            if (!type.IsGenericType)
                return typeName;

            var result = new StringBuilder();
            result.Append(typeName.Substring(0, typeName.LastIndexOf("`")));
            foreach (var arg in type.GetGenericArguments())
            {
                result.Append('`');
                result.Append(GetShortTypeName(arg));
            }
            
            return result.ToString();
        }

        private class Holder<T>
        {
            public static readonly string ShortTypeName = GetShortTypeName(typeof(T));
        }
    }
}
