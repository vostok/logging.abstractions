using System.Collections.Generic;
using System.Linq;
using Vostok.Commons.Collections;
using Vostok.Commons.Formatting;

namespace Vostok.Logging.Abstractions
{
    internal static class LogPropertiesExtensions
    {
        public static ImmutableArrayDictionary<string, object> WithObjectProperties<T>(
            this ImmutableArrayDictionary<string, object> properties,
            T @object,
            bool allowOverwrite,
            bool allowNullValues)
        {
            if (@object == null)
                return properties;

            var pairs = @object is IReadOnlyDictionary<string, object> dictionary
                ? dictionary.Select(pair => (pair.Key, pair.Value))
                : ObjectPropertiesExtractor.ExtractProperties(@object);

            foreach (var (name, value) in pairs)
            {
                if (!allowNullValues && value == null)
                    continue;

                properties = properties.Set(name, value, allowOverwrite);
            }

            return properties;
        }
    }
}
