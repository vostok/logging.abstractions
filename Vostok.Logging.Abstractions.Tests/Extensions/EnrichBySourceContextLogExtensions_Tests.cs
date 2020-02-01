using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class EnrichBySourceContextLogExtensions_Tests
    {
        private const string Context = "TestContext";
        private const string ContextPrefix = "test";
        private const string DifferentContext = "DifferentContext";

        private ILog baseLog;
        private LogEvent originalEvent;
        private List<LogEvent> observedEvents;

        [SetUp]
        public void TestSetup()
        {
            baseLog = Substitute.For<ILog>();
            baseLog.ForContext(Arg.Any<string>()).Returns(info => baseLog);
            baseLog.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);
            baseLog.When(log => log.Log(Arg.Any<LogEvent>())).Do(info => observedEvents.Add(info.Arg<LogEvent>()));

            originalEvent = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);
            observedEvents = new List<LogEvent>();
        }

        [TestCase(Context)]
        [TestCase(ContextPrefix)]
        public void EnrichBySourceContext_should_apply_given_enricher_to_logs_with_specified_context(string filterValue)
        {
            var filterLog = baseLog.EnrichBySourceContext(filterValue, log => log.WithProperty("key", "value"));

            filterLog.ForContext(Context).Log(originalEvent);
            filterLog.ForContext(Context).ForContext(DifferentContext).Log(originalEvent);
            filterLog.ForContext(DifferentContext).ForContext(Context).ForContext(DifferentContext).Log(originalEvent);
            filterLog.ForContext(DifferentContext).Log(originalEvent);

            observedEvents.Should().HaveCount(4);

            observedEvents[0].Properties?["key"].Should().Be("value");
            observedEvents[1].Properties?["key"].Should().Be("value");
            observedEvents[2].Properties?["key"].Should().Be("value");
            observedEvents[3].Properties.Should().BeNull();
        }
    }
}
