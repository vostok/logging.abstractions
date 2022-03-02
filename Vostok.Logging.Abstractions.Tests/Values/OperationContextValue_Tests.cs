using System.Collections.Generic;
using System.Linq;
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
        
        [Test]
        public void Plus_operator_should_union_properties()
        {
            var props1 = new Dictionary<string, object>
            {
                ["k1"] = "x"
            };
            var props2 = new Dictionary<string, object>
            {
                ["k2"] = "y"
            };
            var props3 = new Dictionary<string, object>
            {
                ["k1"] = "x",
                ["k2"] = "y"
            };

            var value1 = new OperationContextValue("foo", props1);
            var value2 = value1 + ("bar", props2);

            value2.ToArray().Should().Equal("foo", "bar");
            value2.Properties.Should().BeEquivalentTo(props3);
        }
        
        [Test]
        public void Plus_operator_should_work_with_null_left_properties()
        {
            var props = new Dictionary<string, object>
            {
                ["k1"] = "x"
            };

            var value1 = new OperationContextValue("foo");
            var value2 = value1 + ("bar", props);

            value2.ToArray().Should().Equal("foo", "bar");
            value2.Properties.Should().BeEquivalentTo(props);
        }
        
        [Test]
        public void Plus_operator_should_work_with_null_right_properties()
        {
            var props = new Dictionary<string, object>
            {
                ["k1"] = "x"
            };

            var value1 = new OperationContextValue("foo", props);
            var value2 = value1 + "bar";

            value2.ToArray().Should().Equal("foo", "bar");
            value2.Properties.Should().BeEquivalentTo(props);
        }
    }
}