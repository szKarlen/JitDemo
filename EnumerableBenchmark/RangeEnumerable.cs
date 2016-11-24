using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnumerableBenchmark
{
    public class RangeEnumerable : IEnumerable<int>
    {
        private readonly int _start;
        private readonly int _count;

        public RangeEnumerable(int start, int count)
        {
            _start = start;
            _count = count;
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_start, _count);
        }

        public struct Enumerator : IEnumerator<int>
        {
            private int _start;
            private int _count;

            public Enumerator(int start, int count)
            {
                _start = start-1;
                _count = count-1;
            }

            public int Current
            {
                get { return _start; }
            }

            public void Dispose()
            {
                
            }

            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            public bool MoveNext()
            {
                return _start++ < _count;
            }

            public void Reset()
            {
                
            }
        }
    }

    public class YieldBasedEnumerable : IEnumerable<int>
    {
        private readonly int _start;
        private readonly int _count;

        public YieldBasedEnumerable(int start, int count)
        {
            _start = start;
            _count = count;
        }

        public IEnumerator<int> GetEnumerator()
        {
            for (int i = _start; i < _count; i++)
            {
                yield return i;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }

    public class RangeEnumerableWithClassEnumerator : IEnumerable<int>
    {
        private readonly int _start;
        private readonly int _count;

        public RangeEnumerableWithClassEnumerator(int start, int count)
        {
            _start = start;
            _count = count;
        }

        IEnumerator<int> IEnumerable<int>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Enumerator GetEnumerator()
        {
            return new Enumerator(_start, _count);
        }

        public class Enumerator : IEnumerator<int>
        {
            private int _start;
            private int _count;

            public Enumerator(int start, int count)
            {
                _start = start - 1;
                _count = count - 1;
            }

            public int Current
            {
                get { return _start; }
            }

            public void Dispose()
            {

            }

            object System.Collections.IEnumerator.Current
            {
                get { return this.Current; }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                return _start++ < _count;
            }

            public void Reset()
            {

            }
        }
    }
}
