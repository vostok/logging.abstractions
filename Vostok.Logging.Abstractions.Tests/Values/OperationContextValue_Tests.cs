using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Values;

namespace Vostok.Logging.Abstractions.Tests.Values
{
    [TestFixture]
    internal class OperationContextValue_Tests
    {
        [Test]
        public void Should_implement_IReadOnlyList()
        {
            new OperationContextValue("foo").Should().BeAssignableTo<IReadOnlyList<string>>();
        }

        [Test]
        public void ToString_with_just_one_context_should_return_it_wrapped_in_square_braces()
        {
            new OperationContextValue("foo").ToString().Should().Be("[foo]");
        }

        [Test]
        public void ToString_with_multiple_contexts_should_join_them()
        {
            new OperationContextValue(new[] { "foo", "bar", "baz" }).ToString()
                .Should().Be("[foo] [bar] [baz]");
        }

        [Test]
        public void Plus_operator_for_string_should_handle_null_left_value()
        {
            ((null as OperationContextValue) + "foo")
                .Should().Equal("foo");
        }

        [Test]
        public void Plus_operator_for_string_should_handle_null_right_value()
        {
            (new OperationContextValue("foo") + null)
                .Should().Equal("foo");
        }

        [Test]
        public void Plus_operator_for_string_should_handle_empty_right_value()
        {
            (new OperationContextValue("foo") + string.Empty)
                .Should().Equal("foo");
        }

        [Test]
        public void Plus_operator_for_string_should_concatenate_contexts()
        {
            (new OperationContextValue("foo") + "bar" + "baz")
                .Should().Equal("foo", "bar", "baz");
        }
    }
}