using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace GenericMethodsDispatching
{
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest, MethodOrderPolicy.Declared)]
    [Config(typeof(Config))]
    public class GenericsMethodBenchmark
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.RyuJitX64.With(Jit.RyuJit));
            }
        }

        // just for Turbo Boost
        [Benchmark(Baseline = true, Description = "non-virtual, pre-boot")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallBoot()
        {
            var plain = new Plain();
            for (int i = 0; i < this.Count; i++)
            {
                plain.Print();
            }
        }

        [Benchmark(Description = "non-virtual")]
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

        [Params(1000000000)]
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