#if NET6_0_OR_GREATER
using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions.Tests.Helpers
{
    [TestFixture]
    internal class InterpolatedHelper_Tests
    {
        [TestCase("property123", true)]
        [TestCase("Property123.tt", true)]
        [TestCase("Hello.My.Friend123_ads (1 + 1)", false)]
        public void IsValidName_should_work(string name, bool valid)
        {
            InterpolatedHelper.IsValidName(name).Should().Be(valid);
        }
    }
}
#endif