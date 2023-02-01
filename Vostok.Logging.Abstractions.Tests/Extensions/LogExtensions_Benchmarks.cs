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
        private FormattingLog formattingLog;
        
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
            formattingLog = new FormattingLog();
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
        public void WithFormat_WithFiveParameters()
        {
            formattingLog.Info("xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four} ({five}).", one, two, three, four, five);
        }
        
        [Benchmark]
        public void WithFormat_WithFiveParameters_Interpolated()
        {
            formattingLog.Info($"xxx = {one} yyy = {two}, zzzz = {three}, wwwwwww = {four} ({five}).");
        }
        
        [Benchmark]
        public void WithFiveParameters_Interpolated_Formatted()
        {
            log.Info($"xxx = {one:D5} yyy = {two,4}, zzzz = {three,-1}, wwwwwww = {four:x8} ({five:10,P}).");
        }

        /*
BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1415 (21H1/May2021Update)
Intel Core i7-4771 CPU 3.50GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
.NET SDK=6.0.101
  [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
  DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


|                                    Method |       Mean |      Error |     StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------------------ |-----------:|-----------:|-----------:|------:|--------:|-------:|------:|------:|----------:|
|                         WithTwoParameters | 262.280 ns |  5.2234 ns | 12.4139 ns |  1.00 |    0.00 | 0.0763 |     - |     - |     320 B |
|            WithTwoParameters_Interpolated | 264.115 ns |  2.9339 ns |  2.6008 ns |  1.01 |    0.06 | 0.1259 |     - |     - |     528 B |
|                  Silent_WithTwoParameters |  11.456 ns |  0.0617 ns |  0.0547 ns |  0.04 |    0.00 | 0.0153 |     - |     - |      64 B |
|     Silent_WithTwoParameters_Interpolated |   6.847 ns |  0.1678 ns |  0.2512 ns |  0.03 |    0.00 |      - |     - |     - |         - |
|                        WithFourParameters | 329.787 ns |  6.4973 ns |  9.9221 ns |  1.26 |    0.08 | 0.0858 |     - |     - |     360 B |
|           WithFourParameters_Interpolated | 340.609 ns |  4.9230 ns |  4.6050 ns |  1.29 |    0.07 | 0.1683 |     - |     - |     704 B |
|                 Silent_WithFourParameters |  18.043 ns |  0.1761 ns |  0.1561 ns |  0.07 |    0.00 | 0.0249 |     - |     - |     104 B |
|    Silent_WithFourParameters_Interpolated |   6.685 ns |  0.0455 ns |  0.0380 ns |  0.03 |    0.00 |      - |     - |     - |         - |
|                        WithFiveParameters | 366.327 ns |  7.1660 ns |  9.5665 ns |  1.39 |    0.08 | 0.0992 |     - |     - |     416 B |
|           WithFiveParameters_Interpolated | 403.327 ns |  8.0130 ns | 16.0029 ns |  1.54 |    0.10 | 0.1874 |     - |     - |     784 B |
|                 Silent_WithFiveParameters |  23.152 ns |  0.2963 ns |  0.2772 ns |  0.09 |    0.01 | 0.0325 |     - |     - |     136 B |
|    Silent_WithFiveParameters_Interpolated |   7.094 ns |  0.1665 ns |  0.1557 ns |  0.03 |    0.00 |      - |     - |     - |         - |
|              WithFormat_WithFiveParameters| 1,189.309 ns | 23.5291 ns | 37.3197 ns |  5.05 |    0.17 | 0.1793 |     - |     - |     752 B |
| WithFormat_WithFiveParameters_Interpolated|   968.415 ns | 17.2323 ns | 39.9386 ns |  4.17 |    0.33 | 0.2670 |     - |     - |   1,120 B |
| WithFiveParameters_Interpolated_Formatted | 828.264 ns | 15.9405 ns | 20.7272 ns |  3.13 |    0.19 | 0.2117 |     - |     - |     888 B |

         */
    }
}
