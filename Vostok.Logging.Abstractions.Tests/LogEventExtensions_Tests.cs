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
            @event  = @event.WithProperty("B", 2);
            @event.Properties.Should().BeEquivalentTo(new Dictionary<string, object> { { "A", 1 }, { "B", 2 } });
        }

        [Test]
        public void WithProperty_should_rewrite_existed_property()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message").WithProperty("A", 1);
            @event = @event.WithProperty("A", 2);
            @event.Properties.Should().BeEquivalentTo(new Dictionary<string, object> { { "A", 2 } });
        }

        [Test]
        public void WithProperty_should_be_case_insensitive()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message").WithProperty("a", 1);
            @event = @event.WithProperty("A", 2);
            @event.Properties.Should().BeEquivalentTo(new Dictionary<string, object> { { "A", 2 } });
        }

        [Test]
        public void WithProperty_should_create_new_properties_if_event_has_no_properties()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, "message");
            @event = @event.WithProperty("A", 1);
            @event.Properties.Should().BeEquivalentTo(new Dictionary<string, object> { { "A", 1 } });
        }
    }
}