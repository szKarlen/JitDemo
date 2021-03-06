﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace VirtualMethodsDispatching
{
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    [Config(typeof(Config))]
    public class VirtualMethodBenchmark
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.RyuJitX64);
                Add(BaselineScaledColumn.ScaledStdDev);
            }
        }

        [Setup]
        public void Setup()
        {
            RuntimeHelpers.PrepareMethod(typeof(Plain).GetMethod("Print").MethodHandle);
        }

        // just for Turbo Boost
        [Benchmark(Baseline = true, Description = "non-virtual")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallBoot()
        {
            var plain = new Plain();
            for (int i = 0; i < Count; i++)
            {
                plain.Print();
            }
        }

        [Params(10000000)]
        public int Count { get; set; }

        [Benchmark(Description = "virtual (base-derived)")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallDerived()
        {
            Base derived = new Derived();
            for (int i = 0; i < Count; i++)
            {
                derived.Print();
            }
        }
    }

    abstract class Base
    {
        public abstract void Print();
    }

    class Derived : Base
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override void Print()
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