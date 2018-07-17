namespace Vostok.Logging.Abstractions
{
    public class SilentLog : ILog
    {
        public void Log(LogEvent @event)
        {
        }

        public bool IsEnabledFor(LogLevel level) => false;

        public ILog ForContext(string context) => this;
    }
}
