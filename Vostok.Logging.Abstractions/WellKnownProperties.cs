using JetBrains.Annotations;
using Vostok.Logging.Abstractions.Values;

namespace Vostok.Logging.Abstractions
{
    /// <summary>
    /// Defines names of special well-known properties in <see cref="LogEvent"/>s.
    /// </summary>
    [PublicAPI]
    public static class WellKnownProperties
    {
        /// <summary>
        /// <para>Property that denotes logging events source, such as a class in code base.</para>
        /// <para>Represented by <see cref="SourceContextValue"/>.</para>
        /// </summary>
        public const string SourceContext = "SourceContext";

        public const string ContextualPrefix = "ContextualPrefix";
    }
}