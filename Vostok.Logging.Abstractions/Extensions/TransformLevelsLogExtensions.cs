using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class TransformLevelsLogExtensions
    {
        /// <summary>
        /// Returns a wrapper log that transforms log levels of incoming log events according to provided mapping.
        /// </summary>
        [Pure]
        public static ILog WithLevelsTransformation([NotNull] this ILog log, [NotNull] IReadOnlyDictionary<LogLevel, LogLevel> mapping)
            => new TransformLevelsLog(log, mapping);

        private class TransformLevelsLog : ILog
        {
            private readonly ILog baseLog;
            private readonly IReadOnlyDictionary<LogLevel, LogLevel> mapping;

            public TransformLevelsLog(ILog baseLog, IReadOnlyDictionary<LogLevel, LogLevel> mapping)
            {
                this.baseLog = baseLog ?? throw new ArgumentNullException(nameof(baseLog));
                this.mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
            }

            public void Log(LogEvent @event)
            {
                if (@event != null)
                {
                    var transformedLevel = TransformLevel(@event.Level);
                    if (transformedLevel != @event.Level)
                        @event = @event.WithLevel(transformedLevel);
                }

                baseLog.Log(@event);
            }

            public bool IsEnabledFor(LogLevel level) =>
                baseLog.IsEnabledFor(TransformLevel(level));

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);

                return ReferenceEquals(baseLogForContext, baseLog) ? this : new TransformLevelsLog(baseLogForContext, mapping);
            }

            private LogLevel TransformLevel(LogLevel level)
                => mapping.TryGetValue(level, out var mappedLevel) ? mappedLevel : level;
        }
    }
}