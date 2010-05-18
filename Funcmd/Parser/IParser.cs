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

    public class Pair<A, B>
    {
        public A First { get; set; }
        public B Second { get; set; }

        public Pair(A first, B second)
        {
            this.First = first;
            this.Second = second;
        }
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
}