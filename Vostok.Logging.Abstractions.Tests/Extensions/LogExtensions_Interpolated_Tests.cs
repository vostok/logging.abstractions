#if NET6_0

using System;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class LogExtensions_Interpolated_Tests
    {
        [Test]
        public void Should_construct_properties_and_template()
        {
            var log = Substitute.For<ILog>();
            log.IsEnabledFor(Arg.Any<LogLevel>()).Returns(true);
            
            var exception = new Exception("error");
            var myClass = new MyClass();
            var str = "asdf qwer";
            var number = 333;

            log.Info(exception, $"myClass = {myClass}, str = {str}, number = {number}");

            log.Received(1)
                .Log(
                    Arg.Is<LogEvent>(
                        e =>
                            e.Level == LogLevel.Info &&
                            e.Exception == exception
                        ));
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