using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests
{
    [Explicit]
    public class LogInfoWithProperties_Benchmarks
    {
        private ILog log;

        [Test]
        public void RunBenchmark()
        {
            BenchmarkRunner.Run<LogInfoWithProperties_Benchmarks>(
                DefaultConfig.Instance
                    .AddDiagnoser(MemoryDiagnoser.Default)
                    .WithOption(ConfigOptions.DisableOptimizationsValidator, true));
        }

        [GlobalSetup]
        public void SetUp()
        {
            log = new DevNullLog();
        }

        [Benchmark]
        public void LogWithObjectProperties()
        {
            var @params = new
            {
                Service = "Print.Api",
                Zone = "default",
                TagFilter = "empty",
                Method = "GET",
                Path = "/print/v1",
                Client = "XML.Handler",
                ClientIp = "127.0.0.1",
                Budget = "00:00:01.200",
                BodySize = "??"
            };
            log.Info("Incoming request to service '{Service}' in zone '{Zone}' with tags filter '{TagFilter}': '{Method} {Path}' from '{Client}' at {RemoteIpAddress} with budget = {Budget}. Body size = {BodySize}.", @params);
        }

        [Benchmark]
        public void LogWithDictionaryProperties()
        {
            var @params = new Dictionary<string, object>()
            {
                {"Service", "Print.Api"},
                {"Zone", "default"},
                {"TagFilter", "empty"},
                {"Method", "GET"},
                {"Path", "/print/v1"},
                {"Client", "XML.Handler"},
                {"ClientIp", "127.0.0.1"},
                {"Budget", "00:00:01.200"},
                {"BodySize", "??"}
            };
            log.Info("Incoming request to service '{Service}' in zone '{Zone}' with tags filter '{TagFilter}': '{Method} {Path}' from '{Client}' at {RemoteIpAddress} with budget = {Budget}. Body size = {BodySize}.", @params);
        }

        /*
        // * Summary *

        BenchmarkDotNet=v0.13.0, OS=Windows 10.0.19043.1415 (21H1/May2021Update)
        Intel Core i7-4771 CPU 3.50GHz (Haswell), 1 CPU, 8 logical and 4 physical cores
        .NET SDK=6.0.101
          [Host]     : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT
          DefaultJob : .NET 6.0.1 (6.0.121.56705), X64 RyuJIT


        |                      Method |       Mean |    Error |   StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
        |---------------------------- |-----------:|---------:|---------:|-------:|------:|------:|----------:|
        |     LogWithObjectProperties |   697.9 ns | 13.21 ns | 21.33 ns | 0.1392 |     - |     - |     584 B |
        | LogWithDictionaryProperties |   892.4 ns |  8.87 ns |  7.41 ns | 0.3319 |     - |     - |   1,392 B |
        */
    }
}