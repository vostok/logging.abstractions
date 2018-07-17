namespace Vostok.Logging.Abstractions
{
    public static class WithPropertyLogExtensions
    {
        /// <summary>
        /// Returns a wrapper log that adds the given property to each <see cref="LogEvent"/> before logging.
        /// </summary>
        public static ILog WithProperty<T>(this ILog log, string key, T value)
        {
            return new WithPropertyLog<T>(log, key, value);
        }

        private class WithPropertyLog<T> : ILog
        {
            private readonly ILog baseLog;
            private readonly string key;
            private readonly T value;

            public WithPropertyLog(ILog baseLog, string key, T value)
            {
                this.baseLog = baseLog;
                this.key = key;
                this.value = value;
            }

            public void Log(LogEvent @event)
            {
                baseLog.Log(@event?.WithProperty(key, value));
            }

            public bool IsEnabledFor(LogLevel level)
            {
                return baseLog.IsEnabledFor(level);
            }

            public ILog ForContext(string context)
            {
                var baseLogForContext = baseLog.ForContext(context);
                return baseLogForContext == baseLog ? this : new WithPropertyLog<T>(baseLogForContext, key, value);
            }
        }
    }
}
