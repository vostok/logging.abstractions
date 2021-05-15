using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using NUnit.Framework;

namespace Vostok.Logging.Abstractions.Tests
{
    [TestFixture]
    [Explicit]
    public class LogEventExtensionsBenchmark
    {
        [Test]
        public void LogEventExtensionsBenchmark_WithObjectProperties() => BenchmarkRunner.Run<LogEventExtensionsBenchmark_WithObjectProperties>();
       
        [Test]
        public void LogEventExtensionsBenchmark_WithParameters() => BenchmarkRunner.Run<LogEventExtensionsBenchmark_WithParameters>();
    }

    [MemoryDiagnoser]
    public abstract class LogEventExtensionsBenchmark_WithParameters
    {
        [Params(1, 4, 8)]
        public int Count;

        [Params(true, false)]
        public bool Preallocate;

        private LogEvent initialLogEvent;
        private object parameters;

        protected LogEventExtensionsBenchmark_WithParameters()
        {
        }

        [GlobalSetup]
        public void Setup()
        {
            LogEventGlobalOptions.InitialPropertiesCapacity = Preallocate ? 10 : 0;

            if (Count == 8)
            {
                parameters = new
                {
                    Service = Guid.NewGuid().ToString(),
                    Zone = Guid.NewGuid().ToString(),
                    Method = Guid.NewGuid().ToString(),
                    Path = Guid.NewGuid().ToString(),
                    Client = Guid.NewGuid().ToString(),
                    RemoteIpAddress = Guid.NewGuid().ToString(),
                    Budget = Guid.NewGuid().ToString(),
                    BodySize = "??"
                };
            }

            if (Count == 4)
            {
                parameters = new
                {
                    Service = Guid.NewGuid().ToString(),
                    Zone = Guid.NewGuid().ToString(),
                    Method = Guid.NewGuid().ToString(),
                    BodySize = "??"
                };

            }

            if (Count == 1)
            {
                parameters = new
                {
                    Service = Guid.NewGuid().ToString(),
                };

            }

            initialLogEvent = new LogEvent(LogLevel.Info, DateTimeOffset.Now, "Template");
        }

        [Benchmark]
        public LogEvent WithObjectProperties()
        {
            return initialLogEvent.WithObjectProperties(parameters);
        }

        //[Benchmark]
        //public LogEvent WithSetRangeObjectProperties()
        //{
        //    return initialLogEvent.WithSetRangeObjectProperties(parameters);
        //}
    }



    [MemoryDiagnoser]
    public abstract class LogEventExtensionsBenchmark_WithObjectProperties
    {
        [Params(1, 2, 3, 4, 5, 6, 7, 8, 9, 10)]
        public int Count;

        [Params(true, false)]
        public bool Preallocate;

        private LogEvent initialLogEvent;
        private object[] parameters;

        protected LogEventExtensionsBenchmark_WithObjectProperties()
        {
        }

        [GlobalSetup]
        public void Setup()
        {
            LogEventGlobalOptions.InitialPropertiesCapacity = Preallocate ? Count : 0;

            parameters = new object[Count];
            for (var i = 0; i < parameters.Length; i++)
                parameters[i] = Guid.NewGuid().ToString();

            initialLogEvent = new LogEvent(LogLevel.Info, DateTimeOffset.Now, "Template");
        }

        [Benchmark]
        public LogEvent WithParameters()
        {
            return initialLogEvent.WithParameters(parameters);
        }

        //[Benchmark]
        //public LogEvent WithParametersSetRange()
        //{
        //    return initialLogEvent.WithSetRangeParameters(parameters);
        //}
        //
        //[Benchmark]
        //public LogEvent WithParametersSetRangeUnsafe()
        //{
        //    return initialLogEvent.WithSetRangeParametersUnsafe(parameters);
        //}
    }
}
