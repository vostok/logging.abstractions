#if NET6_0

using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text;
using Vostok.Commons.Collections;

namespace Vostok.Logging.Abstractions.Interpolated
{
    [InterpolatedStringHandler]
    public ref struct InterpolatedStringHandler
    {
        public InterpolatedStringHandler(int literalLength, int formattedCount, ILog log, out bool isEnabled, IFormatProvider formatProvider = null)
        {
            // todo (kungurtsev, 30.12.2021): copy-paste handler for each LogLevel?
            IsEnabled = isEnabled = log.IsEnabledFor(LogLevel.Info);
            if (!isEnabled)
                return;
            
            MessageTemplate = new StringBuilder(literalLength);

            if (formattedCount > 0)
                Properties = LogEvent.CreateProperties(Math.Max(4, formattedCount));
        }

        public void AppendLiteral(string value)
        {
            MessageTemplate.Append(value);
        }

        public void AppendFormatted<T>(T value, int alignment, [CallerArgumentExpression("value")] string name = "")
        {
            var defaultHandler = CreateDefaultHandler();
            defaultHandler.AppendFormatted(value, alignment);
            AppendFormatted((object)defaultHandler.ToStringAndClear(), name);
        }
        
        public void AppendFormatted<T>(T value, string format, [CallerArgumentExpression("value")] string name = "")
        {
            var defaultHandler = CreateDefaultHandler();
            defaultHandler.AppendFormatted(value, format);
            AppendFormatted((object)defaultHandler.ToStringAndClear(), name);
        }

        public void AppendFormatted<T>(T value, int alignment, string format, [CallerArgumentExpression("value")] string name = "")
        {
            var defaultHandler = CreateDefaultHandler();
            defaultHandler.AppendFormatted(value, alignment, format);
            AppendFormatted((object)defaultHandler.ToStringAndClear(), name);
        }

        public void AppendFormatted(object value, [CallerArgumentExpression("value")] string name = "")
        {
            MessageTemplate.Append('{');
            MessageTemplate.Append(name);
            MessageTemplate.Append('}');

            Properties = Properties.Set(name, value);
        }

        internal bool IsEnabled { get; }
        internal StringBuilder MessageTemplate { get; } = null!;
        internal ImmutableArrayDictionary<string, object> Properties { get; private set; } = null!;
        
        private DefaultInterpolatedStringHandler CreateDefaultHandler() =>
            new DefaultInterpolatedStringHandler(0, 1, CultureInfo.InvariantCulture);
    }
}

#endif