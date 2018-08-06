using JetBrains.Annotations;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions
{
    internal static class LogEventExtensions
    {
        [Pure]
        public static LogEvent WithParameters(this LogEvent @event, object[] parameters)
        {
            if (parameters == null)
                return @event;

            for (var i = 0; i < parameters.Length; i++)
            {
                @event = @event.WithProperty(i.ToString(), parameters[i]);
            }

            return @event;
        }

        [Pure]
        public static LogEvent WithObjectProperties<T>(this LogEvent @event, T @object, bool allowOverwrite = true)
        {
            if (@object == null)
                return @event;

            foreach (var (name, value) in ObjectPropertiesExtractor.ExtractProperties(@object))
            {
                @event = @event.WithProperty(name, value, allowOverwrite);
            }

            return @event;
        }
    }
}
