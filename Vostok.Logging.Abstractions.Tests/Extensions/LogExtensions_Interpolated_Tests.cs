#if NET6_0_OR_GREATER

using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Vostok.Logging.Formatting;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class LogExtensions_Interpolated_Tests
    {
        private ILog log;
        private LogEvent lastEvent;
        private string lastLog;
        private Exception exception;
        private MyClass myClass;
        private MyClass2 myClass2;
        private string str;
        private int number;

        [SetUp]
        public void SetUp()
        {
            var template = OutputTemplate.Create()
                .AddLevel()
                .AddMessage()
                .AddNewline()
                .Build();
            
            log = Substitute.For<ILog>();
            lastEvent = null!;
            log.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);
            log.Log(Arg.Do<LogEvent>(e =>
            {
                lastEvent = e;
                lastLog = LogEventFormatter.Format(e, template);
            }));

            exception = new Exception("error");
            myClass = new MyClass();
            myClass2 = new MyClass2();
            str = "asdf qwer";
            number = 333;
        }
        
        [Test]
        public void Should_do_the_same_with_interpolated_as_without()
        {
            log.Info(exception, "myClass = {myClass}, myClass2 = {myClass2}, str = {str}, number = {number}", myClass, myClass2, str, number);
            log.Received(1).Log(Arg.Any<LogEvent>());
            var (expectedEvent, expectedLog) = (lastEvent, lastLog);
            
            log.Info(exception, $"myClass = {myClass}, myClass2 = {myClass2}, str = {str}, number = {number}");
            log.Received(2).Log(Arg.Any<LogEvent>());
            var (receivedEvent, receivedLog) = (lastEvent, lastLog);
            
            receivedEvent.Should().BeEquivalentTo(expectedEvent, config => config.Excluding(e => e.Timestamp));
            receivedLog.Should().Be(expectedLog);
        }
        
        [Test]
        public void Should_log_without_variables()
        {
            log.Info(exception, $"myClass = {new MyClass()}, myClass2 = {new MyClass2()}, str = {"asdf qwer"}, number = {number}");
            log.Received(1).Log(Arg.Any<LogEvent>());
            var received = lastEvent;

            received.MessageTemplate.Should()
                .Be("myClass = hello 42, myClass2 = Vostok.Logging.Abstractions.Tests.Extensions.LogExtensions_Interpolated_Tests+MyClass2, str = asdf qwer, number = {number}");
            received.Properties.Should()
                .BeEquivalentTo(new Dictionary<string, object>
                {
                    ["number"] = 333
                });
        }

        [Test]
        public void Should_do_nothing_when_disabled()
        {
            log.IsEnabledFor(Arg.Any<LogLevel>()).Returns(false);
            
            log.Info($"myClass = {123}, str = {str}, number = {number}");
            log.Received(0).Log(Arg.Any<LogEvent>());
        }
        
        [Test]
        public void Should_format_arguments()
        {
            log.Info(exception, $"number1 = {42:e2} number2 = {43,10} number3 = {44,10:e2}");
            log.Received(1).Log(Arg.Any<LogEvent>());
            var received = lastEvent;
            
            received.MessageTemplate.Should().Be("number1 = {42} number2 = {43} number3 = {44}");
            received.Properties.Should()
                .BeEquivalentTo(new Dictionary<string, object>
                {
                    ["42"] = "4.20e+001",
                    ["43"] = "        43",
                    ["44"] = " 4.40e+001"
                });
        }
        
        [Test]
        public void Should_not_interpolate_if_formatted_explicitly()
        {
            log.Info(exception, $"myClass = {myClass}, str = {str}, number = {number}".ToString());
            log.Received(1).Log(Arg.Any<LogEvent>());
            var received = lastEvent;
            
            received.Properties.Should().BeNull();
            received.MessageTemplate.Should().Be($"myClass = {myClass}, str = {str}, number = {number}");
        }
        
        [Test]
        public void Should_not_interpolate_if_casted()
        {
            log.Info(exception, (string)$"myClass = {myClass}, str = {str}, number = {number}");
            log.Received(1).Log(Arg.Any<LogEvent>());
            var received = lastEvent;
            
            received.Properties.Should().BeNull();
            received.MessageTemplate.Should().Be($"myClass = {myClass}, str = {str}, number = {number}");
        }

        private class MyClass
        {
            public string X { get; set; } = "hello";
            public int I { get; set; } = 42;
            public override string ToString() =>
                $"{X} {I}";
        }
        
        private class MyClass2
        {
            public string X { get; set; } = "hello";
            public int I { get; set; } = 42;
        }
    }
}

#endif