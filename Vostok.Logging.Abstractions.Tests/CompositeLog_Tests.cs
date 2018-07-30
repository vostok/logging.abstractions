using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests
{
    [TestFixture]
    internal class CompositeLog_Tests
    {
        private ILog log1;
        private ILog log2;
        private ILog log3;
        private CompositeLog compositeLog;

        [SetUp]
        public void TestSetup()
        {
            log1 = Substitute.For<ILog>();
            log2 = Substitute.For<ILog>();
            log3 = Substitute.For<ILog>();

            log1.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);
            log2.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);
            log3.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);

            log1.ForContext(Arg.Any<string>()).Returns(log1);
            log2.ForContext(Arg.Any<string>()).Returns(log2);
            log3.ForContext(Arg.Any<string>()).Returns(log3);

            compositeLog = new CompositeLog(log1, log2, log3);
        }

        [Test]
        public void Log_method_should_forward_log_event_to_all_underlying_logs_in_order()
        {
            var @event = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);

            compositeLog.Log(@event);

            Received.InOrder(
                () =>
                {
                    log1.Log(@event);
                    log2.Log(@event);
                    log3.Log(@event);
                });
        }

        [Test]
        public void IsEnabledFor_should_return_false_when_there_are_no_underlying_logs()
        {
            compositeLog = new CompositeLog();

            compositeLog.IsEnabledFor(LogLevel.Info).Should().BeFalse();
        }

        [Test]
        public void IsEnabledFor_should_return_false_when_none_of_underlying_logs_are_enabled_for_given_level()
        {
            log1.IsEnabledFor(LogLevel.Debug).Returns(false);
            log2.IsEnabledFor(LogLevel.Debug).Returns(false);
            log3.IsEnabledFor(LogLevel.Debug).Returns(false);

            compositeLog.IsEnabledFor(LogLevel.Debug).Should().BeFalse();
        }

        [Test]
        public void IsEnabledFor_should_return_true_when_at_least_one_of_underlying_logs_is_enabled_for_given_level()
        {
            log1.IsEnabledFor(LogLevel.Debug).Returns(false);
            log3.IsEnabledFor(LogLevel.Debug).Returns(false);

            compositeLog.IsEnabledFor(LogLevel.Debug).Should().BeTrue();
        }

        [Test]
        public void ForContext_should_return_same_instance_if_all_of_the_underlying_logs_return_same_instances()
        {
            compositeLog.ForContext("ctx").Should().BeSameAs(compositeLog);
        }

        [Test]
        public void ForContext_should_return_a_composite_log_with_derived_results_of_underlying_logs_with_same_context()
        {
            var log4 = Substitute.For<ILog>();
            var log5 = Substitute.For<ILog>();
            var log6 = Substitute.For<ILog>();

            log1.ForContext("ctx").Returns(log4);
            log2.ForContext("ctx").Returns(log5);
            log3.ForContext("ctx").Returns(log6);

            var result = compositeLog.ForContext("ctx");

            result.Log(null);

            log4.Received(1).Log(null);
            log5.Received(1).Log(null);
            log6.Received(1).Log(null);

            log1.DidNotReceive().Log(Arg.Any<LogEvent>());
            log2.DidNotReceive().Log(Arg.Any<LogEvent>());
            log3.DidNotReceive().Log(Arg.Any<LogEvent>());
        }
    }
}