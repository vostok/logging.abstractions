#if NET6_0

using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class LogExtensions_Interpolated_Tests
    {
        private ILog log;
        private LogEvent lastEvent;
        private Exception exception;
        private MyClass myClass;
        private string str;
        private int number;

        [SetUp]
        public void SetUp()
        {
            log = Substitute.For<ILog>();
            lastEvent = null!;
            log.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);
            log.Log(Arg.Do<LogEvent>(e => lastEvent = e));

            exception = new Exception("error");
            myClass = new MyClass();
            str = "asdf qwer";
            number = 333;
        }
        
        [Test]
        public void Should_do_the_same_with_interpolated_as_without()
        {
            log.Info(exception, "myClass = {myClass}, str = {str}, number = {number}", myClass, str, number);
            log.Received(1).Log(Arg.Any<LogEvent>());
            var expected = lastEvent;
            
            log.Info(exception, $"myClass = {myClass}, str = {str}, number = {number}");
            log.Received(2).Log(Arg.Any<LogEvent>());
            var received = lastEvent;
            
            received.Should().BeEquivalentTo(expected, config => config.Excluding(e => e.Timestamp));
        }
        
        [Test]
        public void Should_log_without_variables()
        {
            log.Info(exception, $"myClass = {new MyClass()}, str = {"asdf qwer"}, number = {333}");
            log.Received(1).Log(Arg.Any<LogEvent>());
            var received = lastEvent;

            received.MessageTemplate.Should().Be("myClass = {new_MyClass__}, str = {_asdf_qwer_}, number = {333}");
            received.Properties.Should()
                .BeEquivalentTo(new Dictionary<string, object>
                {
                    ["new_MyClass__"] = new MyClass(),
                    ["_asdf_qwer_"] = "asdf qwer",
                    ["333"] = 333
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
        public void Should_replace_non_property_name_symbols()
        {
            var c = new[] { "a", "b" };

            log.Info($"Some thing {c.Length}, {c.Length.ToString()}");
            
            var received = lastEvent;

            received.MessageTemplate.Should().Be("Some thing {c.Length}, {c.Length.ToString__}");
            received.Properties.Should()
                .BeEquivalentTo(new Dictionary<string, object>
                {
                    ["c.Length"] = 2,
                    ["c.Length.ToString__"] = "2",
                });
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

        private class MyClass
        {
            public string X { get; set; } = "hello";
            public int I { get; set; } = 42;
            public override string ToString() =>
                $"{X} {I}";
        }
    }
}

#endif