using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;
using BenchmarkDotNet.Columns;

namespace GenericMethodsDispatching
{
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    [Config(typeof(Config))]
    public class GenericsMethodBenchmark
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
            RuntimeHelpers.PrepareMethod(typeof(DataPrinter<>).MakeGenericType(typeof(object)).GetMethod("Print").MethodHandle, new[] { typeof(object).TypeHandle });
            RuntimeHelpers.PrepareMethod(typeof(DataPrinter).GetMethod("Print").MethodHandle, new[] { typeof(object).TypeHandle });
            RuntimeHelpers.PrepareMethod(typeof(DataPrinterUtils).GetMethod("Print").MethodHandle, new[] { typeof(object).TypeHandle });
            RuntimeHelpers.PrepareMethod(typeof(GenericDerived).GetMethod("Print").MethodHandle, new[] { typeof(object).TypeHandle });
        }

        [Benchmark(Description = "non-virtual", Baseline = true)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallNonVirtual()
        {
            var plain = new Plain();
            for (int i = 0; i < this.Count; i++)
            {
                plain.Print();
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

        [Params(10000000)]
        public int Count { get; set; }

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

    class Plain
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Print()
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