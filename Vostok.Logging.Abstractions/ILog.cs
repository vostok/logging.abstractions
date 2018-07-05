using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    public interface ILog
    {
        void Log([CanBeNull] LogEvent @event);

        bool IsEnabledFor(LogLevel level);
    }
}