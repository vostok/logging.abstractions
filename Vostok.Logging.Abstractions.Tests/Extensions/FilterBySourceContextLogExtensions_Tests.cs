using System;
using System.Linq;
using FluentAssertions;
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
            baseLog.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);

            @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);
            context = "TestContext";
            differentContext = "DifferentContext";
        }

        [Test]
        public void WithEventsSelectedBySourceContext_should_return_a_log_that_selects_events_with_context()
        {
            var filterLog = baseLog.WithEventsSelectedBySourceContext(context);

            filterLog.ForContext(context).Log(@event);
            filterLog.ForContext(context).ForContext(differentContext).Log(@event);
            filterLog.ForContext(differentContext).ForContext(context).ForContext(differentContext).Log(@event);

            baseLog.Received(3).Log(@event);
        }

        [Test]
        public void WithEventsSelectedBySourceContext_should_return_a_log_that_drops_events_without_context()
        {
            var filterLog = baseLog.WithEventsSelectedBySourceContext(context);

            filterLog.Log(@event);
            filterLog.ForContext(differentContext).Log(@event);

            baseLog.Received(0).Log(@event);
        }

        [Test]
        public void WithEventsDroppedBySourceContext_should_return_a_log_that_drops_events_with_context()
        {
            var filterLog = baseLog.WithEventsDroppedBySourceContext(context);

            filterLog.ForContext(context).Log(@event);
            filterLog.ForContext(context).ForContext(differentContext).Log(@event);
            filterLog.ForContext(differentContext).ForContext(context).Log(@event);

            baseLog.Received(0).Log(@event);
        }

        [Test]
        public void WithEventsDroppedBySourceContext_should_return_a_log_that_selects_events_without_context()
        {
            var filterLog = baseLog.WithEventsDroppedBySourceContext(context);

            filterLog.Log(@event);
            filterLog.ForContext(differentContext).Log(@event);
            filterLog.ForContext(differentContext).ForContext("AnotherDifferentContext").Log(@event);

            baseLog.Received(3).Log(@event);
        }

        [Test]
        public void WithMinimumLevelForSourceContext_should_return_a_log_that_logs_all_events_with_different_contexts()
        {
            var filterLog = baseLog.WithMinimumLevelForSourceContext(context, LogLevel.Warn).ForContext(differentContext);

            ShouldBeEnabledFor(filterLog, GetAllLevels());
        }

        [Test]
        public void WithMinimumLevelForSourceContext_should_return_a_log_that_logs_all_events_with_given_contexts_and_permitted_levels()
        {
            var filterLog1 = baseLog.WithMinimumLevelForSourceContext(context, LogLevel.Warn)
                .ForContext(context);

            var filterLog2 = baseLog.WithMinimumLevelForSourceContext(context, LogLevel.Warn)
                .ForContext(differentContext)
                .ForContext(context)
                .ForContext(differentContext);

            ShouldBeEnabledFor(filterLog1, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal);
            ShouldBeEnabledFor(filterLog2, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal);

            ShouldBeDisabledFor(filterLog1, LogLevel.Debug, LogLevel.Info);
            ShouldBeDisabledFor(filterLog2, LogLevel.Debug, LogLevel.Info);
        }

        private void ShouldBeEnabledFor(ILog filterLog, params LogLevel[] levels)
        {
            foreach (var level in levels)
            {
                var newEvent = @event.WithLevel(level);

                baseLog.ClearReceivedCalls();

                filterLog.IsEnabledFor(level).Should().BeTrue();
                filterLog.Log(newEvent);

                baseLog.Received(1).Log(newEvent);
            }
        }

        private void ShouldBeDisabledFor(ILog filterLog, params LogLevel[] levels)
        {
            foreach (var level in levels)
            {
                baseLog.ClearReceivedCalls();

                filterLog.IsEnabledFor(level).Should().BeFalse();
                filterLog.Log(@event.WithLevel(level));

                baseLog.ReceivedCalls().Should().BeEmpty();
            }
        }

        private static LogLevel[] GetAllLevels()
            => Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToArray();
    }
}