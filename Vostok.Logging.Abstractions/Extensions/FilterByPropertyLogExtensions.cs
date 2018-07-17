using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions
{
    [PublicAPI]
    public static class FilterByPropertyLogExtensions
    {
        /// <summary>
        /// <para>Returns a wrapper log that only logs events which have a property with given <paramref name="key"/> and a value of type <typeparamref name="T"/> that is matched by <paramref name="allow"/> predicate.</para>
        /// <para>Log events that do not have a property with such <paramref name="key"/> are dropped.</para>
        /// <para>Log events that do have a property whose value is not of type <typeparamref name="T"/> are also dropped.</para>
        /// </summary>
        [Pure]
        public static ILog WithEventsSelectedByProperty<T>(this ILog log, string key, Predicate<T> allow)
        {
            return new PropertyFilterLog<T>(log, key, allow, true);
        }

        /// <summary>
        /// <para>Returns a wrapper log that only logs events whose properties are matched by <paramref name="allow"/> predicate.</para>
        /// </summary>
        [Pure]
        public static ILog WithEventsSelectedByProperties(this ILog log, Predicate<IReadOnlyDictionary<string, object>> allow)
        {
            return new PropertiesFilterLog(log, allow, true);
        }

        /// <summary>
        /// <para>Returns a wrapper log that drops events which have a property with given <paramref name="key"/> and a value of type <typeparamref name="T"/> that is matched by <paramref name="reject"/> predicate.</para>
        /// <para>Log events that do not have a property with such <paramref name="key"/> are logged.</para>
        /// <para>Log events that do have a property whose value is not of type <typeparamref name="T"/> are also logged.</para>
        /// </summary>
        [Pure]
        public static ILog WithEventsDroppedByProperty<T>(this ILog log, string key, Predicate<T> reject)
        {
            return new PropertyFilterLog<T>(log, key, reject, false);
        }

        /// <summary>
        /// <para>Returns a wrapper log that drops events whose properties are matched by <paramref name="reject"/> predicate.</para>
        /// </summary>
        [Pure]
        public static ILog WithEventsDroppedByProperties(this ILog log, Predicate<IReadOnlyDictionary<string, object>> reject)
        {
            return new PropertiesFilterLog(log, reject, false);
        }

        private class PropertyFilterLog<T> : ILog
        {
            private readonly ILog baseLog;
            private readonly string key;
            private readonly Predicate<T> criterion;
            private readonly bool criterionAllowsEvents;

            public PropertyFilterLog(ILog baseLog, string key, Predicate<T> criterion, bool criterionAllowsEvents)
            {
                this.baseLog = baseLog;
                this.key = key;
                this.criterion = criterion;
                this.criterionAllowsEvents = criterionAllowsEvents;
            }

            public void Log(LogEvent @event)
            {
                var properties = @event?.Properties;

                var criterionMatches = 
                    properties != null && 
                    properties.TryGetValue(key, out var value) && 
                    value is T typedValue &&
                    criterion(typedValue);

                if (criterionMatches == criterionAllowsEvents)
                {
                    baseLog.Log(@event);
                }
            }

            public bool IsEnabledFor(LogLevel level)
            {
                return baseLog.IsEnabledFor(level);
            }

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);

                return ReferenceEquals(baseLogForContext, baseLog) ? this : new PropertyFilterLog<T>(baseLogForContext, key, criterion, criterionAllowsEvents);
            }
        }

        private class PropertiesFilterLog : ILog
        {
            private readonly ILog baseLog;
            private readonly Predicate<IReadOnlyDictionary<string, object>> criterion;
            private readonly bool criterionAllowsEvents;

            public PropertiesFilterLog(ILog baseLog, Predicate<IReadOnlyDictionary<string, object>> criterion, bool criterionAllowsEvents)
            {
                this.baseLog = baseLog;
                this.criterion = criterion;
                this.criterionAllowsEvents = criterionAllowsEvents;
            }

            public void Log(LogEvent @event)
            {
                var criterionMatches = criterion(@event?.Properties ?? DictionarySnapshot<string, object>.Empty);
                if (criterionMatches == criterionAllowsEvents)
                {
                    baseLog.Log(@event);
                }
            }

            public bool IsEnabledFor(LogLevel level)
            {
                return baseLog.IsEnabledFor(level);
            }

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);

                return ReferenceEquals(baseLogForContext, baseLog) ? this : new PropertiesFilterLog(baseLogForContext, criterion, criterionAllowsEvents);
            }
        }
    }
}