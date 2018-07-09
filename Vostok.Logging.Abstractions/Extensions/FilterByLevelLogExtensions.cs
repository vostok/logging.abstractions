namespace Vostok.Logging.Abstractions
{
    public static class FilterByLevelLogExtensions
    {
        /// <summary>
        /// Returns a wrapper log that ignores <see cref="LogEvent"/>s with log level less than <paramref name="minLevel"/>.
        /// </summary>
        public static ILog FilterByLevel(this ILog log, LogLevel minLevel)
        {
            return new FilterByLevelLog(log, minLevel);
        }

        private class FilterByLevelLog : ILog
        {
            private readonly ILog baseLog;
            private readonly LogLevel minLevel;

            public FilterByLevelLog(ILog baseLog, LogLevel minLevel)
            {
                this.baseLog = baseLog;
                this.minLevel = minLevel;
            }

            public void Log(LogEvent @event)
            {
                // TODO(krait): Allow nulls?
                if (@event.Level >= minLevel)
                    baseLog.Log(@event);
            }

            public bool IsEnabledFor(LogLevel level)
            {
                return level >= minLevel && baseLog.IsEnabledFor(level);
            }

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);
                return baseLogForContext == baseLog ? this : new FilterByLevelLog(baseLogForContext, minLevel);
            }
        }
    }
}