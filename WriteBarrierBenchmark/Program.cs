using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace WriteBarrierBenchmark
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            BenchmarkRunner.Run<StackBenchmark>();
            Console.ReadLine();
        }
    }
}
