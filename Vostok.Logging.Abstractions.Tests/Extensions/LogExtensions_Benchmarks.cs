using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [Explicit]
    public class LogExtensions_Benchmarks
    {
        private ILog log;

        [Test]
        public void RunBenchmark()
        {
            BenchmarkRunner.Run<LogExtensions_Benchmarks>(
                DefaultConfig.Instance
                    .AddDiagnoser(MemoryDiagnoser.Default)
                    .WithOption(ConfigOptions.DisableOptimizationsValidator, true));
        }

        [GlobalSetup]
        public void SetUp()
        {
            log = new DevNullLog();
        }

        [Benchmark(Baseline = true)]
        public void WithTwoParameters()
        {
            log.Info("xxx = {one} yyy = {two}.", "125", "qqq");
        }

        [Benchmark]
        public void WithFourParameters()
        {
            log.Info("xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four}.", "125", "qqq", "qwerty", "42");
        }

        [Benchmark]
        public void WithFiveParameters()
        {
            log.Info("xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four} ({five}).", "125", "qqq", "qwerty", "42", "43");
        }

        /*
        // * Summary*

        BenchmarkDotNet = v0.13.0, OS = Windows 10.0.19043.1415 (21H1/May2021Update)
        Intel Core i7-4771 CPU 3.50GHz(Haswell), 1 CPU, 8 logical and 4 physical cores
        .NET SDK= 6.0.101

        [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
        DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


        |             Method |     Mean |   Error |   StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
        |------------------- |---------:|--------:|---------:|------:|--------:|-------:|------:|------:|----------:|
        |  WithTwoParameters | 319.6 ns | 6.37 ns | 14.11 ns |  1.00 |    0.00 | 0.0896 |     - |     - |     376 B |
        | WithFourParameters | 437.4 ns | 7.86 ns |  7.36 ns |  1.32 |    0.06 | 0.1125 |     - |     - |     472 B |
        | WithFiveParameters | 493.5 ns | 9.72 ns | 11.57 ns |  1.50 |    0.07 | 0.1297 |     - |     - |     544 B |

         */
    }
}
