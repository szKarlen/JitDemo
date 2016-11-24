using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace WriteBarrierBenchmark
{
    public unsafe struct ValueNode
    {
        public int Value { get; set; }
        public ValueNode* Next { get; set; }
    }

    public unsafe class PoolBasedValueStack : IDisposable 
    {
        ValueNode* first;
        private ValueNode* data;

        public PoolBasedValueStack(ValueNode* allocated)
        {
            data = allocated;
            first = allocated;
        }

        public int Count { get; private set; }

        public void Push(int value)
        {
            var oldFirst = first;
            first = &data[Count++];
            first->Value = value;
            first->Next = oldFirst;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Dispose()
        {
            
        }
    }

    public unsafe class ValueStack : IDisposable
    {
        ValueNode* first;

        public void Push(int value)
        {
            var oldFirst = first;
            first = (ValueNode*)(Marshal.AllocHGlobal(sizeof(ValueNode)));
            first->Value = value;
            first->Next = oldFirst;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Dispose()
        {

        }
    }

    public class Node
    {
        public int Value { get; set; }
        public Node Next { get; set; }
    }

    public class PoolBasedStack : IDisposable
    {
        Node first;
        private readonly Node[] _data;

        public PoolBasedStack(Node[] data)
        {
            _data = data;
        }

        public int Count { get; private set; }

        public void Push(int value)
        {
            var oldFirst = first;
            first = _data[this.Count++];
            first.Value = value;
            first.Next = oldFirst;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Dispose()
        {

        }
    }

    public class Stack : IDisposable
    {
        Node first;

        public int Count { get; private set; }

        public void Push(int value)
        {
            var oldFirst = first;
            first = new Node();
            first.Value = value;
            first.Next = oldFirst;
            Count++;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public void Dispose()
        {

        }
    }
}
