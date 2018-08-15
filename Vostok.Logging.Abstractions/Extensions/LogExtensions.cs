using System;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
	[PublicAPI]
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

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.Now, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Debug"/> level without a message or any additional properties.
        /// </summary>
        public static void Debug(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.Now, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Debug"/> level without any additional properties.
        /// </summary>
        public static void Debug(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.Now, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Debug"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Debug<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Debug(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.Now, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Debug"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Debug(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.Now, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Debug"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Debug<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Debug(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.Now, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Debug"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Debug(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.Now, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Debug(ILog, Exception, string) overload instead.")]
        public static void Debug(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Debug(exception, message);
        }

        /// <summary>
        /// Returns true if given <paramref name="log"/> is enabled to log events of <see cref="LogLevel.Debug"/> level, or false otherwise.
        /// </summary>
        public static bool IsEnabledForDebug(this ILog log)
        {
            return log.IsEnabledFor(LogLevel.Debug);
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

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.Now, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Info"/> level without a message or any additional properties.
        /// </summary>
        public static void Info(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.Now, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Info"/> level without any additional properties.
        /// </summary>
        public static void Info(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.Now, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Info"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Info<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Info(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.Now, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Info"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Info(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.Now, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Info"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Info<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Info(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.Now, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Info"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Info(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.Now, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Info(ILog, Exception, string) overload instead.")]
        public static void Info(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Info(exception, message);
        }

        /// <summary>
        /// Returns true if given <paramref name="log"/> is enabled to log events of <see cref="LogLevel.Info"/> level, or false otherwise.
        /// </summary>
        public static bool IsEnabledForInfo(this ILog log)
        {
            return log.IsEnabledFor(LogLevel.Info);
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

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.Now, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Warn"/> level without a message or any additional properties.
        /// </summary>
        public static void Warn(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.Now, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Warn"/> level without any additional properties.
        /// </summary>
        public static void Warn(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.Now, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Warn"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Warn<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Warn(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.Now, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Warn"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Warn(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.Now, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Warn"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Warn<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Warn(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.Now, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Warn"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Warn(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.Now, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Warn(ILog, Exception, string) overload instead.")]
        public static void Warn(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Warn(exception, message);
        }

        /// <summary>
        /// Returns true if given <paramref name="log"/> is enabled to log events of <see cref="LogLevel.Warn"/> level, or false otherwise.
        /// </summary>
        public static bool IsEnabledForWarn(this ILog log)
        {
            return log.IsEnabledFor(LogLevel.Warn);
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

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.Now, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Error"/> level without a message or any additional properties.
        /// </summary>
        public static void Error(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.Now, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Error"/> level without any additional properties.
        /// </summary>
        public static void Error(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.Now, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Error"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Error<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Error(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.Now, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Error"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Error(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.Now, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Error"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Error<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Error(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.Now, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Error"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Error(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.Now, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Error(ILog, Exception, string) overload instead.")]
        public static void Error(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Error(exception, message);
        }

        /// <summary>
        /// Returns true if given <paramref name="log"/> is enabled to log events of <see cref="LogLevel.Error"/> level, or false otherwise.
        /// </summary>
        public static bool IsEnabledForError(this ILog log)
        {
            return log.IsEnabledFor(LogLevel.Error);
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

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.Now, message));
        }

        /// <summary>
        /// Logs the given <paramref name="exception"/> on the <see cref="LogLevel.Fatal"/> level without a message or any additional properties.
        /// </summary>
        public static void Fatal(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.Now, null, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="message"/> and <paramref name="exception"/> on the <see cref="LogLevel.Fatal"/> level without any additional properties.
        /// </summary>
        public static void Fatal(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.Now, message, exception));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Fatal"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Fatal<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Fatal(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.Now, messageTemplate).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> on the <see cref="LogLevel.Fatal"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Fatal(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.Now, messageTemplate).WithParameters(parameters));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Fatal"/> level with given <paramref name="properties" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="properties"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Fatal<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            if (!typeof(T).IsConstructedGenericType)
            {
                log.Fatal(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.Now, messageTemplate, exception).WithObjectProperties(properties));
        }

        /// <summary>
        /// Logs the given <paramref name="messageTemplate"/> and <paramref name="exception"/> on the <see cref="LogLevel.Fatal"/> level with given <paramref name="parameters" />. The <paramref name="messageTemplate"/> can contain placeholders for <paramref name="parameters"/>, see <see cref="LogEvent.MessageTemplate"/> for details.
        /// </summary>
        public static void Fatal(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.Now, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete("Use the Fatal(ILog, Exception, string) overload instead.")]
        public static void Fatal(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Fatal(exception, message);
        }

        /// <summary>
        /// Returns true if given <paramref name="log"/> is enabled to log events of <see cref="LogLevel.Fatal"/> level, or false otherwise.
        /// </summary>
        public static bool IsEnabledForFatal(this ILog log)
        {
            return log.IsEnabledFor(LogLevel.Fatal);
        }

        #endregion

    }
}