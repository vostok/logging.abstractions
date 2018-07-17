using System;
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
        public void WithProperty_should_not_overwrite_existing_properties_by_default()
        {
            enrichedLog = baseLog.WithProperty("name2", "valueX");

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(2);
            observedEvent.Properties?["name2"].Should().Be("value2");
        }

        [Test]
        public void WithProperty_should_overwrite_existing_properties_when_explicitly_asked_to()
        {
            enrichedLog = baseLog.WithProperty("name2", "valueX", true);

            enrichedLog.Log(originalEvent);

            observedEvent.Properties.Should().HaveCount(2);
            observedEvent.Properties?["name2"].Should().Be("valueX");
        }
    }
}