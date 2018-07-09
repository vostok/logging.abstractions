namespace Vostok.Logging.Abstractions
{
    public static class WithPropertyLogExtensions
    {
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
                baseLog.Log(@event.WithProperty(key, value));
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