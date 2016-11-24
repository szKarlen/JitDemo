using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Order;

namespace WriteBarrierBenchmark
{
    [OrderProvider(SummaryOrderPolicy.FastestToSlowest)]
    [Config(typeof(Config))]
    public class StackBenchmark
    {
        private class Config : ManualConfig
        {
            public Config()
            {
                Add(Job.RyuJitX64);
                Add(BaselineScaledColumn.ScaledStdDev);
            }
        }

        private Node[] _data;
        private unsafe ValueNode* _allocated;

        [Setup]
        public unsafe void Bootstrap()
        {
            _data = Enumerable.Range(0, this.Count).Select(x => new Node()).ToArray();
            _allocated = (ValueNode*) Marshal.AllocHGlobal(sizeof (ValueNode)*(this.Count+1)).ToPointer();
            RuntimeHelpers.PrepareMethod(typeof(Stack).GetMethod("Push", BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).MethodHandle);
        }

        [Benchmark(Description = "unsafe struct node (preallocated)", Baseline = true)]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public unsafe void PoolBasedValueStack()
        {
            using (var stack = new PoolBasedValueStack(_allocated))
            {
                for (int i = 0; i < this.Count; i++)
                {
                    stack.Push(i);
                }
            }
        }

        [Benchmark(Description = "class-based node (preallocated array)")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void PoolBasedStack()
        {
            using (var stack = new PoolBasedStack(_data))
            {
                for (int i = 0; i < this.Count; i++)
                {
                    stack.Push(i);
                }
            }
        }

        [Benchmark(Description = "unsafe struct node (alloc)")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void ValueStack()
        {
            using (var stack = new ValueStack())
            {
                for (int i = 0; i < this.Count; i++)
                {
                    stack.Push(i);
                }
            }
        }

        [Benchmark(Description = "class-based node (constructor)")]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Stack()
        {
            using (var stack = new Stack())
            {
                for (int i = 0; i < this.Count; i++)
                {
                    stack.Push(i);
                }
            }
        }

        [Params(10000)]
        public int Count { get; set; }
    }
}