namespace Vostok.Logging.Abstractions
{
    /// <summary>
    /// <list type="bullet">
    ///     <listheader>Recommended usage:</listheader>
    ///     <item><see cref="Debug"/> — for verbose output; this log level should usually be ignored on production installations.</item>
    ///     <item><see cref="Info"/> — for neutral messages.</item>
    ///     <item><see cref="Warn"/> — for non-critical errors that don't interrupt the normal operation of the application.</item>
    ///     <item><see cref="Error"/> — for unexpected errors that may require human attention.</item>
    ///     <item><see cref="Fatal"/> — for critical errors that result in application shutdown.</item>
    /// </list>
    /// </summary>
    public enum LogLevel
    {
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}
