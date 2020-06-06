using System;
using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions.Tests.Helpers
{
    [TestFixture]
    internal class DeconstructionHelper_Tests
    {
        private const string Template1 = "{Property}";
        private const string Template2 = "{Property1} {Property2}";
        private const string Template3 = "{Property1} {Property2} {Property3}";

        [Test]
        public void Should_not_deconstruct_nulls()
            => DeconstructionHelper.ShouldDeconstruct(Template1, null as string).Should().BeFalse();

        [Test]
        public void Should_deconstruct_dictionaries_of_string_to_object()
            => DeconstructionHelper.ShouldDeconstruct(Template1, new Dictionary<string, object>()).Should().BeTrue();

        [Test]
        public void Should_not_deconstruct_collections()
        {
            DeconstructionHelper.ShouldDeconstruct(Template1, new int[]{}).Should().BeFalse();
            DeconstructionHelper.ShouldDeconstruct(Template1, new string[]{}).Should().BeFalse();
            DeconstructionHelper.ShouldDeconstruct(Template1, new ArrayList()).Should().BeFalse();
            DeconstructionHelper.ShouldDeconstruct(Template1, new List<LogEvent>()).Should().BeFalse();
            DeconstructionHelper.ShouldDeconstruct(Template1, new Dictionary<int, string>()).Should().BeFalse();
        }

        [Test]
        public void Should_not_deconstruct_primitives_and_enums()
        {
            DeconstructionHelper.ShouldDeconstruct(Template1, 1).Should().BeFalse();
            DeconstructionHelper.ShouldDeconstruct(Template1, 1L).Should().BeFalse();
            DeconstructionHelper.ShouldDeconstruct(Template1, 1d).Should().BeFalse();
            DeconstructionHelper.ShouldDeconstruct(Template1, "").Should().BeFalse();
            DeconstructionHelper.ShouldDeconstruct(Template1, Base64FormattingOptions.InsertLineBreaks).Should().BeFalse();
        }

        [Test]
        public void Should_not_deconstruct_types_with_custom_tostring_method()
            => DeconstructionHelper.ShouldDeconstruct(Template1, new TypeWithToString()).Should().BeFalse();

        [Test]
        public void Should_deconstruct_anonymous_types()
            => DeconstructionHelper.ShouldDeconstruct(Template1, new { A = 1}).Should().BeTrue();

        [Test]
        public void Should_deconstruct_arbitrary_types_when_there_is_more_than_one_property_in_the_template()
        {
            DeconstructionHelper.ShouldDeconstruct(Template1, new Dto()).Should().BeFalse();
            DeconstructionHelper.ShouldDeconstruct(Template2, new Dto()).Should().BeTrue();
            DeconstructionHelper.ShouldDeconstruct(Template3, new Dto()).Should().BeTrue();
        }

        private class TypeWithToString
        {
            public override string ToString() => "";
        }

        private class Dto
        {
        }
    }
}
