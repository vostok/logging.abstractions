using System;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests
{
    [TestFixture]
    internal class LogEventExtensions_Tests
    {
        private LogEvent eventBefore;
        private LogEvent eventAfter;

        [SetUp]
        public void TestSetup()
        {
            eventBefore = new LogEvent(LogLevel.Info, DateTimeOffset.UtcNow, null)
                .WithProperty("A", 1)
                .WithProperty("B", 2);
        }

        [Test]
        public void WithParameters_should_return_same_event_when_params_array_is_null()
        {
            eventAfter = eventBefore.WithParameters(null);

            eventAfter.Should().BeSameAs(eventBefore);
        }

        [Test]
        public void WithParameters_should_enrich_event_with_index_named_properties()
        {
            eventAfter = eventBefore.WithParameters(new object[] {"value", null, 123});

            eventAfter.Properties.Should().HaveCount(5);
            eventAfter?.Properties?["0"].Should().Be("value");
            eventAfter?.Properties?["1"].Should().BeNull();
            eventAfter?.Properties?["2"].Should().Be(123);
        }
    }
}