using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests
{
    [TestFixture]
    internal class WithPropertyLogExtensions_Tests
    {
        private ILog baseLog;
        private ILog enrichedLog;
        private LogEvent originalEvent;
        private LogEvent observedEvent;

        [SetUp]
        public void TestSetup()
        {
            baseLog = Substitute.For<ILog>();
            baseLog.When(log => log.Log(Arg.Any<LogEvent>())).Do(info => observedEvent = info.Arg<LogEvent>());

            originalEvent = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, null)
                .WithProperty("name1", "value1")
                .WithProperty("name2", "value2");

            observedEvent = null;
        }

        [Test]
        public void WithProperty_should_return_a_log_that_adds_given_property_to_log_events()
        {
            enrichedLog = baseLog.WithProperty("name3", "value3");

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(3);
            observedEvent.Properties?["name3"].Should().Be("value3");
        }

        [Test]
        public void WithProperty_should_return_a_log_that_does_not_overwrite_existing_properties_by_default()
        {
            enrichedLog = baseLog.WithProperty("name2", "valueX");

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(2);
            observedEvent.Properties?["name2"].Should().Be("value2");
        }

        [Test]
        public void WithProperty_should_return_a_log_that_overwrites_existing_properties_when_explicitly_asked_to()
        {
            enrichedLog = baseLog.WithProperty("name2", "valueX", true);

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(2);
            observedEvent.Properties?["name2"].Should().Be("valueX");
        }

        [Test]
        public void WithProperty_should_return_a_log_that_handles_null_events_gracefully()
        {
            enrichedLog = baseLog.WithProperty("name3", "value3");

            enrichedLog.Log(null);

            observedEvent.Should().BeNull();

            baseLog.Received(1).Log(null);
        }

        [Test]
        public void WithProperty_with_dynamic_value_should_call_value_provider_delegate_for_each_event()
        {
            var counter = 0;

            enrichedLog = baseLog.WithProperty("name3", () => ++counter);

            enrichedLog.Log(originalEvent);

            observedEvent.Properties?["name3"]?.Should().Be(1);

            enrichedLog.Log(originalEvent);

            observedEvent.Properties?["name3"]?.Should().Be(2);
        }

        [Test]
        public void WithProperties_should_return_a_log_that_adds_given_properties_to_log_events()
        {
            enrichedLog = baseLog.WithProperties(new Dictionary<string, object>
            {
                { "name3", "value3" },
                { "name4", "value4" }
            });

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(4);
            observedEvent.Properties?["name3"].Should().Be("value3");
            observedEvent.Properties?["name4"].Should().Be("value4");
        }

        [Test]
        public void WithProperties_should_return_a_log_that_does_not_overwrite_existing_properties_by_default()
        {
            enrichedLog = baseLog.WithProperties(new Dictionary<string, object>
            {
                { "name2", "valueX" },
                { "name3", "value3" }
            });

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(3);
            observedEvent.Properties?["name2"].Should().Be("value2");
            observedEvent.Properties?["name3"].Should().Be("value3");
        }

        [Test]
        public void WithProperties_should_return_a_log_that_overwrites_existing_properties_when_explicitly_asked_to()
        {
            enrichedLog = baseLog.WithProperties(new Dictionary<string, object>
            {
                { "name2", "valueX" },
                { "name3", "value3" }
            }, true);

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(3);
            observedEvent.Properties?["name2"].Should().Be("valueX");
            observedEvent.Properties?["name3"].Should().Be("value3");
        }

        [Test]
        public void WithProperties_should_return_a_log_that_handles_null_events_gracefully()
        {
            enrichedLog = baseLog.WithProperties(new Dictionary<string, object>
            {
                { "name3", "value3" },
                { "name4", "value4" }
            }, true);

            enrichedLog.Log(null);

            observedEvent.Should().BeNull();

            baseLog.Received(1).Log(null);
        }

        [Test]
        public void WithProperties_with_dynamic_provider_should_call_the_delegate_for_each_event()
        {
            var counter = 0;

            enrichedLog = baseLog.WithProperties(() => new []
            {
                ("name3", ++counter as object),
                ("name4", ++counter as object),
            });

            enrichedLog.Log(originalEvent);

            observedEvent.Properties?["name3"]?.Should().Be(1);
            observedEvent.Properties?["name4"]?.Should().Be(2);

            enrichedLog.Log(originalEvent);

            observedEvent.Properties?["name3"]?.Should().Be(3);
            observedEvent.Properties?["name4"]?.Should().Be(4);
        }

        [Test]
        public void WithProperties_with_dynamic_provider_should_handle_null_object_returned()
        {
            enrichedLog = baseLog.WithProperties(() => null);

            enrichedLog.Log(originalEvent);

            observedEvent.Should().BeSameAs(originalEvent);
        }

        [Test]
        public void WithObjectProperties_should_return_a_log_that_adds_given_properties_to_log_events()
        {
            enrichedLog = baseLog.WithObjectProperties(new {name3 = "value3", name4 = "value4"});

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(4);
            observedEvent.Properties?["name3"].Should().Be("value3");
            observedEvent.Properties?["name4"].Should().Be("value4");
        }

        [Test]
        public void WithObjectProperties_should_return_a_log_that_does_not_overwrite_existing_properties_by_default()
        {
            enrichedLog = baseLog.WithObjectProperties(new { name2 = "valueX", name3 = "value3" });

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(3);
            observedEvent.Properties?["name2"].Should().Be("value2");
            observedEvent.Properties?["name3"].Should().Be("value3");
        }

        [Test]
        public void WithObjectProperties_should_return_a_log_that_overwrites_existing_properties_when_explicitly_asked_to()
        {
            enrichedLog = baseLog.WithObjectProperties(new { name2 = "valueX", name3 = "value3" }, true);

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(3);
            observedEvent.Properties?["name2"].Should().Be("valueX");
            observedEvent.Properties?["name3"].Should().Be("value3");
        }

        [Test]
        public void WithObjectProperties_should_return_a_log_that_handles_null_events_gracefully()
        {
            enrichedLog = baseLog.WithObjectProperties(new { name3 = "value3", name4 = "value4" });

            enrichedLog.Log(null);

            observedEvent.Should().BeNull();

            baseLog.Received(1).Log(null);
        }

        [Test]
        public void WithObjectProperties_with_dynamic_provider_should_call_the_delegate_for_each_event()
        {
            var counter = 0;

            enrichedLog = baseLog.WithObjectProperties(() => new { name3 = ++counter, name4 = ++counter });

            enrichedLog.Log(originalEvent);

            observedEvent.Properties?["name3"]?.Should().Be(1);
            observedEvent.Properties?["name4"]?.Should().Be(2);

            enrichedLog.Log(originalEvent);

            observedEvent.Properties?["name3"]?.Should().Be(3);
            observedEvent.Properties?["name4"]?.Should().Be(4);
        }

        [Test]
        public void WithObjectProperties_with_dynamic_provider_should_handle_null_object_returned()
        {
            enrichedLog = baseLog.WithObjectProperties(() => null as object);

            enrichedLog.Log(originalEvent);

            observedEvent.Should().BeSameAs(originalEvent);
        }
    }
}