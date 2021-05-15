using System.Collections.Generic;
using System.Linq;
using Vostok.Commons.Collections;
using Vostok.Commons.Formatting;
using Vostok.Logging.Abstractions.CommonsCopy;

namespace Vostok.Logging.Abstractions
{
    internal static class LogPropertiesExtensions
    {
        public static ImmutableArrayDictionary<string, object> WithObjectPropertiesSetRage<T>(
            this ImmutableArrayDictionary<string, object> properties,
            T @object,
            bool allowOverwrite,
            bool allowNullValues)
        {
            if (@object == null)
                return properties;

            (string, object)[] pairs;
            if (@object is IReadOnlyDictionary<string, object> dictionary)
            {
                pairs = dictionary.Select(pair => (pair.Key, pair.Value)).ToArray();
            }
            else
            {
                pairs = ObjectPropertiesExtractor.ExtractPropertiesArray(@object);
            }

            var hasNull = false;

            if (!allowNullValues)
            {
                for (var i = 0; i < pairs.Length; i++)
                    if (pairs[i].Item2 == null)
                    {
                        hasNull = true;
                        break;
                    }
            }

            if (hasNull)
            {
                foreach (var (name, value) in pairs)
                {
                    if (!allowNullValues && value == null)
                        continue;

                    properties = properties.Set(name, value, allowOverwrite);
                }
            }
            else
            {
                properties = properties.SetRangeUnsafe(pairs, allowOverwrite);
            }

            return properties;
        }


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
