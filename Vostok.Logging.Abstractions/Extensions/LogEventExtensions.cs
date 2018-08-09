using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions
{
    internal static class LogEventExtensions
    {
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

        [Pure]
        public static LogEvent WithParameters(this LogEvent @event, object[] parameters)
        {
            if (parameters == null)
                return @event;

            var templatePropertyNames = TemplatePropertiesExtractor.ExtractPropertyNames(@event.MessageTemplate);

            if (ShouldInferNamesForPositionalParameters(parameters, templatePropertyNames))
            {
                // (iloktionov): Name positional parameters with corresponding placeholder names from template:
                for (var i = 0; i < parameters.Length; i++)
                {
                    @event = @event.WithProperty(templatePropertyNames[i], parameters[i]);
                }
            }
            else
            {
                // (iloktionov): Name positional parameters with their indices:
                for (var i = 0; i < parameters.Length; i++)
                {
                    @event = @event.WithProperty(i.ToString(), parameters[i]);
                }
            }

            return @event;
        }

        private static bool ShouldInferNamesForPositionalParameters(object[] parameters, string[] propertyNames)
        {
            if (parameters.Length != propertyNames.Length)
                return false;

            if (propertyNames.Length == 0)
                return false;

            if (propertyNames.All(IsPositionalName))
                return false;

            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static bool IsPositionalName(string propertyName)
        {
            foreach (var character in propertyName)
            {
                if (character < '0')
                    return false;

                if (character > '9')
                    return false;
            }

            return true;
        }
    }
}
