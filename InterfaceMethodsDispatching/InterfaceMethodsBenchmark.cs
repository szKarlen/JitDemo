using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Columns;

namespace InterfaceMethodsDispatching
{
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    [Config(typeof(Config))]
    public class InterfaceMethodsBenchmark
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
            RuntimeHelpers.PrepareMethod(typeof(SomeImpl).GetMethod("Print").MethodHandle);
        }

        [Benchmark(Description = "non-virtual", Baseline = true)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallNonVirtual()
        {
            var plain = new Plain();
            for (int i = 0; i < Count; i++)
            {
                plain.Print();
            }
        }

        [Params(10000000)]
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

        [Benchmark(Description = "generic class")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallGenericType()
        {
            var generic = new DataPrinter<object>();
            for (int i = 0; i < this.Count; i++)
            {
                generic.Print();
            }
        }

        [Benchmark(Description = "generic method")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallGenericMethod()
        {
            var plainGeneric = new DataPrinter();
            for (int i = 0; i < this.Count; i++)
            {
                plainGeneric.Print<object>();
            }
        }

        [Benchmark(Description = "static generic method")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallStaticGenericMethod()
        {
            for (int i = 0; i < this.Count; i++)
            {
                DataPrinterUtils.Print<object>();
            }
        }

        [Benchmark(Description = "generic virtual method")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallGVM()
        {
            GenericBase gvm = new GenericDerived();
            for (int i = 0; i < this.Count; i++)
            {
                gvm.Print<object>();
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

    class DataPrinter
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Print<T>()
        {

        }
    }

    class DataPrinter<T>
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Print()
        {

        }
    }

    static class DataPrinterUtils
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static void Print<T>()
        {

        }
    }

    class GenericBase
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public virtual void Print<T>()
        {

        }
    }

    class GenericDerived : GenericBase
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public override void Print<T>()
        {

        }
    }
}