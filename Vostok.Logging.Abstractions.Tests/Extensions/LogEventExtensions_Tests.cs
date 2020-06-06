using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Commons.Collections;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class LogEventExtensions_Tests
    {
        private LogEvent eventBefore;
        private LogEvent eventAfter;

        [SetUp]
        public void TestSetup()
        {
            eventBefore = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null)
                .WithProperty("A", 1)
                .WithProperty("B", 2);
        }
      
        [Test]
        public void WithObjectProperties_should_return_same_event_when_params_array_is_null()
        {
            eventAfter = eventBefore.WithObjectProperties(null as object);

            eventAfter.Should().BeSameAs(eventBefore);
        }

        [Test]
        public void WithObjectProperties_should_enrich_event_with_named_properties_from_anonymous_constructed_object()
        {
            eventAfter = eventBefore.WithObjectProperties(new { C = "value", D = null as object, E = 123 });

            eventAfter.Properties.Should().HaveCount(5);
            eventAfter?.Properties?["C"].Should().Be("value");
            eventAfter?.Properties?["D"].Should().BeNull();
            eventAfter?.Properties?["E"].Should().Be(123);
        }

        [Test]
        public void WithObjectProperties_should_enrich_event_with_named_properties_from_a_dictionary()
        {
            eventAfter = eventBefore.WithObjectProperties(new Dictionary<string, object>
            {
                ["C"] = "value",
                ["D"] = null,
                ["E"] = 123
            });

            eventAfter.Properties.Should().HaveCount(5);
            eventAfter?.Properties?["C"].Should().Be("value");
            eventAfter?.Properties?["D"].Should().BeNull();
            eventAfter?.Properties?["E"].Should().Be(123);
        }

        [Test]
        public void WithObjectProperties_should_ignore_private_properties()
        {
            eventAfter = eventBefore.WithObjectProperties(new ClassWithPrivateProperty());

            eventAfter.Properties.Should().HaveCount(2);
            eventAfter.Properties?.ContainsKey("Property").Should().BeFalse();
        }

        [Test]
        public void WithObjectProperties_should_ignore_static_properties()
        {
            eventAfter = eventBefore.WithObjectProperties(new ClassWithStaticProperty());

            eventAfter.Properties.Should().HaveCount(2);
            eventAfter.Properties?.ContainsKey("Property").Should().BeFalse();
        }

        [Test]
        public void WithObjectProperties_should_skip_properties_with_private_getters()
        {
            eventAfter = eventBefore.WithObjectProperties(new ClassWithPrivateGetterProperty());

            eventAfter.Properties.Should().HaveCount(2);
        }

        [Test]
        public void WithObjectProperties_should_put_special_values_for_faulty_properties()
        {
            eventAfter = eventBefore.WithObjectProperties(new ClassWithFaultyProperty());

            eventAfter.Properties.Should().HaveCount(5);
            eventAfter.Properties?["Property1"].Should().Be(1);
            eventAfter.Properties?["Property2"].Should().Be(2);
            eventAfter.Properties?["Property3"].Should().BeOfType<string>().Which.Should().StartWith("<error: ");
        }

        [Test]
        public void WithParameters_should_return_same_event_when_params_array_is_null()
        {
            eventAfter = eventBefore.WithParameters(null);

            eventAfter.Should().BeSameAs(eventBefore);
        }

        [Test]
        public void WithParameters_should_return_same_event_when_params_array_is_empty()
        {
            eventAfter = eventBefore.WithParameters(new object[]{});

            eventAfter.Should().BeSameAs(eventBefore);
        }

        [Test]
        public void WithParameters_should_enrich_event_with_index_named_properties_when_message_template_is_null()
        {
            eventAfter = eventBefore.WithParameters(new object[] { "value", null, 123 });

            eventAfter.Properties.Should().HaveCount(5);
            eventAfter?.Properties?["0"].Should().Be("value");
            eventAfter?.Properties?["1"].Should().BeNull();
            eventAfter?.Properties?["2"].Should().Be(123);
        }

        [Test]
        public void WithParameters_should_enrich_event_with_index_named_properties_when_message_template_contains_only_positional_placeholders()
        {
            SetMessageTemplate("..{0}..{1}..{2}");

            eventAfter = eventBefore.WithParameters(new object[] { "value", null, 123 });

            eventAfter.Properties.Should().HaveCount(5);
            eventAfter?.Properties?["0"].Should().Be("value");
            eventAfter?.Properties?["1"].Should().BeNull();
            eventAfter?.Properties?["2"].Should().Be(123);
        }

        [Test]
        public void WithParameters_should_enrich_event_with_index_named_properties_when_message_template_contains_less_placeholders_than_there_are_parameters()
        {
            SetMessageTemplate("..{0}..{1}");

            eventAfter = eventBefore.WithParameters(new object[] { "value", null, 123 });

            eventAfter.Properties.Should().HaveCount(5);
            eventAfter?.Properties?["0"].Should().Be("value");
            eventAfter?.Properties?["1"].Should().BeNull();
            eventAfter?.Properties?["2"].Should().Be(123);
        }

        [Test]
        public void WithParameters_should_enrich_event_with_index_named_properties_when_message_template_contains_more_placeholders_than_there_are_parameters()
        {
            SetMessageTemplate("..{0}..{1}..{0}..{1}");

            eventAfter = eventBefore.WithParameters(new object[] { "value", null, 123 });

            eventAfter.Properties.Should().HaveCount(5);
            eventAfter?.Properties?["0"].Should().Be("value");
            eventAfter?.Properties?["1"].Should().BeNull();
            eventAfter?.Properties?["2"].Should().Be(123);
        }

        [Test]
        public void WithParameters_should_infer_property_names_from_message_template_on_perfect_match()
        {
            SetMessageTemplate("..{prop1}..{prop2}..{prop3}");

            eventAfter = eventBefore.WithParameters(new object[] { "value", null, 123 });

            eventAfter.Properties.Should().HaveCount(5);
            eventAfter?.Properties?["prop1"].Should().Be("value");
            eventAfter?.Properties?["prop2"].Should().BeNull();
            eventAfter?.Properties?["prop3"].Should().Be(123);
        }

        [Test]
        public void WithParameters_should_infer_property_names_from_message_template_even_when_provided_with_less_parameters_than_expected()
        {
            SetMessageTemplate("..{prop1}..{prop2}..{prop3}");

            eventAfter = eventBefore.WithParameters(new object[] { "value", 123 });

            eventAfter.Properties.Should().HaveCount(4);
            eventAfter?.Properties?["prop1"].Should().Be("value");
            eventAfter?.Properties?["prop2"].Should().Be(123);
        }

        [Test]
        public void WithParameters_should_infer_property_names_from_message_template_even_when_provided_with_more_parameters_than_expected()
        {
            SetMessageTemplate("..{prop1}..{prop2}..{prop3}");

            eventAfter = eventBefore.WithParameters(new object[] { 1, 2, 3, 4});

            eventAfter.Properties.Should().HaveCount(6);
            eventAfter?.Properties?["prop1"].Should().Be(1);
            eventAfter?.Properties?["prop2"].Should().Be(2);
            eventAfter?.Properties?["prop3"].Should().Be(3);
            eventAfter?.Properties?["3"].Should().Be(4);
        }

        private void SetMessageTemplate(string template)
        {
            eventBefore = new LogEvent(eventBefore.Level, eventBefore.Timestamp, template, (ImmutableArrayDictionary<string, object>) eventBefore.Properties, eventBefore.Exception);
        }

        private class ClassWithPrivateProperty
        {
            private int Property { get; set; } = 1;
        }

        private class ClassWithPrivateGetterProperty
        {
            public int Property { private get; set; } = 1;
        }

        private class ClassWithStaticProperty
        {
            public static int Property { get; set; } = 1;
        }

        private class ClassWithFaultyProperty
        {
            public int Property1 => 1;
            public int Property2 => 2;
            public int Property3 => throw new InvalidOperationException();
        }
    }
}