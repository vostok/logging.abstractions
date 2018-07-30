using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NSubstitute;
using NUnit.Framework;

#pragma warning disable 612

namespace Vostok.Logging.Abstractions.Tests
{
    [TestFixture]
    internal class LogExtensions_Tests
    {
        private ILog log;
        private Exception exception;
        private string message;

        [SetUp]
        public void SetUp()
        {
            log = Substitute.For<ILog>();
            exception = new Exception();
            message = "message";
        }

        [Test]
        public void LogMethod_should_work_correctly_for_arguments_message([Values] LogLevel level)
        {
            var method = GetLogMethod(level, typeof (ILog), typeof (string));

            method.Invoke(null, new object[] {log, message});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, message});
            log.Received(1)
                .Log(
                    Arg.Is<LogEvent>(
                        e =>
                            e.Level == level &&
                            HasRoughlyLocalTimestamp(e) &&
                            e.MessageTemplate.Equals(message) &&
                            e.Properties == null &&
                            e.Exception == null));
        }

        [Test]
        public void LogMethod_should_work_correctly_for_arguments_exception([Values] LogLevel level)
        {
            var method = GetLogMethod(level, typeof (ILog), typeof (Exception));

            method.Invoke(null, new object[] {log, exception});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, exception});
            log.Received(1)
                .Log(
                    Arg.Is<LogEvent>(
                        e =>
                            e.Level == level &&
                            HasRoughlyLocalTimestamp(e) &&
                            e.MessageTemplate == null &&
                            e.Properties == null &&
                            e.Exception == exception));
        }

        [Test]
        public void LogMethod_should_work_correctly_for_arguments_exception_message([Values] LogLevel level)
        {
            var method = GetLogMethod(level, typeof (ILog), typeof (Exception), typeof (string));

            method.Invoke(null, new object[] {log, exception, message});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, exception, message});
            log.Received(1)
                .Log(
                    Arg.Is<LogEvent>(
                        e =>
                            e.Level == level &&
                            HasRoughlyLocalTimestamp(e) &&
                            e.MessageTemplate.Equals(message) &&
                            e.Properties == null &&
                            e.Exception == exception));
        }

        [Test]
        public void LogMethod_should_work_correctly_for_arguments_message_properties([Values] LogLevel level)
        {
            var method = GetLogMethodWithSingleGenericParameter(level, typeof (ILog), typeof (string));
            method = method.MakeGenericMethod(new {A = 1}.GetType());

            method.Invoke(null, new object[] {log, message, new {A = 1}});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, message, new {A = 1}});
            log.Received(1)
                .Log(
                    Arg.Is<LogEvent>(
                        e =>
                            e.Level == level &&
                            HasRoughlyLocalTimestamp(e) &&
                            e.MessageTemplate.Equals(message) &&
                            e.Properties.SequenceEqual(new Dictionary<string, object> {{"A", 1}}) &&
                            e.Exception == null));
        }

        [Test]
        public void LogMethod_should_use_non_anonymous_class_argument_like_parameter_for_arguments_message_properies([Values] LogLevel level)
        {
            var method = GetLogMethodWithSingleGenericParameter(level, typeof (ILog), typeof (string));
            method = method.MakeGenericMethod(new CustomClass().GetType());

            var obj = new CustomClass();
            method.Invoke(null, new object[] {log, message, obj});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, message, obj});
            log.Received(1).Log(Arg.Is<LogEvent>(e => e.Properties.SequenceEqual(new Dictionary<string, object> {{"0", obj}})));
        }

        [Test]
        public void LogMethod_should_work_correctly_for_arguments_message_parameters([Values] LogLevel level)
        {
            var method = GetLogMethod(level, typeof (ILog), typeof (string), typeof (object[]));

            var obj = new object();
            method.Invoke(null, new object[] {log, message, new[] {obj}});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, message, new[] {obj}});
            log.Received(1)
                .Log(
                    Arg.Is<LogEvent>(
                        e =>
                            e.Level == level &&
                            HasRoughlyLocalTimestamp(e) &&
                            e.MessageTemplate.Equals(message) &&
                            e.Properties.SequenceEqual(new Dictionary<string, object> {{"0", obj}}) &&
                            e.Exception == null));
        }

        [Test]
        public void LogMethod_should_work_correctly_for_arguments_exception_message_properties([Values] LogLevel level)
        {
            var method = GetLogMethodWithSingleGenericParameter(level, typeof (ILog), typeof (Exception), typeof (string));
            method = method.MakeGenericMethod(new {A = 1}.GetType());

            method.Invoke(null, new object[] {log, exception, message, new {A = 1}});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, exception, message, new {A = 1}});
            log.Received(1)
                .Log(
                    Arg.Is<LogEvent>(
                        e =>
                            e.Level == level &&
                            HasRoughlyLocalTimestamp(e) &&
                            e.MessageTemplate.Equals(message) &&
                            e.Properties.SequenceEqual(new Dictionary<string, object> {{"A", 1}}) &&
                            e.Exception == exception));
        }

        [Test]
        public void LogMethod_should_use_non_anonymous_class_argument_like_parameter_for_arguments_exception_message_properies([Values] LogLevel level)
        {
            var method = GetLogMethodWithSingleGenericParameter(level, typeof (ILog), typeof (Exception), typeof (string));
            method = method.MakeGenericMethod(new CustomClass().GetType());

            var obj = new CustomClass();
            method.Invoke(null, new object[] {log, exception, message, obj});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, exception, message, obj});
            log.Received(1).Log(Arg.Is<LogEvent>(e => e.Properties.SequenceEqual(new Dictionary<string, object> {{"0", obj}})));
        }

        [Test]
        public void LogMethod_should_work_correctly_for_arguments_exception_message_parameters([Values] LogLevel level)
        {
            var method = GetLogMethod(level, typeof (ILog), typeof (Exception), typeof (string), typeof (object[]));

            var obj = new object();
            method.Invoke(null, new object[] {log, exception, message, new[] {obj}});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, exception, message, new[] {obj}});
            log.Received(1)
                .Log(
                    Arg.Is<LogEvent>(
                        e =>
                            e.Level == level &&
                            HasRoughlyLocalTimestamp(e) &&
                            e.MessageTemplate.Equals(message) &&
                            e.Properties.SequenceEqual(new Dictionary<string, object> {{"0", obj}}) &&
                            e.Exception == exception));
        }

        [Test]
        public void ObsoleteLogMethod_should_work_correctly_for_arguments_message_exception([Values] LogLevel level)
        {
            var method = GetLogMethod(level, typeof (ILog), typeof (string), typeof (Exception));

            method.Invoke(null, new object[] {log, message, exception});
            log.Received(0).Log(Arg.Any<LogEvent>());

            SetLogEnabledForCurrentLevel(level, true);
            method.Invoke(null, new object[] {log, message, exception});
            log.Received(1)
                .Log(
                    Arg.Is<LogEvent>(
                        e =>
                            e.Level == level &&
                            HasRoughlyLocalTimestamp(e) &&
                            e.MessageTemplate.Equals(message) &&
                            e.Properties == null &&
                            e.Exception == exception));
        }

        [Test]
        public void IsEnabledMethod_should_work_correctly_for_level([Values] LogLevel level)
        {
            var method = typeof (LogExtensions).GetMethods().Single(m => m.Name == nameof(ILog.IsEnabledFor) + level);

            method.Invoke(null, new object[] { log });

            log.Received(1).IsEnabledFor(level);
        }

        private static MethodInfo GetLogMethod(LogLevel level, params Type[] argumentsTypes)
        {
            var levelMethods = typeof (LogExtensions).GetMethods().Where(m => m.Name.Equals(level.ToString()));
            return levelMethods.Single(
                m =>
                {
                    if (m.ContainsGenericParameters)
                        return false;

                    var parameters = m.GetParameters().Select(p => p.ParameterType);
                    return parameters.SequenceEqual(argumentsTypes);
                });
        }

        private static MethodInfo GetLogMethodWithSingleGenericParameter(LogLevel level, params Type[] argumentsTypes)
        {
            var levelMethods = typeof (LogExtensions).GetMethods().Where(m => m.Name.Equals(level.ToString()));
            return levelMethods.Single(
                m =>
                {
                    var parameters = m.GetParameters().Select(p => p.ParameterType).ToList();

                    if (parameters.Count(p => p.IsGenericParameter) != 1)
                        return false;

                    if (!parameters.Last().IsGenericParameter)
                        return false;

                    return parameters.Where(p => !p.IsGenericParameter).SequenceEqual(argumentsTypes);
                });
        }

        private static bool HasRoughlyLocalTimestamp(LogEvent @event)
        {
            return Math.Abs((@event.Timestamp.DateTime - DateTime.Now).TotalMinutes) <= 1.0;
        }

        private void SetLogEnabledForCurrentLevel(LogLevel level, bool isEnabled)
        {
            log.IsEnabledFor(level).Returns(isEnabled);
        }

        private class CustomClass
        {
        }
    }
}
