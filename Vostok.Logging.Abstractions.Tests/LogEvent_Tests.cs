using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests
{
    internal class LogEvent_Tests
    {
        [Test]
        public void WithProperty_should_add_absent_property_to_log_event_properties()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message").WithProperty("A", 1);
            @event = @event.WithProperty("B", 2);
            @event.Properties.Should().BeEquivalentTo(new Dictionary<string, object> {{"A", 1}, {"B", 2}});
        }

        [Test]
        public void WithProperty_should_rewrite_existed_property()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message").WithProperty("A", 1);
            @event = @event.WithProperty("A", 2);
            @event.Properties.Should().BeEquivalentTo(new Dictionary<string, object> {{"A", 2}});
        }

        [Test]
        public void WithProperty_should_be_case_insensitive()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message").WithProperty("a", 1);
            @event = @event.WithProperty("A", 2);
            @event.Properties.Should().BeEquivalentTo(new Dictionary<string, object> {{"A", 2}});
        }

        [Test]
        public void WithProperty_should_create_new_properties_if_event_has_no_properties()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message");
            @event = @event.WithProperty("A", 1);
            @event.Properties.Should().BeEquivalentTo(new Dictionary<string, object> {{"A", 1}});
        }

        [Test]
        public void WithProperty_should_return_same_log_event_when_rewriting_a_property_with_same_value()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message").WithProperty("A", 1);
            var newEvent = @event.WithProperty("A", 1);

            newEvent.Should().BeSameAs(@event);
        }

        [Test]
        public void WithoutProperty_should_have_no_effect_on_event_without_any_properties()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message");
            var newEvent = @event.WithoutProperty("A");

            newEvent.Should().BeSameAs(@event);
        }

        [Test]
        public void WithoutProperty_should_have_no_effect_on_event_without_given_property()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message")
                .WithProperty("B", 1)
                .WithProperty("C", 2);

            var newEvent = @event.WithoutProperty("A");

            newEvent.Should().BeSameAs(@event);
        }

        [Test]
        public void WithoutProperty_should_remove_property_with_given_name()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message")
                .WithProperty("B", 1)
                .WithProperty("C", 2);

            var newEvent = @event.WithoutProperty("B");

            newEvent.Properties.Should().BeEquivalentTo(new Dictionary<string, object> { { "C", 2 } });
        }

        [Test]
        public void WithoutProperty_should_be_case_insensitive()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message")
                .WithProperty("B", 1)
                .WithProperty("C", 2);

            var newEvent = @event.WithoutProperty("c");

            newEvent.Properties.Should().BeEquivalentTo(new Dictionary<string, object> { { "B", 1 } });
        }
    }
}
