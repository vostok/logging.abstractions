using System;
using System.Linq.Expressions;

namespace Vostok.Logging.Abstractions
{
    // TODO(krait): Tests.
    internal static class LogEventExtensions
    {
        public static LogEvent WithParameters(this LogEvent @event, object[] parameters)
        {
            if (parameters == null)
                return @event;

            for (int i = 0; i < parameters.Length; i++)
            {
                @event = @event.WithProperty(i.ToString(), parameters[i]);
            }

            return @event;
        }

        public static LogEvent WithObjectProperties<T>(this LogEvent @event, T @object)
        {
            if (@object == null)
                return @event;

            for (int i = 0; i < ObjectWrapper<T>.Properties.Length; i++)
            {
                var property = ObjectWrapper<T>.Properties[i];
                @event = @event.WithProperty(property.key, property.getter(@object));
            }

            return @event;
        }

        private static class ObjectWrapper<T>
        {
            public static readonly (string key, Func<T, object> getter)[] Properties = BuildProperties();

            private static (string, Func<T, object>)[] BuildProperties()
            {
                var typeProperties = typeof (T).GetProperties();
                var properties = new (string, Func<T, object>)[typeProperties.Length];

                for (int i = 0; i < typeProperties.Length; i++)
                {
                    var parameter = Expression.Parameter(typeof (T));
                    var getter = Expression.Lambda<Func<T, object>>(
                            Expression.Convert(Expression.PropertyOrField(parameter, typeProperties[i].Name), typeof (object)),
                            parameter)
                        .Compile();
                    properties[i] = (typeProperties[i].Name, getter);
                }

                return properties;
            }
        }
    }
}
