using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Values;

namespace Vostok.Logging.Abstractions.Tests.Values
{
    [TestFixture]
    internal class SourceContextValue_Tests
    {
        [Test]
        public void Should_implement_IReadOnlyList()
        {
            new SourceContextValue("foo").Should().BeAssignableTo<IReadOnlyList<string>>();
        }

        [Test]
        public void ToString_with_just_one_context_should_return_it()
        {
            new SourceContextValue("foo").ToString().Should().Be("foo");
        }

        [Test]
        public void ToString_with_multiple_contexts_should_join_them()
        {
            new SourceContextValue(new [] {"foo", "bar", "baz"}).ToString()
                .Should().Be("foo -> bar -> baz");
        }

        [Test]
        public void Plus_operator_for_string_should_handle_null_left_value()
        {
            (null as SourceContextValue + "foo")
                .Should().Equal("foo");
        }

        [Test]
        public void Plus_operator_for_string_should_handle_null_right_value()
        {
            (new SourceContextValue("foo") + (null as string))
                .Should().Equal("foo");
        }

        [Test]
        public void Plus_operator_for_string_should_handle_empty_right_value()
        {
            (new SourceContextValue("foo") + string.Empty)
                .Should().Equal("foo");
        }

        [Test]
        public void Plus_operator_for_string_should_concatenate_contexts()
        {
            (new SourceContextValue("foo") + "bar" + "baz")
                .Should().Equal("foo", "bar", "baz");
        }

        [Test]
        public void Plus_operator_for_value_should_handle_null_left_value()
        {
            (null + new SourceContextValue("foo"))
                .Should().Equal("foo");
        }

        [Test]
        public void Plus_operator_for_value_should_handle_null_right_value()
        {
            (new SourceContextValue("foo") + (null as SourceContextValue))
                .Should().Equal("foo");
        }

        [Test]
        public void Plus_operator_for_value_should_concatenate_contexts()
        {
            (new SourceContextValue("foo") + new SourceContextValue("bar") + new SourceContextValue("baz"))
                .Should().Equal("foo", "bar", "baz");
        }
    }
}