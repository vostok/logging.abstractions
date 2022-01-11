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


|                                    Method |       Mean |      Error |     StdDev | Ratio | RatioSD |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------------------------------ |-----------:|-----------:|-----------:|------:|--------:|-------:|------:|------:|----------:|
|                         WithTwoParameters | 400.271 ns |  8.4356 ns | 24.7402 ns |  1.00 |    0.00 | 0.0954 |     - |     - |     400 B |
|            WithTwoParameters_Interpolated | 312.648 ns |  6.1385 ns |  7.3075 ns |  0.77 |    0.06 | 0.1259 |     - |     - |     528 B |
|                  Silent_WithTwoParameters |  14.084 ns |  0.1745 ns |  0.1363 ns |  0.03 |    0.00 | 0.0153 |     - |     - |      64 B |
|     Silent_WithTwoParameters_Interpolated |   6.116 ns |  0.1510 ns |  0.1855 ns |  0.02 |    0.00 |      - |     - |     - |         - |
|                        WithFourParameters | 499.726 ns | 10.0272 ns | 12.6812 ns |  1.23 |    0.08 | 0.1240 |     - |     - |     520 B |
|           WithFourParameters_Interpolated | 390.654 ns |  7.7371 ns |  7.2372 ns |  0.94 |    0.06 | 0.1683 |     - |     - |     704 B |
|                 Silent_WithFourParameters |  22.494 ns |  0.3336 ns |  0.6266 ns |  0.06 |    0.00 | 0.0249 |     - |     - |     104 B |
|    Silent_WithFourParameters_Interpolated |   7.453 ns |  0.1573 ns |  0.1394 ns |  0.02 |    0.00 |      - |     - |     - |         - |
|                        WithFiveParameters | 560.296 ns | 10.4555 ns |  9.7801 ns |  1.35 |    0.08 | 0.1469 |     - |     - |     616 B |
|           WithFiveParameters_Interpolated | 431.541 ns |  5.1139 ns |  4.5334 ns |  1.04 |    0.06 | 0.1874 |     - |     - |     784 B |
|                 Silent_WithFiveParameters |  28.108 ns |  0.5276 ns |  0.4935 ns |  0.07 |    0.00 | 0.0325 |     - |     - |     136 B |
|    Silent_WithFiveParameters_Interpolated |   7.790 ns |  0.1407 ns |  0.1248 ns |  0.02 |    0.00 |      - |     - |     - |         - |
| WithFiveParameters_Interpolated_Formatted | 940.528 ns | 18.2511 ns | 23.0818 ns |  2.32 |    0.15 | 0.2117 |     - |     - |     888 B |

         */
    }
}
