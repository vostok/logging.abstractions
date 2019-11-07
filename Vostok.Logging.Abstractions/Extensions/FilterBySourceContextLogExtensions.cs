using System;
using System.Linq;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions.Values;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class FilterBySourceContextLogExtensions
    {
        /// <summary>
        /// <para>Returns a wrapper log that only logs events made by log with given <paramref name="context"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// </summary>
        [Pure]
        public static ILog WithEventsSelectedBySourceContext([NotNull] this ILog log, [NotNull] string context)
        {
            return new SourceContextFilterLog(log, context, true);
        }
        
        /// <summary>
        /// <para>Returns a wrapper log that only logs events made by log with context equal to the name of <typeparamref name="T"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// </summary>
        [Pure]
        public static ILog WithEventsSelectedBySourceContext<T>([NotNull] this ILog log)
        {
            return WithEventsSelectedBySourceContext(log, typeof(T).Name);
        }

        /// <summary>
        /// <para>Returns a wrapper log that drops events made by log with given <paramref name="context"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// </summary>
        [Pure]
        public static ILog WithEventsDroppedBySourceContext([NotNull] this ILog log, [NotNull] string context)
        {
            return new SourceContextFilterLog(log, context, false);
        }
        
        /// <summary>
        /// <para>Returns a wrapper log that drops events made by log with context equal to the name of <typeparamref name="T"/> passed to <see cref="ILog.ForContext(string)"/></para>
        /// </summary>
        [Pure]
        public static ILog WithEventsDroppedBySourceContext<T>([NotNull] this ILog log)
        {
            return WithEventsDroppedBySourceContext(log, typeof(T).Name);
        }
        
        public class SourceContextFilterLog : ILog
        {
            private readonly ILog baseLog;
            private readonly string contextFilterValue;
            private readonly bool filterAllowsEvent;
            private bool logEnabled;

            public SourceContextFilterLog(ILog baseLog, string contextFilterValue, bool filterAllowsEvent)
            {
                this.baseLog = baseLog ?? throw new ArgumentNullException(nameof(baseLog));
                this.contextFilterValue = contextFilterValue ?? throw new ArgumentNullException(nameof(contextFilterValue));
                this.filterAllowsEvent = filterAllowsEvent;
                logEnabled = !filterAllowsEvent;
            }

            private SourceContextFilterLog(ILog baseLog, string contextFilterValue, bool filterAllowsEvent, bool logEnabled)
                : this(baseLog, contextFilterValue, filterAllowsEvent)
            {
                this.logEnabled = logEnabled;
            }

            public void Log(LogEvent @event)
            {
                if (logEnabled)
                {
                    baseLog.Log(@event);
                }
            }

            public bool IsEnabledFor(LogLevel level) => baseLog.IsEnabledFor(level);

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);

                if (context == contextFilterValue)
                {
                    var newLogEnabled = filterAllowsEvent;
                    return new SourceContextFilterLog(baseLogForContext, contextFilterValue, filterAllowsEvent, newLogEnabled);
                }

                return ReferenceEquals(baseLogForContext, baseLog)
                    ? this
                    : new SourceContextFilterLog(baseLogForContext, contextFilterValue, filterAllowsEvent, logEnabled);
            }
        }
    }
}