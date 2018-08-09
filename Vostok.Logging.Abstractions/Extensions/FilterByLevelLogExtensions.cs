using System;
using System.Linq;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class FilterByLevelLogExtensions
    {
        /// <summary>
        /// Returns a wrapper log that ignores <see cref="LogEvent"/>s with log level less than <paramref name="minLevel"/>.
        /// </summary>
        [Pure]
        public static ILog WithMinimumLevel([NotNull] this ILog log, LogLevel minLevel) =>
            new FilterByLevelLog(log, minLevel);

        /// <summary>
        /// Returns a wrapper log that ignores <see cref="LogEvent"/>s with log levels among provided <paramref name="disabledLevels"/>.
        /// </summary>
        [Pure]
        public static ILog WithDisabledLevels([NotNull] this ILog log, [NotNull] params LogLevel[] disabledLevels) =>
            new DisabledLevelsLog(log, disabledLevels);

        private class FilterByLevelLog : ILog
        {
            private readonly ILog baseLog;
            private readonly LogLevel minLevel;

            public FilterByLevelLog(ILog baseLog, LogLevel minLevel)
            {
                this.baseLog = baseLog ?? throw new ArgumentNullException(nameof(baseLog));
                this.minLevel = minLevel;
            }

            public void Log(LogEvent @event)
            {
                if (@event?.Level >= minLevel)
                    baseLog.Log(@event);
            }

            public bool IsEnabledFor(LogLevel level) =>
                level >= minLevel && baseLog.IsEnabledFor(level);

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);

                return ReferenceEquals(baseLogForContext, baseLog) ? this : new FilterByLevelLog(baseLogForContext, minLevel);
            }
        }

        private class DisabledLevelsLog : ILog
        {
            private readonly ILog baseLog;
            private readonly LogLevel[] disabledLevels;

            public DisabledLevelsLog(ILog baseLog, LogLevel[] disabledLevels)
            {
                this.baseLog = baseLog ?? throw new ArgumentNullException(nameof(baseLog));
                this.disabledLevels = disabledLevels ?? throw new ArgumentNullException(nameof(disabledLevels));
            }

            public void Log(LogEvent @event)
            {
                if (@event == null || !disabledLevels.Contains(@event.Level))
                    baseLog.Log(@event);
            }

            public bool IsEnabledFor(LogLevel level) =>
                !disabledLevels.Contains(level) && baseLog.IsEnabledFor(level);

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);

                return ReferenceEquals(baseLogForContext, baseLog) ? this : new DisabledLevelsLog(baseLogForContext, disabledLevels);
            }
        }
    }
}