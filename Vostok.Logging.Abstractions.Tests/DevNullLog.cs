namespace Vostok.Logging.Abstractions.Tests
{
    internal class DevNullLog : ILog
    {
        private LogEvent lastEvent;

        public void Log(LogEvent @event)
        {
            lastEvent = @event;
        }

        public bool IsEnabledFor(LogLevel level) => true;

        public ILog ForContext(string context) => this;
    }
}