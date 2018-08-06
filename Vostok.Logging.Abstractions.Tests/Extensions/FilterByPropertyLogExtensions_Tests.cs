using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class FilterByPropertyLogExtensions_Tests
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
        public void WithEventsSelectedByProperty_should_return_a_log_that_drops_null_events()
        {
            filteredLog = baseLog.WithEventsSelectedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(null);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithEventsSelectedByProperty_should_return_a_log_that_drops_events_without_any_properties()
        {
            filteredLog = baseLog.WithEventsSelectedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithEventsSelectedByProperty_should_return_a_log_that_drops_events_without_property_with_given_key()
        {
            @event = @event.WithProperty("key2", "value2");

            filteredLog = baseLog.WithEventsSelectedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithEventsSelectedByProperty_should_return_a_log_that_drops_events_with_property_of_non_matching_type()
        {
            @event = @event.WithProperty("key1", 123);

            filteredLog = baseLog.WithEventsSelectedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithEventsSelectedByProperty_should_return_a_log_that_drops_events_with_property_whose_values_do_not_match_the_predicate()
        {
            @event = @event.WithProperty("key1", "suffix");

            filteredLog = baseLog.WithEventsSelectedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithEventsSelectedByProperty_should_return_a_log_that_forwards_events_with_property_whose_values_match_the_predicate()
        {
            @event = @event.WithProperty("key1", "prefix+suffix");

            filteredLog = baseLog.WithEventsSelectedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.Received(1).Log(@event);
        }

        [Test]
        public void WithEventsDroppedByProperty_should_return_a_log_that_forwards_null_events()
        {
            filteredLog = baseLog.WithEventsDroppedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(null);

            baseLog.Received(1).Log(null);
        }

        [Test]
        public void WithEventsDroppedByProperty_should_return_a_log_that_forwards_events_without_any_properties()
        {
            filteredLog = baseLog.WithEventsDroppedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.Received(1).Log(@event);
        }

        [Test]
        public void WithEventsDroppedByProperty_should_return_a_log_that_forwards_events_without_property_with_given_key()
        {
            @event = @event.WithProperty("key2", "value2");

            filteredLog = baseLog.WithEventsDroppedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.Received(1).Log(@event);
        }

        [Test]
        public void WithEventsDroppedByProperty_should_return_a_log_that_forwards_events_with_property_of_non_matching_type()
        {
            @event = @event.WithProperty("key1", 123);

            filteredLog = baseLog.WithEventsDroppedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.Received(1).Log(@event);
        }

        [Test]
        public void WithEventsDroppedByProperty_should_return_a_log_that_forwards_events_with_property_whose_values_do_not_match_the_predicate()
        {
            @event = @event.WithProperty("key1", "suffix");

            filteredLog = baseLog.WithEventsDroppedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.Received(1).Log(@event);
        }

        [Test]
        public void WithEventsDroppedByProperty_should_return_a_log_that_drops_events_with_property_whose_values_match_the_predicate()
        {
            @event = @event.WithProperty("key1", "prefix+suffix");

            filteredLog = baseLog.WithEventsDroppedByProperty<string>("key1", value => value.StartsWith("prefix"));

            filteredLog.Log(@event);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithEventsSelectedByProperties_should_return_a_log_that_passes_empty_dictionary_to_predicate_when_event_is_null()
        {
            IReadOnlyDictionary<string, object> observedProperties = null;

            filteredLog = baseLog.WithEventsSelectedByProperties(
                props =>
                {
                    observedProperties = props;
                    return true;
                });

            filteredLog.Log(null);

            observedProperties.Should().NotBeNull();
            observedProperties.Should().BeEmpty();
        }

        [Test]
        public void WithEventsSelectedByProperties_should_return_a_log_that_drops_events_not_matched_by_the_predicate()
        {
            filteredLog = baseLog.WithEventsSelectedByProperties(_ => false);

            filteredLog.Log(@event);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }

        [Test]
        public void WithEventsSelectedByProperties_should_return_a_log_that_forwards_events_matched_by_the_predicate()
        {
            filteredLog = baseLog.WithEventsSelectedByProperties(_ => true);

            filteredLog.Log(@event);

            baseLog.Received(1).Log(@event);
        }

        [Test]
        public void WithEventsDroppedByProperties_should_return_a_log_that_passes_empty_dictionary_to_predicate_when_event_is_null()
        {
            IReadOnlyDictionary<string, object> observedProperties = null;

            filteredLog = baseLog.WithEventsDroppedByProperties(
                props =>
                {
                    observedProperties = props;
                    return true;
                });

            filteredLog.Log(null);

            observedProperties.Should().NotBeNull();
            observedProperties.Should().BeEmpty();
        }

        [Test]
        public void WithEventsDroppedByProperties_should_return_a_log_that_forwards_events_not_matched_by_the_predicate()
        {
            filteredLog = baseLog.WithEventsDroppedByProperties(_ => false);

            filteredLog.Log(@event);

            baseLog.Received(1).Log(@event);
        }

        [Test]
        public void WithEventsDroppedByProperties_should_return_a_log_that_drops_events_matched_by_the_predicate()
        {
            filteredLog = baseLog.WithEventsDroppedByProperties(_ => true);

            filteredLog.Log(@event);

            baseLog.ReceivedCalls().Should().BeEmpty();
        }
    }
}