using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests.Extensions
{
    [Explicit]
    public class WithPropertyLogExtensions_Benchmarks
    {
        private ILog log;

        [Test]
        public void RunBenchmark()
        {
            BenchmarkRunner.Run<WithPropertyLogExtensions_Benchmarks>(
                DefaultConfig.Instance
                    .AddDiagnoser(MemoryDiagnoser.Default)
                    .WithOption(ConfigOptions.DisableOptimizationsValidator, true));
        }

        [GlobalSetup]
        public void SetUp()
        {
            log = new DevNullLog();
            //For example, tracing module add two properties.
            for (int i = 0; i < 2; i++)
            {
                var i1 = i;
                log = log.WithProperty(i.ToString(), () => i1.ToString());
            }
        }

        [Benchmark]
        public void LogWithPropertiesOnBaseLog()
        {
            log.Info("Empty message");
        }

        /*
        // * Summary *
        
        BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1415 (21H1/May2021Update)
        Intel Core i7-4771 CPU 3.50GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
        .NET SDK=6.0.101
          [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
          DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


        |                     Method |     Mean |   Error |  StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
        |--------------------------- |---------:|--------:|--------:|-------:|------:|------:|----------:|
        | LogWithPropertiesOnBaseLog | 266.7 ns | 2.97 ns | 2.63 ns | 0.0877 |     - |     - |     368 B |

        */
    }
}