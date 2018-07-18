using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class WithPropertyLogExtensions
    {
        /// <summary>
        /// <para>Returns a wrapper log that adds the given property to each <see cref="LogEvent"/> before logging.</para>
        /// <para>By default, existing properties are not overwritten. This can be changed via <paramref name="allowOverwrite"/> parameter.</para>
        /// </summary>
        [Pure]
        public static ILog WithProperty<T>(this ILog log, string key, T value, bool allowOverwrite = false)
        {
            return new WithPropertyLog<T>(log, key, value, allowOverwrite);
        }

        /// <summary>
        /// <para>Returns a wrapper log that adds all of the given properties to each <see cref="LogEvent"/> before logging.</para>
        /// <para>By default, existing properties are not overwritten. This can be changed via <paramref name="allowOverwrite"/> parameter.</para>
        /// </summary>
        [Pure]
        public static ILog WithProperties(this ILog log, IReadOnlyDictionary<string, object> properties, bool allowOverwrite = false)
        {
            return new WithPropertiesLog(log, properties, allowOverwrite);
        }

        private class WithPropertyLog<T> : ILog
        {
            private readonly ILog baseLog;
            private readonly string key;
            private readonly T value;
            private readonly bool allowOverwrite;

            public WithPropertyLog(ILog baseLog, string key, T value, bool allowOverwrite)
            {
                this.baseLog = baseLog ?? throw new ArgumentNullException(nameof(baseLog));
                this.key = key ?? throw new ArgumentNullException(nameof(key));
                this.value = value;
                this.allowOverwrite = allowOverwrite;
            }

            public void Log(LogEvent @event)
            {
                baseLog.Log(@event?.WithProperty(key, value, allowOverwrite));
            }

            public bool IsEnabledFor(LogLevel level)
            {
                return baseLog.IsEnabledFor(level);
            }

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);
                return ReferenceEquals(baseLogForContext, baseLog) ? this : new WithPropertyLog<T>(baseLogForContext, key, value, allowOverwrite);
            }
        }

        private class WithPropertiesLog : ILog
        {
            private readonly ILog baseLog;
            private readonly IReadOnlyDictionary<string, object> properties;
            private readonly bool allowOverwrite;

            public WithPropertiesLog(ILog baseLog, IReadOnlyDictionary<string, object> properties, bool allowOverwrite)
            {
                this.baseLog = baseLog ?? throw new ArgumentNullException(nameof(baseLog));
                this.properties = properties ?? throw new ArgumentNullException(nameof(properties));
                this.allowOverwrite = allowOverwrite;
            }

            public void Log(LogEvent @event)
            {
                foreach (var pair in properties)
                {
                    @event = @event?.WithProperty(pair.Key, pair.Value, allowOverwrite);
                }

                baseLog.Log(@event);
            }

            public bool IsEnabledFor(LogLevel level)
            {
                return baseLog.IsEnabledFor(level);
            }

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);
                return ReferenceEquals(baseLogForContext, baseLog) ? this : new WithPropertiesLog(baseLogForContext, properties, allowOverwrite);
            }
        }
    }
}
