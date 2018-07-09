using System.Linq;

namespace Vostok.Logging.Abstractions
{
    public class CompositeLog : ILog
    {
        private readonly ILog[] baseLogs;

        public CompositeLog(params ILog[] baseLogs)
        {
            this.baseLogs = baseLogs;
        }

        public void Log(LogEvent @event)
        {
            foreach (var baseLog in baseLogs)
                baseLog.Log(@event);
        }

        public bool IsEnabledFor(LogLevel level)
        {
            return baseLogs.Any(x => x.IsEnabledFor(level));
        }

        public ILog ForContext(string context)
        {
            var baseLogsForContext = new ILog[baseLogs.Length];
            var sameContext = true;
            for (int i = 0; i < baseLogs.Length; i++)
            {
                baseLogsForContext[i] = baseLogs[i].ForContext(context);
                if (baseLogsForContext[i] != baseLogs[i])
                    sameContext = false;
            }
            return sameContext ? this : new CompositeLog(baseLogsForContext);
        }
    }
}