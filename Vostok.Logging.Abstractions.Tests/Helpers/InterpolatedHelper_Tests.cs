#if NET6_0_OR_GREATER
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions.Tests.Helpers
{
    [TestFixture]
    internal class InterpolatedHelper_Tests
    {
        [Test]
        public void EscapeName_should_not_touch_good_string()
        {
            var str = "Hello.My.Friend123";
            var result = InterpolatedHelper.EscapeName(str);
            ReferenceEquals(result, str).Should().BeTrue();
        }

        [Test]
        public void EscapeName_should_replace_bad_symbols()
        {
            var str = "Hello.My.Friend123_ads (1 + 1)";
            var result = InterpolatedHelper.EscapeName(str);
            result.Should().Be("Hello.My.Friend123_ads__1___1_");
        }
    }
}
#endif