using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualMethodsDispatching;

namespace GenericMethodsDispatching
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<GenericsMethodBenchmark>();
            Console.ReadLine();
        }
    }
}
