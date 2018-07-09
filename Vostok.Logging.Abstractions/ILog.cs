using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    /// <summary>
    /// <para>Represents a log.</para>
    /// <para>Implementations are expected to be thread-safe and never throw exceptions in any method.</para>
    /// </summary>
    public interface ILog
    {
        /// <summary>
        /// Logs the given <see cref="LogEvent"/>. This method should not be called directly in most cases. 
        /// Use one of the <see cref="LogExtensions.Debug(ILog,string)"/>, <see cref="LogExtensions.Info(ILog,string)"/>, <see cref="LogExtensions.Warn(ILog,string)"/>, <see cref="LogExtensions.Error(ILog,string)"/> or <see cref="LogExtensions.Fatal(ILog,string)"/> extension methods instead.
        /// </summary>
        void Log([CanBeNull] LogEvent @event);

        /// <summary>
        /// <para>Returns whether the current log is configured to log events of the given <see cref="LogLevel"/>.</para>
        /// <para>In case you use the <see cref="Log"/> method directly, call this method to avoid unnecessary construction of <see cref="LogEvent"/>s.</para>
        /// </summary>
        bool IsEnabledFor(LogLevel level);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        ILog ForContext(string context);
    }
}