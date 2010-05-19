using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Parser
{
    public class ParserResult<O, C>
    {
        public O Result { get; internal set; }
        public C Context { get; internal set; }

        public ParserResult(O result, C context)
        {
            this.Result = result;
            this.Context = context;
        }
    }

    public interface IParser<I, O, C>
    {
        ParserResult<O, C> Parse(ref ICloneableEnumerator<I> input, C context);
    }

    public class ParserException<T> : Exception
    {
        public ICloneableEnumerator<T> Input { get; private set; }

        public ParserException(string message, ICloneableEnumerator<T> input)
            : base(message)
        {
            this.Input = input;
        }
    }

    public class Pair<A, B>
    {
        public A Value1 { get; set; }
        public B Value2 { get; set; }

        public Pair()
        {
        }

        public Pair(A value1, B value2)
        {
            this.Value1 = value1;
            this.Value2 = value2;
        }
    }

    public class Pair<T1, T2, T3>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
    }

    public class Pair<T1, T2, T3, T4>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
        public T4 Value4 { get; set; }
    }

    public class Pair<T1, T2, T3, T4, T5>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
        public T4 Value4 { get; set; }
        public T5 Value5 { get; set; }
    }

    public class Pair<T1, T2, T3, T4, T5, T6>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
        public T4 Value4 { get; set; }
        public T5 Value5 { get; set; }
        public T6 Value6 { get; set; }
    }

    public class Pair<T1, T2, T3, T4, T5, T6, T7>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
        public T4 Value4 { get; set; }
        public T5 Value5 { get; set; }
        public T6 Value6 { get; set; }
        public T7 Value7 { get; set; }
    }

    public class Pair<T1, T2, T3, T4, T5, T6, T7, T8>
    {
        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
        public T4 Value4 { get; set; }
        public T5 Value5 { get; set; }
        public T6 Value6 { get; set; }
        public T8 Value8 { get; set; }
    }
}