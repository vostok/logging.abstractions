using System;
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

            return FillExistingProperties(@object, allowOverwrite, allowNullValues, properties);
        }

        public static ImmutableArrayDictionary<string, object> GenerateInitialObjectProperties<T>(
            T @object,
            bool allowNullValues)
        {
            if (@object == null)
                return null;

            if (allowNullValues)
            {
                if (@object is IReadOnlyDictionary<string, object> dictionary)
                    return LogEvent.CreatePropertiesFromSource(dictionary);

                //(deniaa): Object properties are always unique by design so we can fill immutable array dictionary without worrying about using the ImmutableArrayDictionary.Set method and overwrite flag.
                var (count, pairs) = ObjectPropertiesExtractor.ExtractPropertiesWithCount(@object);
                return LogEvent.CreatePropertiesFromSource(Math.Max(4, count), count, pairs);
            }

            var properties = LogEvent.CreateProperties();

            return FillExistingProperties(@object, true, false, properties);
        }

        private static ImmutableArrayDictionary<string, object> FillExistingProperties<T>(T @object, bool allowOverwrite, bool allowNullValues, ImmutableArrayDictionary<string, object> properties)
        {
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
