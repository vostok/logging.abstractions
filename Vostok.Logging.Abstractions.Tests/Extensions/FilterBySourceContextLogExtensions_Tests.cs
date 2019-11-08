using System;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    public class FilterBySourceContextLogExtensions_Tests
    {
        private ILog baseLog;

        private LogEvent @event;
        private string context;
        private string differentContext;

        [SetUp]
        public void TestSetup()
        {
            baseLog = Substitute.For<ILog>();
            baseLog.ForContext(Arg.Any<string>()).Returns(info => baseLog);

            @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);
            context = "TestContext";
            differentContext = "DifferentContext";
        }

        [Test]
        public void WithEventsSelectedBySourceContext_should_return_a_log_that_select_events_with_context()
        {
            var filterLog = baseLog.WithEventsSelectedBySourceContext(context);

            filterLog.ForContext(context).Log(@event);
            filterLog.ForContext(context).ForContext(differentContext).Log(@event);
            filterLog.ForContext(differentContext).ForContext(context).ForContext(differentContext).Log(@event);

            baseLog.Received(3).Log(@event);
        }

        [Test]
        public void WithEventsSelectedBySourceContext_should_return_a_log_that_drop_events_without_context()
        {
            var filterLog = baseLog.WithEventsSelectedBySourceContext(context);

            filterLog.Log(@event);
            filterLog.ForContext(differentContext).Log(@event);

            baseLog.Received(0).Log(@event);
        }

        [Test]
        public void WithEventsDroppedBySourceContext_should_return_a_log_that_drop_events_with_context()
        {
            var filterLog = baseLog.WithEventsDroppedBySourceContext(context);

            filterLog.ForContext(context).Log(@event);
            filterLog.ForContext(context).ForContext(differentContext).Log(@event);
            filterLog.ForContext(differentContext).ForContext(context).Log(@event);

            baseLog.Received(0).Log(@event);
        }

        [Test]
        public void WithEventsDroppedBySourceContext_should_return_a_log_that_select_events_without_context()
        {
            var filterLog = baseLog.WithEventsDroppedBySourceContext(context);

            filterLog.Log(@event);
            filterLog.ForContext(differentContext).Log(@event);
            filterLog.ForContext(differentContext).ForContext("AnotherDifferentContext").Log(@event);

            baseLog.Received(3).Log(@event);
        }
    }
}