using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Vostok.Logging.Abstractions;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    public class FilterBySourceContextLogExtensions_Tests
    {
        private ILog baseLog;
        private LogEvent @event;
        private ILog forContextBaseLog;
        private string context;

        [SetUp]
        public void TestSetup()
        {
            baseLog = Substitute.For<ILog>();
            forContextBaseLog = Substitute.For<ILog>();
            baseLog.ForContext(Arg.Any<string>()).Returns(info => forContextBaseLog);
            
            @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);
            context = "TestContext";
        }

        [Test]
        public void SelectEventsBySourceContext_should_return_a_log_that_select_events_with_context()
        {
            var filterLog = baseLog.WithEventsSelectedBySourceContext(context);
            
            filterLog.ForContext(context).Log(@event);

            forContextBaseLog.Received(1).Log(@event);
        }

        [Test]
        public void SelectEventsBySourceContext_should_return_a_log_that_drop_events_without_context()
        {
            var filterLog = baseLog.WithEventsSelectedBySourceContext(context);
            
            filterLog.ForContext("DifferentContext").Log(@event);

            forContextBaseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void DropEventsBySourceContext_should_return_a_log_that_drop_events_with_context()
        {
            var filterLog = baseLog.WithEventsDroppedBySourceContext(context);
            
            filterLog.ForContext(context).Log(@event);

            forContextBaseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void DropEventsBySourceContext_should_return_a_log_that_select_events_without_context()
        {
            var filterLog = baseLog.WithEventsDroppedBySourceContext(context);
            
            filterLog.ForContext("DifferentContext").Log(@event);

            forContextBaseLog.Received(1).Log(@event);
        }
    }
}