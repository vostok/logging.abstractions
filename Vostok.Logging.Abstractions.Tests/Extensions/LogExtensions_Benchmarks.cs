using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [Explicit]
    public class LogExtensions_Benchmarks
    {
        private DevNullLog log;
        private SilentLog silentLog;
        
        private int one = 125;
        private string two = "qqq";
        private string three = "qwerty";
        private int four = 42;
        private int five = 43;

        [Test]
        public void RunBenchmark()
        {
            BenchmarkRunner.Run<LogExtensions_Benchmarks>(
                DefaultConfig.Instance
                    .AddDiagnoser(MemoryDiagnoser.Default)
                    .WithOption(ConfigOptions.DisableOptimizationsValidator, true));
        }

        [Test]
        public void Should_log_same_events()
        {
            Check(WithTwoParameters, WithTwoParameters_Interpolated);
            Check(WithFourParameters, WithFourParameters_Interpolated);
            Check(WithFiveParameters, WithFiveParameters_Interpolated);

            void Check(Action log1, Action log2)
            {
                log = new DevNullLog();
                log1();
                var event1 = log.LastEvent;
                log2();
                var event2 = log.LastEvent;
                
                event1.Should().BeEquivalentTo(event2, config => config.Excluding(e => e.Timestamp));
            }
        }

        [GlobalSetup]
        public void SetUp()
        {
            log = new DevNullLog();
            silentLog = new SilentLog();
        }

        [Benchmark(Baseline = true)]
        public void WithTwoParameters()
        {
            log.Info("xxx = {one} yyy = {two}.", one, two);
        }
        
        [Benchmark]
        public void WithTwoParameters_Interpolated()
        {
            log.Info($"xxx = {one} yyy = {two}.");
        }
        
        [Benchmark]
        public void Silent_WithTwoParameters()
        {
            silentLog.Info("xxx = {one} yyy = {two}.", one, two);
        }
        
        [Benchmark]
        public void Silent_WithTwoParameters_Interpolated()
        {
            silentLog.Info($"xxx = {one} yyy = {two}.");
        }

        [Benchmark]
        public void WithFourParameters()
        {
            log.Info("xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four}.", one, two, three, four);
        }
        
        [Benchmark]
        public void WithFourParameters_Interpolated()
        {
            log.Info($"xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four}.");
        }
        
        [Benchmark]
        public void Silent_WithFourParameters()
        {
            silentLog.Info("xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four}.", one, two, three, four);
        }
        
        [Benchmark]
        public void Silent_WithFourParameters_Interpolated()
        {
            silentLog.Info($"xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four}.");
        }

        [Benchmark]
        public void WithFiveParameters()
        {
            log.Info("xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four} ({five}).", one, two, three, four, five);
        }
        
        [Benchmark]
        public void WithFiveParameters_Interpolated()
        {
            log.Info($"xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four} ({five}).");
        }
        
        [Benchmark]
        public void Silent_WithFiveParameters()
        {
            silentLog.Info("xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four} ({five}).", one, two, three, four, five);
        }
        
        [Benchmark]
        public void Silent_WithFiveParameters_Interpolated()
        {
            silentLog.Info($"xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four} ({five}).");
        }
        
        [Benchmark]
        public void WithFiveParameters_Interpolated_Formatted()
        {
            log.Info($"xxx = {one:D5} yyy = {two,4}, zzzz = {three,-1}, wwwwwww = {four:x8} ({five:10,P}).");
        }

        /*

BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1415 (21H1/May2021Update)
Intel Core i7-6700 CPU 3.40GHz (Skylake), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


|                                    Method |         Mean |      Error |     StdDev |       Median | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------------------ |-------------:|-----------:|-----------:|-------------:|------:|--------:|-------:|------:|------:|----------:|
|                         WithTwoParameters |   340.965 ns |  5.6540 ns |  7.1505 ns |   339.929 ns |  1.00 |    0.00 | 0.0954 |     - |     - |     400 B |
|            WithTwoParameters_Interpolated |   385.489 ns |  7.5835 ns | 14.6108 ns |   380.326 ns |  1.13 |    0.05 | 0.1450 |     - |     - |     608 B |
|                  Silent_WithTwoParameters |    15.336 ns |  0.3035 ns |  0.2690 ns |    15.305 ns |  0.04 |    0.00 | 0.0153 |     - |     - |      64 B |
|     Silent_WithTwoParameters_Interpolated |     5.191 ns |  0.1346 ns |  0.1383 ns |     5.161 ns |  0.02 |    0.00 |      - |     - |     - |         - |
|                        WithFourParameters |   511.856 ns |  9.1225 ns | 20.2149 ns |   509.361 ns |  1.50 |    0.08 | 0.1240 |     - |     - |     520 B |
|           WithFourParameters_Interpolated |   550.226 ns | 10.6584 ns | 11.8468 ns |   550.542 ns |  1.61 |    0.04 | 0.2060 |     - |     - |     864 B |
|                 Silent_WithFourParameters |    25.784 ns |  0.4800 ns |  0.5335 ns |    25.730 ns |  0.08 |    0.00 | 0.0249 |     - |     - |     104 B |
|    Silent_WithFourParameters_Interpolated |     5.800 ns |  0.1548 ns |  0.2751 ns |     5.749 ns |  0.02 |    0.00 |      - |     - |     - |         - |
|                        WithFiveParameters |   544.228 ns | 10.8947 ns |  8.5059 ns |   543.793 ns |  1.58 |    0.03 | 0.1469 |     - |     - |     616 B |
|           WithFiveParameters_Interpolated |   582.414 ns |  9.8156 ns |  7.6633 ns |   583.068 ns |  1.70 |    0.02 | 0.2346 |     - |     - |     984 B |
|                 Silent_WithFiveParameters |    33.668 ns |  1.1894 ns |  3.4317 ns |    32.255 ns |  0.10 |    0.01 | 0.0325 |     - |     - |     136 B |
|    Silent_WithFiveParameters_Interpolated |     5.371 ns |  0.1444 ns |  0.1928 ns |     5.332 ns |  0.02 |    0.00 |      - |     - |     - |         - |
| WithFiveParameters_Interpolated_Formatted | 1,191.122 ns | 29.1080 ns | 84.4475 ns | 1,185.662 ns |  3.55 |    0.31 | 0.2594 |     - |     - |   1,088 B |

         */
    }
}
