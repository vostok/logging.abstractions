using JetBrains.Annotations;

namespace Vostok.Logging.Abstractions
{
    /// <summary>
    /// Defines names of special well-known properties in <see cref="LogEvent"/>s.
    /// </summary>
    [PublicAPI]
    public static class WellKnownProperties
    {
        public const string SourceContext = "SourceContext";

        public const string ContextualPrefix = "ContextualPrefix";
    }
}