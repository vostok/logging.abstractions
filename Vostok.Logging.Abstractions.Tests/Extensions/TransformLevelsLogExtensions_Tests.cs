using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class TransformLevelsLogExtensions_Tests
    {
        private ILog baseLog;
        private ILog transformingLog;
        private LogEvent @event;

        [SetUp]
        public void TestSetup()
        {
            baseLog = Substitute.For<ILog>();

            transformingLog = baseLog.WithLevelsTransformation(
                new Dictionary<LogLevel, LogLevel>
                {
                    [LogLevel.Error] = LogLevel.Warn,
                    [LogLevel.Fatal] = LogLevel.Warn
                });

            @event = new LogEvent(LogLevel.Error, DateTimeOffset.Now, null);
        }

        [Test]
        public void Wrapped_log_should_log_events_with_unmapped_levels_as_is()
        {
            @event = @event.WithLevel(LogLevel.Info);

            transformingLog.Log(@event);

            baseLog.Received().Log(@event);
        }

        [Test]
        public void Wrapped_log_should_transform_log_event_levels_according_to_given_mapping()
        {
            transformingLog.Log(@event);

            baseLog.DidNotReceive().Log(@event);
            baseLog.Received().Log(Arg.Is<LogEvent>(e => e.Level == LogLevel.Warn));
        }

        [Test]
        public void Wrapped_log_should_transform_log_event_levels_passed_to_IsEnabledFor()
        {
            transformingLog.IsEnabledFor(LogLevel.Info);
            transformingLog.IsEnabledFor(LogLevel.Error);
            transformingLog.IsEnabledFor(LogLevel.Fatal);

            Received.InOrder(
                () =>
                {
                    baseLog.IsEnabledFor(LogLevel.Info);
                    baseLog.IsEnabledFor(LogLevel.Warn);
                    baseLog.IsEnabledFor(LogLevel.Warn);
                });
        }
    }
}