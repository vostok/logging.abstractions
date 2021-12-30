#if NET6_0

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Vostok.Commons.Collections;

namespace Vostok.Logging.Abstractions.Helpers
{
    [InterpolatedStringHandler]
    public ref struct InterpolatedStringHandler
    {
        internal bool IsEnabled { get; }
        internal StringBuilder MessageTemplate { get; } = null!;
        internal ImmutableArrayDictionary<string, object> Properties { get; private set; } = null!;

        public InterpolatedStringHandler(int literalLength, int formattedCount, ILog log, out bool isEnabled)
        {
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

        public void AppendFormatted<T>(T value, [CallerArgumentExpression("value")] string name = "")
        {
            MessageTemplate.Append('{');
            MessageTemplate.Append(name);
            MessageTemplate.Append('}');

            Properties = Properties.Set(name, value);
        }
    }
}

#endif