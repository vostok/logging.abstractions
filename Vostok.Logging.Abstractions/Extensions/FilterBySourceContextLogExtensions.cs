using System.Linq;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions.Values;

namespace Vostok.Logging.Abstractions
{
     public static class FilterBySourceContextLogExtensions
    {
        public static ILog WithEventsSelectedBySourceContext(this ILog log, string context)
        {
            return new SourceContextFilterLog(log, context, true);
        }

        public static ILog WithEventsDroppedBySourceContext(this ILog log, string context)
        {
            return new SourceContextFilterLog(log, context, false);
        }

        public class SourceContextFilterLog : ILog
        {
            private readonly ILog baseLog;
            private readonly string contextFilterValue;
            private readonly bool filterAllowsEvent;

            public SourceContextFilterLog([NotNull] ILog baseLog,
                                          [CanBeNull] SourceContextValue sourceContextValue,
                                          [NotNull] string contextFilterValue,
                                          bool filterAllowsEvent)
            {
                this.baseLog = baseLog;
                this.contextFilterValue = contextFilterValue;
                this.filterAllowsEvent = filterAllowsEvent;
                SourceContext = sourceContextValue;
            }

            public SourceContextFilterLog([NotNull] ILog baseLog,
                                          [NotNull] string contextFilterValue,
                                          bool filterAllowsEvent)
                : this(baseLog, null, contextFilterValue, filterAllowsEvent)
            {
            }

            [CanBeNull]
            public SourceContextValue SourceContext { get; }

            public void Log(LogEvent @event)
            {
                var matchFilter = SourceContext?.Contains(contextFilterValue) ?? false;
                if (matchFilter == filterAllowsEvent)
                {
                    baseLog.Log(@event);
                }
            }

            public bool IsEnabledFor(LogLevel level) => baseLog.IsEnabledFor(level);

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);
                var newSourceContext = SourceContext + new SourceContextValue(context);
                return new SourceContextFilterLog(baseLogForContext, newSourceContext, contextFilterValue, filterAllowsEvent);
            }
        }
    }
}