#if NET6_0

using System;
using System.Threading;
using FluentAssertions;
using FluentAssertions.Extensions;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class LogExtensions_Interpolated_Tests
    {
        [Test]
        public void Should_do_the_same_with_interpolated_as_without()
        {
            var log = Substitute.For<ILog>();
            LogEvent lastEvent = null!;
            log.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);
            log.Log(Arg.Do<LogEvent>(e => lastEvent = e));

            var exception = new Exception("error");
            var myClass = new MyClass();
            var str = "asdf qwer";
            var number = 333;

            log.Info(exception, "myClass = {myClass}, str = {str}, number = {number}", myClass, str, number);
            log.Received(1).Log(Arg.Any<LogEvent>());
            var expected = lastEvent;
            
            Thread.Sleep(3.Seconds());
            
            log.Info(exception, $"myClass = {myClass}, str = {str}, number = {number}");
            log.Received(2).Log(Arg.Any<LogEvent>());
            var received = lastEvent;
            
            received.Should().BeEquivalentTo(expected, config => config.Excluding(e => e.Timestamp));
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