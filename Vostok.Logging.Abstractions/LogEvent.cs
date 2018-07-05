using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions.Flow;

namespace Vostok.Logging.Abstractions
{
    public sealed class LogEvent
    {
        public LogLevel Level { get; }

        public DateTimeOffset Timestamp { get; }

        [CanBeNull]
        public string MessageTemplate { get; }

        [CanBeNull]
        public IReadOnlyDictionary<string, object> Properties;

        [CanBeNull]
        public Exception Exception { get; }

        public LogEvent(LogLevel level, DateTimeOffset timestamp, [CanBeNull] string messageTemplate, [CanBeNull] IReadOnlyDictionary<string, object> properties = null, [CanBeNull] Exception exception = null)
            : this(level, timestamp, messageTemplate, CreateProperties(properties), exception)
        {
        }

        private LogEvent(LogLevel level, DateTimeOffset timestamp, string messageTemplate, DictionarySnapshot<string, object> properties, Exception exception)
        {
            Level = level;
            Timestamp = timestamp;
            MessageTemplate = messageTemplate;
            Properties = properties;
            Exception = exception;
        }

        public LogEvent WithProperty<T>([NotNull] string key, [NotNull] T value)
        {
            var properties = Properties == null 
                ? CreateProperties().Set(key, value)
                : ((DictionarySnapshot<string, object>)Properties).Set(key, value);

            return new LogEvent(Level, Timestamp, MessageTemplate, properties, Exception);
        }

        private static DictionarySnapshot<string, object> CreateProperties()
        {
            return new DictionarySnapshot<string, object>(StringComparer.InvariantCultureIgnoreCase);
        }

        private static DictionarySnapshot<string, object> CreateProperties(IReadOnlyDictionary<string, object> collection)
        {
            return collection?.Aggregate(CreateProperties(), (current, property) => current.Set(property.Key, property.Value));
        }
    }
}