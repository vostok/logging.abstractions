using System;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Wrappers;

namespace Vostok.Logging.Abstractions.Tests.Wrappers
{
    [TestFixture]
    internal class SourceContextWrapper_Tests
    {
        private ILog baseLog;
        private ILog wrapper;
        private LogEvent originalEvent;
        private LogEvent observedEvent;

        [SetUp]
        public void TestSetup()
        {
            baseLog = Substitute.For<ILog>();
            baseLog.When(log => log.Log(Arg.Any<LogEvent>())).Do(info => observedEvent = info.Arg<LogEvent>());

            originalEvent = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null)
                .WithProperty("name1", "value1")
                .WithProperty("name2", "value2");

            observedEvent = null;

            wrapper = new SourceContextWrapper(baseLog, "ctx-value");
        }

        [Test]
        public void Log_method_should_handle_null_events()
        {
            wrapper.Log(null);

            baseLog.Received().Log(null);
        }

        [Test]
        public void Log_method_should_enrich_events_with_source_context_property()
        {
            wrapper.Log(originalEvent);

            baseLog.Received(1).Log(Arg.Any<LogEvent>());

            observedEvent.Properties.Should().HaveCount(3);
            observedEvent.Properties?[WellKnownProperties.SourceContext].Should().Be("ctx-value");
        }

        [Test]
        public void IsEnabled_method_should_delegate_to_base_log()
        {
            foreach (var level in Enum.GetValues(typeof(LogLevel)).Cast<LogLevel>())
            {
                baseLog.IsEnabledFor(level).Returns(true);

                wrapper.IsEnabledFor(level).Should().BeTrue();

                baseLog.IsEnabledFor(level).Returns(false);

                wrapper.IsEnabledFor(level).Should().BeFalse();
            }
        }

        [Test]
        public void ForContext_should_not_produce_wrapper_chains()
        {
            wrapper = wrapper
                .ForContext("1")
                .ForContext("2")
                .ForContext("3");

            var result = wrapper.Should().BeOfType<SourceContextWrapper>().Which;

            result.BaseLog.Should().BeSameAs(baseLog);

            result.Context.Should().Be("3");
        }

        [Test]
        public void ForContext_should_unwrap_chains_of_source_context_wrappers()
        {
            wrapper = new SourceContextWrapper(wrapper, "1");
            wrapper = new SourceContextWrapper(wrapper, "2");
            wrapper = new SourceContextWrapper(wrapper, "3");
            wrapper = wrapper.ForContext("4");

            var result = wrapper.Should().BeOfType<SourceContextWrapper>().Which;

            result.BaseLog.Should().BeSameAs(baseLog);

            result.Context.Should().Be("4");
        }
    }
}