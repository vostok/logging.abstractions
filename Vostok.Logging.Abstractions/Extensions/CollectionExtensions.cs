using System.Collections.Generic;

namespace Vostok.Logging.Abstractions
{
    internal static class CollectionExtensions
    {
        public static IReadOnlyDictionary<string, object> ToDictionary(this object[] parameters)
        {
            var result = new Dictionary<string, object>();

            for (var i = 0; i < parameters.Length; i++)
            {
                result.Add(i.ToString(), parameters[i]);
            }

            return result;
        }

        public static IReadOnlyDictionary<string, object> ToDictionary<T>(this T obj)
        {
            var result = new Dictionary<string, object>();
            var properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(obj);
                result.Add(property.Name, value);
            }

            return result;
        }
    }
}