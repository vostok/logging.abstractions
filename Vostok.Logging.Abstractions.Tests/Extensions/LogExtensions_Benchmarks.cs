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
        public void Silent_WithFiveParameters_Interpolated_Formatted()
        {
            silentLog.Info($"xxx = {one:D5} yyy = {two,4}, zzzz = {three,-1}, wwwwwww = {four:x8} ({five:10,P}).");
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
