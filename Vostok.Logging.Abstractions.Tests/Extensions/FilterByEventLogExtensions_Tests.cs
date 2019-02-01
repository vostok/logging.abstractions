using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class FilterByEventLogExtensions_Tests
    {
        private ILog baseLog;
        private ILog filteredLog;
        private LogEvent @event;

        [SetUp]
        public void TestSetup()
        {
            baseLog = Substitute.For<ILog>();

            @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);
        }
        
        [Test]
        public void SelectEvents_should_return_a_log_that_passes_events_to_predicate()
        {
            LogEvent observedEvent = null;

            filteredLog = baseLog.SelectEvents(
                e =>
                {
                    observedEvent = e;
                    return true;
                });

            filteredLog.Log(@event);

            observedEvent.Should().BeSameAs(@event);
        }

        [Test]
        public void SelectEvents_should_return_a_log_that_drops_events_not_matched_by_the_predicate()
        {
            filteredLog = baseLog.SelectEvents(_ => false);

            filteredLog.Log(@event);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void SelectEvents_should_return_a_log_that_forwards_events_matched_by_the_predicate()
        {
            filteredLog = baseLog.SelectEvents(_ => true);

            filteredLog.Log(@event);

            baseLog.Received(1).Log(@event);
        }

        [Test]
        public void DropEvents_should_return_a_log_that_passes_events_to_predicate()
        {
            LogEvent observedEvent = null;

            filteredLog = baseLog.DropEvents(
                e =>
                {
                    observedEvent = e;
                    return true;
                });

            filteredLog.Log(@event);

            observedEvent.Should().BeSameAs(@event);
        }

        [Test]
        public void DropEvents_should_return_a_log_that_forwards_events_not_matched_by_the_predicate()
        {
            filteredLog = baseLog.DropEvents(_ => false);

            filteredLog.Log(@event);

            baseLog.Received(1).Log(@event);
        }

        [Test]
        public void DropEvents_should_return_a_log_that_drops_events_matched_by_the_predicate()
        {
            filteredLog = baseLog.DropEvents(_ => true);

            filteredLog.Log(@event);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }
    }
}