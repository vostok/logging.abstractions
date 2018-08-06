using System;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class FilterByLevelLogExtensions_Tests
    {
        private ILog baseLog;
        private ILog filteredLog;
        private LogLevel[] allLevels;

        [SetUp]
        public void TestSetup()
        {
            baseLog = Substitute.For<ILog>();
            allLevels = Enum.GetValues(typeof (LogLevel)).Cast<LogLevel>().ToArray();
        }

        [Test]
        public void WithMinimumLevel_should_return_a_log_that_does_not_forward_log_events_of_lower_levels([Values] LogLevel minLevel)
        {
            filteredLog = baseLog.WithMinimumLevel(minLevel);

            foreach (var lowerLevel in allLevels.Where(l => l < minLevel))
            {
                filteredLog.Log(new LogEvent(lowerLevel, DateTimeOffset.Now, null));

                baseLog.ReceivedCalls().Should().BeEmpty();
            }
        }

        [Test]
        public void WithMinimumLevel_should_return_a_log_that_forwards_log_events_of_minimum_and_higher_levels([Values] LogLevel minLevel)
        {
            filteredLog = baseLog.WithMinimumLevel(minLevel);

            foreach (var sameOrHigherLevel in allLevels.Where(l => l >= minLevel))
            {
                var @event = new LogEvent(sameOrHigherLevel, DateTimeOffset.Now, null);

                filteredLog.Log(@event);

                baseLog.Received(1).Log(@event);
            }
        }

        [Test]
        public void WithMinimumLevel_should_return_a_log_that_is_unconditionally_disabled_for_lower_levels([Values] LogLevel minLevel)
        {
            filteredLog = baseLog.WithMinimumLevel(minLevel);

            foreach (var lowerLevel in allLevels.Where(l => l < minLevel))
            {
                filteredLog.IsEnabledFor(lowerLevel).Should().BeFalse();

                baseLog.ReceivedCalls().Should().BeEmpty();
            }
        }

        [Test]
        public void WithMinimumLevel_should_return_a_log_that_delegates_IsEnabled_calls_to_base_log_for_minimum_and_higher_levels([Values] LogLevel minLevel)
        {
            filteredLog = baseLog.WithMinimumLevel(minLevel);

            foreach (var sameOrHigherLevel in allLevels.Where(l => l >= minLevel))
            {
                baseLog.IsEnabledFor(sameOrHigherLevel).Returns(true);

                filteredLog.IsEnabledFor(sameOrHigherLevel).Should().BeTrue();

                baseLog.Received(1).IsEnabledFor(sameOrHigherLevel);
            }
        }

        [Test]
        public void WithMinimumLevel_should_return_a_log_that_handles_null_events_gracefully()
        {
            filteredLog = baseLog.WithMinimumLevel(LogLevel.Warn);

            filteredLog.Log(null);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithDisabledLevels_should_return_a_log_that_does_not_forward_log_events_of_given_levels([Values] LogLevel disabledLevel)
        {
            filteredLog = baseLog.WithDisabledLevels(disabledLevel);

            filteredLog.Log(new LogEvent(disabledLevel, DateTimeOffset.Now, null));

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithDisabledLevels_should_return_a_log_that_forwards_log_events_of_unmentioned_levels([Values] LogLevel disabledLevel)
        {
            filteredLog = baseLog.WithDisabledLevels(disabledLevel);

            foreach (var level in allLevels.Where(l => l != disabledLevel))
            {
                var @event = new LogEvent(level, DateTimeOffset.Now, null);

                filteredLog.Log(@event);

                baseLog.Received(1).Log(@event);
            }
        }

        [Test]
        public void WithDisabledLevels_should_return_a_log_that_is_unconditionally_disabled_for_given_levels([Values] LogLevel disabledLevel)
        {
            baseLog.IsEnabledFor(disabledLevel).Returns(true);

            filteredLog = baseLog.WithDisabledLevels(disabledLevel);

            filteredLog.IsEnabledFor(disabledLevel).Should().BeFalse();

            baseLog.ReceivedCalls().Should().BeEmpty();
        }
        
        [Test]
        public void WithDisabledLevels_should_return_a_log_that_delegates_IsEnabled_calls_to_base_log_for_unmentioned_levels([Values] LogLevel disabledLevel)
        {
            filteredLog = baseLog.WithDisabledLevels(disabledLevel);
        
            foreach (var level in allLevels.Where(l => l != disabledLevel))
            {
                baseLog.IsEnabledFor(level).Returns(true);
        
                filteredLog.IsEnabledFor(level).Should().BeTrue();
        
                baseLog.Received(1).IsEnabledFor(level);
            }
        }

        [Test]
        public void WithDisabledLevels_should_return_a_log_that_handles_null_events_gracefully()
        {
            filteredLog = baseLog.WithDisabledLevels(LogLevel.Warn);

            filteredLog.Log(null);

            baseLog.Received(1).Log(null);
        }
    }
}