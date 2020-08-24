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
        private const string Context = "TestContext";
        private const string ContextPrefix = "test";
        private const string DifferentContext = "DifferentContext";
        private const string DifferentContextPrefix = "different";

        private ILog baseLog;
        private LogEvent @event;

        [SetUp]
        public void TestSetup()
        {
            baseLog = Substitute.For<ILog>();
            baseLog.ForContext(Arg.Any<string>()).Returns(info => baseLog);
            baseLog.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);

            @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);
        }

        [TestCase(Context)]
        [TestCase(ContextPrefix)]
        public void WithEventsSelectedBySourceContext_should_return_a_log_that_selects_events_with_context(string filterValue)
        {
            var filterLog = baseLog.WithEventsSelectedBySourceContext(filterValue);

            filterLog.ForContext(Context).Log(@event);
            filterLog.ForContext(Context).ForContext(DifferentContext).Log(@event);
            filterLog.ForContext(DifferentContext).ForContext(Context).ForContext(DifferentContext).Log(@event);

            baseLog.Received(3).Log(Arg.Any<LogEvent>());
        }

        [TestCase(Context)]
        [TestCase(ContextPrefix)]
        public void WithEventsSelectedBySourceContext_should_return_a_log_that_drops_events_without_context(string filterValue)
        {
            var filterLog = baseLog.WithEventsSelectedBySourceContext(filterValue);

            filterLog.Log(@event);
            filterLog.ForContext(DifferentContext).Log(@event);

            baseLog.Received(0).Log(@event);
        }

        [TestCase(Context)]
        [TestCase(ContextPrefix)]
        public void WithEventsDroppedBySourceContext_should_return_a_log_that_drops_events_with_context(string filterValue)
        {
            var filterLog = baseLog.WithEventsDroppedBySourceContext(filterValue);

            filterLog.ForContext(Context).Log(@event);
            filterLog.ForContext(Context).ForContext(DifferentContext).Log(@event);
            filterLog.ForContext(DifferentContext).ForContext(Context).Log(@event);

            baseLog.Received(0).Log(@event);
        }

        [TestCase(Context)]
        [TestCase(ContextPrefix)]
        public void WithEventsDroppedBySourceContext_should_return_a_log_that_selects_events_without_context(string filterValue)
        {
            var filterLog = baseLog.WithEventsDroppedBySourceContext(filterValue);

            filterLog.Log(@event);
            filterLog.ForContext(DifferentContext).Log(@event);
            filterLog.ForContext(DifferentContext).ForContext("AnotherDifferentContext").Log(@event);

            baseLog.Received(3).Log(Arg.Any<LogEvent>());
        }

        [TestCase(Context)]
        [TestCase(ContextPrefix)]
        public void WithMinimumLevelForSourceContext_should_return_a_log_that_logs_all_events_with_different_contexts(string filterValue)
        {
            var filterLog = baseLog.WithMinimumLevelForSourceContext(filterValue, LogLevel.Warn).ForContext(DifferentContext);

            ShouldBeEnabledFor(filterLog, GetAllLevels());
        }

        [TestCase(Context)]
        [TestCase(ContextPrefix)]
        public void WithMinimumLevelForSourceContext_should_return_a_log_that_logs_all_events_with_given_contexts_and_permitted_levels(string filterValue)
        {
            var filterLog1 = baseLog.WithMinimumLevelForSourceContext(filterValue, LogLevel.Warn)
                .ForContext(Context);

            var filterLog2 = baseLog.WithMinimumLevelForSourceContext(filterValue, LogLevel.Warn)
                .ForContext(DifferentContext)
                .ForContext(Context)
                .ForContext(DifferentContext);

            ShouldBeEnabledFor(filterLog1, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal);
            ShouldBeEnabledFor(filterLog2, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal);

            ShouldBeDisabledFor(filterLog1, LogLevel.Debug, LogLevel.Info);
            ShouldBeDisabledFor(filterLog2, LogLevel.Debug, LogLevel.Info);
        }

        [TestCase(Context, DifferentContext)]
        [TestCase(ContextPrefix, DifferentContextPrefix)]
        public void WithMinimumLevelForSourceContexts_should_return_a_log_that_logs_all_events_with_different_contexts(string filterValue1, string filterValue2)
        {
            var filterLog1 = baseLog.WithMinimumLevelForSourceContexts(LogLevel.Warn, filterValue1, filterValue2)
                                    .ForContext(DifferentContext);

            var filterLog2 = baseLog.WithMinimumLevelForSourceContexts(LogLevel.Warn, filterValue1, filterValue2)
                                    .ForContext(Context);

            ShouldBeEnabledFor(filterLog1, GetAllLevels());
            ShouldBeEnabledFor(filterLog2, GetAllLevels());
        }

        [TestCase(Context, DifferentContext)]
        [TestCase(ContextPrefix, DifferentContextPrefix)]
        public void WithMinimumLevelForSourceContexts_should_return_a_log_that_logs_all_events_with_given_contexts_and_permitted_levels(string filterValue1, string filterValue2)
        {
            var filterLog1 = baseLog.WithMinimumLevelForSourceContexts(LogLevel.Warn, filterValue1, filterValue2)
                                    .ForContext(Context)
                                    .ForContext(DifferentContext);

            var filterLog2 = baseLog.WithMinimumLevelForSourceContexts(LogLevel.Warn, filterValue1, filterValue2)
                                    .ForContext("SomeCtx1")
                                    .ForContext(DifferentContext)
                                    .ForContext("SomeCtx2")
                                    .ForContext(Context)
                                    .ForContext("SomeCtx3");

            ShouldBeEnabledFor(filterLog1, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal);
            ShouldBeEnabledFor(filterLog2, LogLevel.Warn, LogLevel.Error, LogLevel.Fatal);

            ShouldBeDisabledFor(filterLog1, LogLevel.Debug, LogLevel.Info);
            ShouldBeDisabledFor(filterLog2, LogLevel.Debug, LogLevel.Info);
        }

        private static LogLevel[] GetAllLevels()
            => Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>().ToArray();

        private void ShouldBeEnabledFor(ILog filterLog, params LogLevel[] levels)
        {
            foreach (var level in levels)
            {
                var newEvent = @event.WithLevel(level);

                baseLog.ClearReceivedCalls();

                filterLog.IsEnabledFor(level).Should().BeTrue();
                filterLog.Log(newEvent);

                baseLog.Received(1).Log(Arg.Any<LogEvent>());
            }
        }

        private void ShouldBeDisabledFor(ILog filterLog, params LogLevel[] levels)
        {
            foreach (var level in levels)
            {
                baseLog.ClearReceivedCalls();

                filterLog.Log(@event.WithLevel(level));

                baseLog.ReceivedCalls().Should().BeEmpty();
            }
        }
    }
}
