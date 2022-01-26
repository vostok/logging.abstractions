#if NET6_0

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using NUnit.Framework;
using Vostok.Logging.Abstractions.Helpers;

namespace Vostok.Logging.Abstractions.Tests.Helpers
{
    [Explicit]
    public class InterpolatedHelper_Benchmarks
    {
        [Test]
        public void RunBenchmark()
        {
            BenchmarkRunner.Run<InterpolatedHelper_Benchmarks>(
                DefaultConfig.Instance
                    .AddDiagnoser(MemoryDiagnoser.Default)
                    .WithOption(ConfigOptions.DisableOptimizationsValidator, true));
        }

        [Benchmark]
        public string Same()
        {
            return InterpolatedHelper.EscapeName("GoodName.Without.BadSymbols123");
        }

        [Benchmark]
        public string Escaped()
        {
            return InterpolatedHelper.EscapeName("BadName With BadSymbols ( ) !#");
        }

/*

 */
    }
}

#endif