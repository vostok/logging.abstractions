using Vostok.Logging.Formatting;

namespace Vostok.Logging.Abstractions.Tests;

internal class FormattingLog : ILog
{
    public string LastLog;
    private readonly OutputTemplate template;

    public FormattingLog()
    {
        template = OutputTemplate.Create()
            .AddLevel()
            .AddMessage()
            .AddNewline()
            .Build();
    }
        
    public void Log(LogEvent @event)
    {
        LastLog = LogEventFormatter.Format(@event!, template);
    }

    public bool IsEnabledFor(LogLevel level) => true;

    public ILog ForContext(string context) => this;
}