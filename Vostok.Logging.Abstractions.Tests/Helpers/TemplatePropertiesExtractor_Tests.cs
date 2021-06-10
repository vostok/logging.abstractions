using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions.Tests.Helpers
{
    [TestFixture]
    internal class TemplatePropertiesExtractor_Tests
    {
        [TestCase(null)]
        [TestCase("")]
        [TestCase("   ")]
        [TestCase("Hello, world!")]
        [TestCase("!@#$%^&*()")]

        [TestCase("{Hello}", "Hello")]
        [TestCase("{Hello123}", "Hello123")]
        [TestCase("{0}", "0")]
        [TestCase("{1}", "1")]
        [TestCase("{_prop}", "_prop")]
        [TestCase("{_PROP}", "_PROP")]
        [TestCase("{.prop}", ".prop")]

        [TestCase("{prop:format}", "prop")]
        [TestCase("{0:format}", "0")]
        [TestCase("{1:format}", "1")]
        [TestCase("{{prop}}")]
        [TestCase("{{prop:format}}")]

        [TestCase("{{ Hi! }")]
        [TestCase("Well, {{ Hi!")]
        [TestCase("Nice }}-: mo")]

        [TestCase("{0 space}")]
        [TestCase("{:}")]
        [TestCase("{}")]
        [TestCase("{ }")]
        [TestCase("{:format}")]
        [TestCase("{!:format}")]
        [TestCase("{Hello")]
        [TestCase("{,,}")]
        [TestCase("{")]
        [TestCase("}")]

        [TestCase("...{Greeting}, {Name}!", "Greeting", "Name")]
        [TestCase("...{Greeting:D5}, {Name:D5}!", "Greeting", "Name")]
        [TestCase("...{0:D5}, {1:D5}, {0}!", "0", "1", "0")]
        [TestCase("...{prop1}{prop2}{prop3}", "prop1", "prop2", "prop3")]
        public void Should_extract_correct_property_names_from_given_template(string template, params string[] expectedNames)
        {
            TemplatePropertiesExtractor.ExtractPropertyNames(template).Should().Equal(expectedNames);
        }
    }
}