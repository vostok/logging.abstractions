using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Vostok.Commons.Collections;

namespace Vostok.Logging.Abstractions
{
    /// <summary>
    /// A single event to be logged. Consists of a timestamp, a log message, a saved exception and user-defined properties.
    /// </summary>
    [PublicAPI]
    public sealed class LogEvent
    {
        [CanBeNull]
        private readonly ImmutableArrayDictionary<string, object> properties;

        /// <summary>
        /// Creates a new log event with specified <paramref name="level"/>, <paramref name="timestamp"/>, <paramref name="messageTemplate"/>, <paramref name="exception"/> and empty properties.
        /// </summary>
        public LogEvent(LogLevel level, DateTimeOffset timestamp, [CanBeNull] string messageTemplate, [CanBeNull] Exception exception = null)
            : this(level, timestamp, messageTemplate, null, exception)
        {
        }

        internal LogEvent(LogLevel level, DateTimeOffset timestamp, string messageTemplate, ImmutableArrayDictionary<string, object> properties, Exception exception)
        {
            Level = level;
            Timestamp = timestamp;
            MessageTemplate = messageTemplate;
            Exception = exception;

            this.properties = properties;
        }

        /// <summary>
        /// The <see cref="LogLevel"/> of the event. See <see cref="LogLevel"/> enumeration for tips on when to use which log level.
        /// </summary>
        public LogLevel Level { get; }

        /// <summary>
        /// The timestamp of the event. Represents the time when the event was created, rather then the time when it was written to a file or processed in any other way. The local time zone is kept here for future use. 
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// <para>The template of the log message containing placeholders to be filled with values from <see cref="Properties"/>.</para>
        /// <para>For example, the template "foo{0} {key}" and properties { '0': 'bar', 'key': 'baz' } produce the following output: "foobar baz".</para>
        /// <para>Use double curly braces to escape curly braces in text: "{{key}}", { 'key': 'value' } --> "{{key}}".</para>
        /// <para>Any mismatched braces are kept as-is: "key1}", { 'key1': 'value' } --> "key1}".</para>
        /// <para>Any nonexistent keys are rendered as empty strings: "{key1}", { } --> "".</para>
        /// <para>Can be null for events containing only <see cref="Exception"/>.</para>
        /// </summary>
        [CanBeNull]
        public string MessageTemplate { get; }

        /// <summary>
        /// <para>Contains various user-defined properties of the event.</para>
        /// <list type="bullet">
        ///     <listheader>There are two kinds of properties:</listheader>
        ///     <item>Named properties. These should be set using logging extensions with the 'properties' argument like <see cref="LogExtensions.Info{T}(ILog,string,T)"/>.</item> 
        ///     <item>Positional parameters. These should be set using logging extensions with the 'parameters' argument like <see cref="LogExtensions.Info(ILog,string,object[])"/> and are then referenced by their position in the array instead of name.</item>
        /// </list>
        /// <para>Both kinds of properties can be substituted into the <see cref="MessageTemplate"/>. See <see cref="MessageTemplate"/> for details.</para>
        /// <para>Can be null if there is no properties.</para>
        /// </summary>
        [CanBeNull]
        public IReadOnlyDictionary<string, object> Properties => properties;

        /// <summary>
        /// The error associated with this log event. Can be null if there is no error.
        /// </summary>
        [CanBeNull]
        public Exception Exception { get; }

        /// <summary>
        /// <para>Returns a copy of the log event with property <paramref name="key"/> set to <paramref name="value"/>. </para>
        /// <para>Existing properties can be overwritten this way.</para>
        /// </summary>
        [Pure]
        public LogEvent WithProperty<T>([NotNull] string key, [CanBeNull] T value)
            => WithProperty(key, value, true);

        /// <summary>
        /// <para>Returns a copy of the log event with property <paramref name="key"/> set to <paramref name="value"/>. </para>
        /// <para>Existing properties can not be overwritten this way: the same <see cref="LogEvent"/> is returned upon conflict.</para>
        /// </summary>
        [Pure]
        public LogEvent WithPropertyIfAbsent<T>([NotNull] string key, [CanBeNull] T value)
            => WithProperty(key, value, false);

        /// <summary>
        /// Returns a copy of the log event with property <paramref name="key"/> removed.
        /// </summary>
        [Pure]
        public LogEvent WithoutProperty([NotNull] string key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var newProperties = properties?.Remove(key);

            if (ReferenceEquals(newProperties, properties))
                return this;

            return new LogEvent(Level, Timestamp, MessageTemplate, newProperties, Exception);
        }

        /// <summary>
        /// Returns a copy of the log event with given <paramref name="messageTemplate"/>.
        /// </summary>
        [Pure]
        public LogEvent WithMessageTemplate([CanBeNull] string messageTemplate)
            => new LogEvent(Level, Timestamp, messageTemplate, properties, Exception);

        /// <summary>
        /// Returns a copy of the log event with given <paramref name="exception"/>.
        /// </summary>
        [Pure]
        public LogEvent WithException([CanBeNull] Exception exception)
            => new LogEvent(Level, Timestamp, MessageTemplate, properties, exception);

        /// <summary>
        /// Returns a copy of the log event with given <paramref name="level"/>.
        /// </summary>
        [Pure]
        public LogEvent WithLevel(LogLevel level)
            => new LogEvent(level, Timestamp, MessageTemplate, properties, Exception);

        /// <summary>
        /// Returns a copy of the log event with given <paramref name="timestamp"/>.
        /// </summary>
        [Pure]
        public LogEvent WithTimestamp(DateTimeOffset timestamp)
            => new LogEvent(Level, timestamp, MessageTemplate, properties, Exception);

        internal LogEvent WithProperty<T>([NotNull] string key, [CanBeNull] T value, bool allowOverwrite)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            var newProperties = properties == null
                ? CreateProperties().Set(key, value)
                : properties.Set(key, value, allowOverwrite);

            if (ReferenceEquals(newProperties, properties))
                return this;

            return new LogEvent(Level, Timestamp, MessageTemplate, newProperties, Exception);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ImmutableArrayDictionary<string, object> CreateProperties()
            => new ImmutableArrayDictionary<string, object>(StringComparer.Ordinal);
    }
}
