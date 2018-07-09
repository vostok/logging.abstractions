using System;
using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    public static class LogExtensions
    {
        #region Debug

        public static void Debug(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, message));
        }

        public static void Debug(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, null, exception));
        }

        public static void Debug(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, message, exception));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Debug(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Debug(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete]
        public static void Debug(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Debug(exception, message);
        }

        #endregion

        #region Info

        public static void Info(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, message));
        }

        public static void Info(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, null, exception));
        }

        public static void Info(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, message, exception));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Info(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Info(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete]
        public static void Info(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Info(exception, message);
        }

        #endregion

        #region Warn

        public static void Warn(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, message));
        }

        public static void Warn(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, null, exception));
        }

        public static void Warn(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, message, exception));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Warn(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Warn(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete]
        public static void Warn(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Warn(exception, message);
        }

        #endregion

        #region Error

        public static void Error(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, message));
        }

        public static void Error(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, null, exception));
        }

        public static void Error(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, message, exception));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Error(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Error(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete]
        public static void Error(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Error(exception, message);
        }

        #endregion

        #region Fatal

        public static void Fatal(this ILog log, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, message));
        }

        public static void Fatal(this ILog log, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, null, exception));
        }

        public static void Fatal(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, message, exception));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Fatal(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate).WithParameters(parameters));
        }

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

        [StringFormatMethod("messageTemplate")]
        public static void Fatal(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate, exception).WithParameters(parameters));
        }

        [Obsolete]
        public static void Fatal(this ILog log, [CanBeNull] string message, [CanBeNull] Exception exception)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Fatal(exception, message);
        }

        #endregion

    }
}