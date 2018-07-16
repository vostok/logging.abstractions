using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests
{
    [TestFixture]
    internal class LogEventFormatter_Tests
    {
        [Test]
        public void FormatMessage_should_replace_placeholder_if_such_key_exists_in_props()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{prop}bb", properties).Should().BeEquivalentTo("aavaluebb");
        }

        [Test]
        public void FormatMessage_should_replace_placeholder_if_template_starts_with_it()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("{prop}bb", properties).Should().BeEquivalentTo("valuebb");
        }

        [Test]
        public void FormatMessage_should_replace_placeholder_if_template_ends_with_it()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{prop}", properties).Should().BeEquivalentTo("aavalue");
        }

        [Test]
        public void FormatMessage_should_work_correctly_if_template_ends_with_left_brace()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{", properties).Should().BeEquivalentTo("aa{");
        }

        [Test]
        public void FormatMessage_should_work_correctly_if_template_starts_with_right_brace()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("}aa", properties).Should().BeEquivalentTo("}aa");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_with_doubled_braces()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{{prop}}bb", properties).Should().BeEquivalentTo("aa{prop}bb");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_with_doubled_braces_if_template_starts_with_it()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("{{prop}}bb", properties).Should().BeEquivalentTo("{prop}bb");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_with_doubled_braces_if_template_ends_with_it()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{{prop}}", properties).Should().BeEquivalentTo("aa{prop}");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_if_left_brace_is_doubled()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{{prop}bb", properties).Should().BeEquivalentTo("aa{prop}bb");
        }

        [Test]
        public void FormatMessage_should_replace_placeholder_if_right_brace_is_doubled()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{prop}}bb", properties).Should().BeEquivalentTo("aavalue}bb");
        }

        [Test]
        public void FormatMessage_should_replace_placeholder_with_tripled_braces()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("{{{prop}}}", properties).Should().BeEquivalentTo("{value}");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_with_inverted_braces()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa}prop{bb", properties).Should().BeEquivalentTo("aa}prop{bb");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_with_inverted_braces_if_template_starts_and_ends_with_it()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("}prop{", properties).Should().BeEquivalentTo("}prop{");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_without_right_brace()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{prop", properties).Should().BeEquivalentTo("aa{prop");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_without_right_brace_if_template_starts_with_it()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("{prop", properties).Should().BeEquivalentTo("{prop");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_without_left_brace()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("prop}bb", properties).Should().BeEquivalentTo("prop}bb");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_without_left_brace_if_template_ends_with_it()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("prop}", properties).Should().BeEquivalentTo("prop}");
        }

        [Test]
        public void FormatMessage_should_replace_only_deepest_placeholders()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{bb{prop}cc}dd", properties).Should().BeEquivalentTo("aa{bbvaluecc}dd");
        }

        [Test]
        public void FormatMessage_should_replace_placeholder_after_left_brace_when_separator_between_them_exists()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("aa{bb{prop}cc", properties).Should().BeEquivalentTo("aa{bbvaluecc");
        }

        [Test]
        public void FormatMessage_should_replace_placeholder_before_right_brace_when_separator_between_them_exists()
        {
            var properties = new Dictionary<string, object> { { "prop", "value" } };
            LogEventFormatter.FormatMessage("bb{prop}cc}dd", properties).Should().BeEquivalentTo("bbvaluecc}dd");
        }

        [Test]
        public void FormatMessage_should_not_replace_placeholder_if_such_key_not_exists_in_props()
        {
            var properties = new Dictionary<string, object> { { "other", "value" } };
            LogEventFormatter.FormatMessage("aa{prop}bb", properties).Should().BeEquivalentTo("aa{prop}bb");
        }

        [Test]
        public void FormatMessage_should_not_ignore_case_if_props_are_not_ignorecased()
        {
            var properties = new Dictionary<string, object> { { "Prop", "value" } };
            LogEventFormatter.FormatMessage("aa{prop}bb", properties).Should().BeEquivalentTo("aa{prop}bb");
        }

        [Test]
        public void FormatMessage_should_ignore_case_if_props_are_ignorecased()
        {
            var properties = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase) { { "Prop", "value" } };
            LogEventFormatter.FormatMessage("aa{prop}bb", properties).Should().BeEquivalentTo("aavaluebb");
        }

        [Test]
        public void FormatMessage_should_replace_two_placeholders()
        {
            var properties = new Dictionary<string, object> { { "prop1", "value1" }, { "prop2", "value2" } };
            LogEventFormatter.FormatMessage("aa{prop1}bb{prop2}cc", properties).Should().BeEquivalentTo("aavalue1bbvalue2cc");
        }

        [Test]
        public void FormatMessage_should_replace_two_placeholders_if_they_have_not_separators_between_them()
        {
            var properties = new Dictionary<string, object> { { "prop1", "value1" }, { "prop2", "value2" } };
            LogEventFormatter.FormatMessage("{prop1}{prop2}", properties).Should().BeEquivalentTo("value1value2");
        }

        [Test]
        public void FormatMessage_should_replace_empty_placeholders_if_such_key_exists_in_props()
        {
            var properties = new Dictionary<string, object> { { "", "value" } };
            LogEventFormatter.FormatMessage("aa{}bb", properties).Should().BeEquivalentTo("aavaluebb");
        }

        [Test]
        public void FormatMessage_should_replace_whitespace_placeholders_if_such_key_exists_in_props()
        {
            var properties = new Dictionary<string, object> { { " ", "value" } };
            LogEventFormatter.FormatMessage("aa{ }bb", properties).Should().BeEquivalentTo("aavaluebb");
        }

        [Test]
        public void FormatMessage_should_replace_placeholders_named_like_parameters()
        {
            var properties = new Dictionary<string, object> { { "0", "value1" }, { "1", "value2" } };
            LogEventFormatter.FormatMessage("aa{0}bb{1}cc", properties).Should().BeEquivalentTo("aavalue1bbvalue2cc");
        }

        [Test]
        public void FormatMessage_should_return_null_for_null_template()
        {
            LogEventFormatter.FormatMessage(null, new Dictionary<string, object>()).Should().BeNull();
        }

        [Test]
        public void FormatMessage_should_return_template_for_null_properties()
        {
            LogEventFormatter.FormatMessage("aa{prop}bb", null).Should().BeEquivalentTo("aa{prop}bb");
        }
    }
}