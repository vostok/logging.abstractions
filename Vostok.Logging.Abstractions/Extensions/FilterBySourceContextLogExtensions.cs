using System;
using System.Linq;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class FilterBySourceContextLogExtensions
    {
        /// <summary>
        /// <para>Returns a wrapper log that only logs events made by log with given <paramref name="context"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// <para>Provided context value is treated as a case-insensitive prefix.</para>
        /// </summary>
        [Pure]
        public static ILog WithEventsSelectedBySourceContext([NotNull] this ILog log, [NotNull] string context) =>
            new SourceContextFilterLog(log, context, true);

        /// <summary>
        /// <para>Returns a wrapper log that only logs events made by log with context equal to the name of <typeparamref name="T"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// </summary>
        [Pure]
        public static ILog WithEventsSelectedBySourceContext<T>([NotNull] this ILog log) =>
            WithEventsSelectedBySourceContext(log, typeof(T).Name);

        /// <summary>
        /// <para>Returns a wrapper log that drops events made by log with given <paramref name="context"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// <para>Provided context value is treated as a case-insensitive prefix.</para>
        /// </summary>
        [Pure]
        public static ILog WithEventsDroppedBySourceContext([NotNull] this ILog log, [NotNull] string context) =>
            new SourceContextFilterLog(log, context, false);

        /// <summary>
        /// <para>Returns a wrapper log that drops events made by log with context equal to the name of <typeparamref name="T"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// </summary>
        [Pure]
        public static ILog WithEventsDroppedBySourceContext<T>([NotNull] this ILog log) =>
            WithEventsDroppedBySourceContext(log, typeof(T).Name);

        /// <summary>
        /// <para>Returns a wrapper log that drops events with level lesser than <paramref name="minLevel"/> made by log with given <paramref name="context"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// <para>Provided context value is treated as a case-insensitive prefix.</para>
        /// </summary>
        [Pure]
        public static ILog WithMinimumLevelForSourceContext([NotNull] this ILog log, [NotNull] string context, LogLevel minLevel)
            => new SourceContextLevelFilterLog(log, new[] {context}, minLevel);

        /// <summary>
        /// <para>Returns a wrapper log that drops events with level lesser than <paramref name="minLevel"/> made by log with context equal to the name of <typeparamref name="T"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// </summary>
        [Pure]
        public static ILog WithMinimumLevelForSourceContext<T>([NotNull] this ILog log, LogLevel minLevel)
            => new SourceContextLevelFilterLog(log, new[] {typeof(T).Name}, minLevel);

        /// <summary>
        /// <para>Returns a wrapper log that drops events with level lesser than <paramref name="minLevel"/> made by log with given <paramref name="contexts"/> passed to sequence of <see cref="ILog.ForContext(string)"/></para>
        /// <para>Provided context value is treated as a case-insensitive prefix.</para>
        /// </summary>
        [Pure]
        public static ILog WithMinimumLevelForSourceContexts([NotNull] this ILog log, LogLevel minLevel, [NotNull, ItemNotNull] params string[] contexts)
            => new SourceContextLevelFilterLog(log, contexts, minLevel);

        private class SourceContextFilterLog : ILog
        {
            private readonly ILog baseLog;
            private readonly string contextFilterValue;
            private readonly bool filterAllowsEvent;
            private readonly bool logEnabled;

            public SourceContextFilterLog(ILog baseLog, string contextFilterValue, bool filterAllowsEvent)
                : this(baseLog, contextFilterValue, filterAllowsEvent, !filterAllowsEvent)
            {
            }

            private SourceContextFilterLog(ILog baseLog, string contextFilterValue, bool filterAllowsEvent, bool logEnabled)
            {
                this.baseLog = baseLog ?? throw new ArgumentNullException(nameof(baseLog));
                this.contextFilterValue = contextFilterValue ?? throw new ArgumentNullException(nameof(contextFilterValue));
                this.filterAllowsEvent = filterAllowsEvent;
                this.logEnabled = logEnabled;
            }

            public void Log(LogEvent @event)
            {
                if (logEnabled)
                    baseLog.Log(@event);
            }

            public bool IsEnabledFor(LogLevel level) => logEnabled && baseLog.IsEnabledFor(level);

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);

                if (context.StartsWith(contextFilterValue, StringComparison.OrdinalIgnoreCase))
                {
                    var newLogEnabled = filterAllowsEvent;
                    return new SourceContextFilterLog(baseLogForContext, contextFilterValue, filterAllowsEvent, newLogEnabled);
                }

                return ReferenceEquals(baseLogForContext, baseLog)
                    ? this
                    : new SourceContextFilterLog(baseLogForContext, contextFilterValue, filterAllowsEvent, logEnabled);
            }
        }

        private class SourceContextLevelFilterLog : ILog
        {
            private readonly ILog baseLog;
            private readonly string[] contextFilterValues;
            private readonly LogLevel minimumContextLevel;
            private readonly LogLevel minimumEffectiveLevel;

            public SourceContextLevelFilterLog(ILog baseLog, string[] contextFilterValues, LogLevel minimumContextLevel)
                : this(baseLog, contextFilterValues, minimumContextLevel, LogLevel.Debug) { }

            private SourceContextLevelFilterLog(ILog baseLog, string[] contextFilterValues, LogLevel minimumContextLevel, LogLevel minimumEffectiveLevel)
            {
                this.baseLog = baseLog;
                this.contextFilterValues = contextFilterValues;
                this.minimumContextLevel = minimumContextLevel;
                this.minimumEffectiveLevel = minimumEffectiveLevel;
            }

            public void Log(LogEvent @event)
            {
                if (@event?.Level >= minimumEffectiveLevel)
                    baseLog.Log(@event);
            }

            public bool IsEnabledFor(LogLevel level) 
                => level >= minimumEffectiveLevel && baseLog.IsEnabledFor(level);

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);
                var contextFilterValuesWithoutGivenContext = contextFilterValues.Where(x => !context.StartsWith(x, StringComparison.OrdinalIgnoreCase)).ToArray();
                if (!contextFilterValuesWithoutGivenContext.Any())
                    return new SourceContextLevelFilterLog(baseLogForContext, contextFilterValues, minimumContextLevel, minimumContextLevel);

                if (contextFilterValuesWithoutGivenContext.Length < contextFilterValues.Length)
                    return new SourceContextLevelFilterLog(baseLogForContext, contextFilterValuesWithoutGivenContext, minimumContextLevel, minimumEffectiveLevel);

                return ReferenceEquals(baseLogForContext, baseLog)
                           ? this
                           : new SourceContextLevelFilterLog(baseLogForContext, contextFilterValues, minimumContextLevel, minimumEffectiveLevel);
            }
        }
    }
}