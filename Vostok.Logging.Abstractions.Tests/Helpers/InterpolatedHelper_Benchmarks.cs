#if NET6_0_OR_GREATER

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
BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1466 (21H1/May2021Update)
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


|  Method |      Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|-------- |----------:|---------:|---------:|-------:|------:|------:|----------:|
|    Same |  61.58 ns | 2.123 ns | 6.259 ns |      - |     - |     - |         - |
| Escaped | 109.87 ns | 2.288 ns | 5.782 ns | 0.0210 |     - |     - |      88 B |
 */
    }
}

#endif