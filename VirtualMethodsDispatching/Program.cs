using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualMethodsDispatching
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<VirtualMethodBenchmark>();
            Console.ReadLine();
        }
    }
}
