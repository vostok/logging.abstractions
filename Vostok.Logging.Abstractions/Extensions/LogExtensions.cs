using System;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    public static class LogExtensions
    {
        #region Debug

        /// <summary>
        /// Logs the given <paramref name="message"/> on the <see cref="LogLevel.Debug"/> level without any additional properties.
        /// </summary>
        public static void Debug(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Debug"/> level without a message or any additional properties.
        /// </summary>
        public static void Debug(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Debug"/> level without any additional properties.
        /// </summary>
        public static void Debug(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Debug"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Debug<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Debug(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Debug"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Debug(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Debug"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Debug<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Debug(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Debug"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Debug(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Debug(ILog, Exception, string) overload instead.")]
        public static void Debug(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Debug(exception, message);
        }

        #endregion

        #region Info

        /// <summary>
        /// Logs the given <paramref name="message"/> on the <see cref="LogLevel.Info"/> level without any additional properties.
        /// </summary>
        public static void Info(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Info"/> level without a message or any additional properties.
        /// </summary>
        public static void Info(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Info"/> level without any additional properties.
        /// </summary>
        public static void Info(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Info"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Info<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Info(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Info"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Info(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Info"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Info<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Info(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Info"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Info(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Info(ILog, Exception, string) overload instead.")]
        public static void Info(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Info(exception, message);
        }

        #endregion

        #region Warn

        /// <summary>
        /// Logs the given <paramref name="message"/> on the <see cref="LogLevel.Warn"/> level without any additional properties.
        /// </summary>
        public static void Warn(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Warn"/> level without a message or any additional properties.
        /// </summary>
        public static void Warn(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Warn"/> level without any additional properties.
        /// </summary>
        public static void Warn(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Warn"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Warn<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Warn(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Warn"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Warn(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Warn"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Warn<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Warn(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Warn"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Warn(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Warn(ILog, Exception, string) overload instead.")]
        public static void Warn(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Warn(exception, message);
        }

        #endregion

        #region Error

        /// <summary>
        /// Logs the given <paramref name="message"/> on the <see cref="LogLevel.Error"/> level without any additional properties.
        /// </summary>
        public static void Error(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Error"/> level without a message or any additional properties.
        /// </summary>
        public static void Error(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Error"/> level without any additional properties.
        /// </summary>
        public static void Error(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Error"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Error<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Error(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Error"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Error(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Error"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Error<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Error(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Error"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Error(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Error(ILog, Exception, string) overload instead.")]
        public static void Error(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Error(exception, message);
        }

        #endregion

        #region Fatal

        /// <summary>
        /// Logs the given <paramref name="message"/> on the <see cref="LogLevel.Fatal"/> level without any additional properties.
        /// </summary>
        public static void Fatal(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Fatal"/> level without a message or any additional properties.
        /// </summary>
        public static void Fatal(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Fatal"/> level without any additional properties.
        /// </summary>
        public static void Fatal(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Fatal"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Fatal<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Fatal(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Fatal"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Fatal(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Fatal"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Fatal<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Fatal(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Fatal"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEventFormatter"/> for details.
        /// </summary>
        [StringFormatMethod("messageTemplate")]
        public static void Fatal(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Fatal(ILog, Exception, string) overload instead.")]
        public static void Fatal(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Fatal(exception, message);
        }

        #endregion

    }
}