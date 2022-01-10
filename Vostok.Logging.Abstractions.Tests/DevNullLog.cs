namespace Vostok.Logging.Abstractions.Tests
{
    internal class DevNullLog : ILog
    {
        public LogEvent LastEvent;

        public void Log(LogEvent @event)
        {
            LastEvent = @event;
        }

        public bool IsEnabledFor(LogLevel level) => true;

        public ILog ForContext(string context) => this;
    }
}