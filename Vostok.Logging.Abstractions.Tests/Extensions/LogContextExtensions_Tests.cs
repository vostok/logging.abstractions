using System;
using System.Diagnostics;
using NSubstitute;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [TestFixture]
    internal class LogContextExtensions_Tests
    {
        [Test]
        public void ForContext_with_shortname_with_nonGeneric_should_use_shortname_as_sourceContext()
        {
            var baseLog = Substitute.For<ILog>();
            baseLog.ForContext<NonGeneric>(useFullTypeName: false);
            baseLog.Received().ForContext(nameof(NonGeneric));
        }

        [Test]
        public void ForContext_with_fullname_with_nonGeneric_should_use_fullname_as_sourceContext()
        {
            var baseLog = Substitute.For<ILog>();
            baseLog.ForContext<NonGeneric>(useFullTypeName: true);
            baseLog.Received().ForContext(typeof(NonGeneric).FullName);
        }

        [Test]
        public void ForContext_with_fullname_with_generic_should_use_fullname_as_sourceContext()
        {
            var baseLog = Substitute.For<ILog>();
            baseLog.ForContext<Generic<NonGeneric>>(useFullTypeName: true);
            baseLog.Received().ForContext(typeof(Generic<NonGeneric>).FullName);
        }

        [Test]
        public void ForContext_with_shortname_with_generic_should_use_clear_shortname_as_sourceContext()
        {
            var baseLog = Substitute.For<ILog>();
            baseLog.ForContext<Generic<NonGeneric>>(useFullTypeName: false);
            baseLog.Received().ForContext($"{nameof(Generic<object>)}`{nameof(NonGeneric)}");
        }

        [Test]
        public void ForContext_with_shortname_with_megaGeneric_should_use_clear_shortname_as_sourceContext()
        {
            var baseLog = Substitute.For<ILog>();
            baseLog.ForContext<MegaGeneric<object, object, object, object, object, object, object, object, object, object>>(useFullTypeName: false);
            baseLog.Received()
                .ForContext(
                    $"{nameof(MegaGeneric<object, object, object, object, object, object, object, object, object, object>)}" +
                    $"`Object`Object`Object`Object`Object`Object`Object`Object`Object`Object");
        }

        [Test]
        public void ForContext_with_shortname_with_nested_generic_should_use_clear_shortname_as_sourceContext()
        {
            var baseLog = Substitute.For<ILog>();
            baseLog.ForContext<Generic<Generic<NonGeneric>>>(useFullTypeName: false);
            baseLog.Received().ForContext($"{nameof(Generic<object>)}`{nameof(Generic<object>)}`{nameof(NonGeneric)}");
        }

        [TestFixture]
        [Explicit]
        public class PerfTests
        {
            [Test]
            public void ForContext_static_generic_perf()
            {
                var baseLog = new SilentLog();
                baseLog.ForContext<Generic<Generic<NonGeneric>>>();

                const int count = 100_000_000;
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < count; i++)
                    baseLog.ForContext<Generic<Generic<NonGeneric>>>();
                sw.Stop();
                Console.Out.WriteLine($"elapsed: {sw.ElapsedMilliseconds}; throughput: {(count * 1000L / sw.ElapsedMilliseconds)} per second");
            }

            [Test]
            public void ForContext_static_nonGeneric_perf()
            {
                var baseLog = new SilentLog();
                baseLog.ForContext<NonGeneric>();

                const int count = 100_000_000;
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < count; i++)
                    baseLog.ForContext<NonGeneric>();
                sw.Stop();
                Console.Out.WriteLine($"elapsed: {sw.ElapsedMilliseconds}; throughput: {(count * 1000L / sw.ElapsedMilliseconds)} per second");
            }

            [Test]
            public void ForContext_dynamic_generic_perf()
            {
                var baseLog = new SilentLog();
                baseLog.ForContext(typeof(Generic<Generic<NonGeneric>>));

                const int count = 100_000;
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < count; i++)
                    baseLog.ForContext(typeof(Generic<Generic<NonGeneric>>));
                sw.Stop();
                Console.Out.WriteLine($"elapsed: {sw.ElapsedMilliseconds}; throughput: {(count * 1000L / sw.ElapsedMilliseconds)} per second");
            }

            [Test]
            public void ForContext_dynamic_nonGeneric_perf()
            {
                var baseLog = new SilentLog();
                baseLog.ForContext(typeof(NonGeneric));

                const int count = 1_000_000;
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < count; i++)
                    baseLog.ForContext(typeof(NonGeneric));
                sw.Stop();
                Console.Out.WriteLine($"elapsed: {sw.ElapsedMilliseconds}; throughput: {(count * 1000L / sw.ElapsedMilliseconds)} per second");
            }
        }

        private class NonGeneric
        {
        }

        private class Generic<T>
        {
        }

        private class MegaGeneric<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
        {
        }
    }
}