using BenchmarkDotNet.Running;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumerableBenchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> enumerable = new RangeEnumerable(0, 100000);
            long sum = 0L;
            foreach (var item in enumerable)
            {
                sum += item;
            }
            sum.ToString();
            BenchmarkRunner.Run<EnumerableInterfaceMethodsBenchmark>();
            Console.ReadLine();
        }
    }
}
