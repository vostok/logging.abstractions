using System;
using System.Text;

namespace Vostok.Logging.Abstractions.Helpers
{
    internal static class StringBuilderCache
    {
        private const int MaximumSize = 768;

        [ThreadStatic]
        private static StringBuilder CachedInstance;

        public static StringBuilder Acquire(int capacity)
        {
            if (capacity <= MaximumSize)
            {
                var builder = CachedInstance;
                if (capacity <= builder?.Capacity)
                {
                    CachedInstance = null;
                    builder.Clear();
                    return builder;
                }
            }

            return new StringBuilder(capacity);
        }

        public static string GetStringAndRelease(StringBuilder builder)
        {
            if (builder.Capacity <= MaximumSize)
            {
                CachedInstance = builder;
            }

            return builder.ToString();
        }
    }
}
