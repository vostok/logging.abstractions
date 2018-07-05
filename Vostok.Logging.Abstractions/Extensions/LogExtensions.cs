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

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, null, exception: exception));
        }

        public static void Debug(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, message, exception: exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Debug<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Debug(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Debug(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Debug<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Debug(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary(), exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Debug(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Debug))
                return;

            log.Log(new LogEvent(LogLevel.Debug, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary(), exception));
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

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, null, exception: exception));
        }

        public static void Info(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, message, exception: exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Info<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Info(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Info(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Info<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Info(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary(), exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Info(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Info))
                return;

            log.Log(new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary(), exception));
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

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, null, exception: exception));
        }

        public static void Warn(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, message, exception: exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Warn<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Warn(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Warn(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Warn<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Warn(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary(), exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Warn(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Warn))
                return;

            log.Log(new LogEvent(LogLevel.Warn, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary(), exception));
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

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, null, exception: exception));
        }

        public static void Error(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, message, exception: exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Error<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Error(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Error(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Error<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Error(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary(), exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Error(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Error))
                return;

            log.Log(new LogEvent(LogLevel.Error, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary(), exception));
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

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, null, exception: exception));
        }

        public static void Fatal(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string message)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, message, exception: exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Fatal<T>(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Fatal(messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Fatal(this ILog log, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary()));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Fatal<T>(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] T properties)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            if (!properties.GetType().IsConstructedGenericType)
            {
                log.Fatal(exception, messageTemplate, (object)properties);
                return;
            }

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate, properties.ToDictionary(), exception));
        }

        [StringFormatMethod("messageTemplate")]
        public static void Fatal(this ILog log, [CanBeNull] Exception exception, [CanBeNull] string messageTemplate, [CanBeNull] params object[] parameters)
        {
            if (!log.IsEnabledFor(LogLevel.Fatal))
                return;

            log.Log(new LogEvent(LogLevel.Fatal, DateTimeOffset.UtcNow, messageTemplate, parameters.ToDictionary(), exception));
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