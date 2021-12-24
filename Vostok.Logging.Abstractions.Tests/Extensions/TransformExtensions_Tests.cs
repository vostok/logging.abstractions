using System;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class TransformExtensions_Tests
    {
        [Test]
        public void Wrapped_log_should_log_events_with_new_property()
        {
            var baseLog = Substitute.For<ILog>();

            var transformingLog = baseLog.WithTransformation(e => e.WithProperty("p2", "v2"));

            var @event = new LogEvent(LogLevel.Info,
                DateTimeOffset.Now,
                null,
                new Dictionary<string, object>
                {
                    ["p1"] = "v1"
                },
                null);

            transformingLog.Log(@event);

            baseLog.Received()
                .Log(Arg.Is<LogEvent>(e =>
                    (string)e.Properties["p1"] == "v1" &&
                    (string)e.Properties["p2"] == "v2"));
        }
    }
}