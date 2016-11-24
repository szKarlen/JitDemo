using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace EnumerableBenchmark
{
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    [Config(typeof(Config))]
    public class EnumerableInterfaceMethodsBenchmark
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
            RuntimeHelpers.PrepareMethod(typeof(RangeEnumerableWithClassEnumerator.Enumerator).GetMethod("MoveNext", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).MethodHandle);
        }

        [Benchmark(Description = "struct enumerator", Baseline = true)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallStructEnumerator()
        {
            var enumerable = new RangeEnumerable(0, this.Count);
            long sum = 0L;
            foreach (var item in enumerable)
            {
                sum += item;
            }
            sum.ToString();
        }

        [Benchmark(Description = "struct enumerator boxed")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallStructEnumeratorBoxed()
        {
            IEnumerable<int> enumerable = new RangeEnumerable(0, this.Count);
            long sum = 0L;
            foreach (var item in enumerable)
            {
                sum += item;
            }
            sum.ToString();
        }

        [Benchmark(Description = "class enumerator")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallClassBasedEnumerator()
        {
            var enumerable = new RangeEnumerableWithClassEnumerator(0, this.Count);
            long sum = 0L;
            foreach (var item in enumerable)
            {
                sum += item;
            }
            sum.ToString();
        }

        [Benchmark(Description = "linq enumerable")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallLINQEnumerable()
        {
            var enumerable = Enumerable.Range(0, this.Count);
            long sum = 0L;
            foreach (var item in enumerable)
            {
                sum += item;
            }
            sum.ToString();
        }

        [Params(10000000)]
        public int Count { get; set; }

        [Benchmark(Description = "yield-based enumerator")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void CallYieldEnumerable()
        {
            var enumerable = new YieldBasedEnumerable(0, this.Count);
            long sum = 0L;
            foreach (var item in enumerable)
            {
                sum += item;
            }
            sum.ToString();
        }
    }
}