using FluentAssertions;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions.Tests
{
    [TestFixture]
    internal class CharArrayCache_Tests
    {
        [TestCase(0)]
        [TestCase(1)]
        [TestCase(16)]
        [TestCase(100)]
        [TestCase(1000)]
        public void Acquire_should_always_provide_an_array_of_at_least_requested_capacity(int capacity)
        {
            CharArrayCache.Acquire(capacity).Length.Should().BeGreaterOrEqualTo(capacity);
        }

        [Test]
        public void Acquire_should_hand_out_different_instances_when_called_multiple_times_in_a_row_from_same_thread()
        {
            var array1 = CharArrayCache.Acquire(10);
            var array2 = CharArrayCache.Acquire(10);
            var array3 = CharArrayCache.Acquire(10);

            array1.Should().NotBeSameAs(array2);
            array1.Should().NotBeSameAs(array3);
        }

        [Test]
        public void Acquire_should_cache_and_reuse_returned_instances()
        {
            var array = CharArrayCache.Acquire(10);

            for (var i = 0; i < 10; i++)
            {
                CharArrayCache.Return(array);

                CharArrayCache.Acquire(10).Should().BeSameAs(array);
            }
        }
    }
}