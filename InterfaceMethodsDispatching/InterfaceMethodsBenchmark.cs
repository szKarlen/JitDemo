using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Configs;

namespace InterfaceMethodsDispatching
{
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    [Config(typeof(Config))]
    public class InterfaceMethodsBenchmark
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                //Add(Job.LegacyJitX86.With(Mode.SingleRun).WithLaunchCount(4).WithWarmupCount(4).WithTargetCount(4));
                //Add(Job.LegacyJitX64.With(Mode.SingleRun).WithLaunchCount(4).WithWarmupCount(4).WithTargetCount(4));
                Add(Job.RyuJitX64.With(Mode.SingleRun).WithLaunchCount(4).WithWarmupCount(4).WithTargetCount(4));
            }
        }

        // just for Turbo Boost
        [Benchmark(Baseline = true, Description = "non-virtual, pre-boot")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallBoot()
        {
            var plain = new Plain();
            for (int i = 0; i < Count; i++)
            {
                plain.Print();
            }
        }

        [Benchmark(Description = "non-virtual")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallNonVirtual()
        {
            var plain = new Plain();
            for (int i = 0; i < Count; i++)
            {
                plain.Print();
            }
        }

        [Params(1000000000)]
        public int Count { get; set; }

        [Benchmark(Description = "monomorphic stub")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallInterfaceStub()
        {
            ICallable callable = new SomeImpl();
            for (int i = 0; i < Count; i++)
            {
                callable.Print();
            }
        }
    }

    public interface ICallable
    {
        void Print();
    }

    class SomeImpl : ICallable
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Print()
        {

        }
    }

    class Plain
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Print()
        {

        }
    }
}