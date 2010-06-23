using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Parser
{
    public class UnitParser<I, O, C> : IParser<I, O, C>
    {
        private O value;

        public UnitParser(O value)
        {
            this.value = value;
        }

        public ParserResult<O, C> Parse(ref ICloneableEnumerator<I> input, C context)
        {
            return new ParserResult<O, C>(value, context);
        }
    }

    public class SeqParser<I, A, B, C> : IParser<I, Pair<A, B>, C>
    {
        private IParser<I, A, C> a = null;
        private IParser<I, B, C> b = null;

        public SeqParser(IParser<I, A, C> a, IParser<I, B, C> b)
        {
            this.a = a;
            this.b = b;
        }

        public ParserResult<Pair<A, B>, C> Parse(ref ICloneableEnumerator<I> input, C context)
        {
            ICloneableEnumerator<I> temp = input.CloneEnumerator();
            var resultA = a.Parse(ref temp, context);
            var resultB = b.Parse(ref temp, resultA.Context);
            input = temp;
            return new ParserResult<Pair<A, B>, C>(new Pair<A, B>(resultA.Result, resultB.Result), resultB.Context);
        }
    }

    public class AltParser<I, O, C> : IParser<I, O, C>
    {
        private IParser<I, O, C> a = null;
        private IParser<I, O, C> b = null;

        public AltParser(IParser<I, O, C> a, IParser<I, O, C> b)
        {
            this.a = a;
            this.b = b;
        }

        public ParserResult<O, C> Parse(ref ICloneableEnumerator<I> input, C context)
        {
            try
            {
                return a.Parse(ref input, context);
            }
            catch (ParserException<I> eA)
            {
                try
                {
                    return b.Parse(ref input, context);
                }
                catch (ParserException<I> eB)
                {
                    if (eA.Input.CompareTo(eB.Input) < 0)
                    {
                        throw;
                    }
                }
                throw;
            }
        }
    }

    public class LoopParser<I, O, C> : IParser<I, IEnumerable<O>, C>
    {
        private IParser<I, O, C> parser = null;
        private bool loopToEnd = false;

        public LoopParser(IParser<I, O, C> parser, bool loopToEnd)
        {
            this.parser = parser;
            this.loopToEnd = loopToEnd;
        }

        public ParserResult<IEnumerable<O>, C> Parse(ref ICloneableEnumerator<I> input, C context)
        {
            List<O> list = new List<O>();
            while (true)
            {
                try
                {
                    if (input.Available)
                    {
                        ParserResult<O, C> result = parser.Parse(ref input, context);
                        list.Add(result.Result);
                        context = result.Context;
                    }
                    else
                    {
                        break;
                    }
                }
                catch (ParserException<I> e)
                {
                    if (loopToEnd)
                    {
                        throw;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return new ParserResult<IEnumerable<O>, C>(list, context);
        }
    }

    public class ConverterParser<I, A, B, C> : IParser<I, B, C>
    {
        private IParser<I, A, C> parser = null;
        private Func<A, B> converter = null;

        public ConverterParser(IParser<I, A, C> parser, Func<A, B> converter)
        {
            this.parser = parser;
            this.converter = converter;
        }

        public ParserResult<B, C> Parse(ref ICloneableEnumerator<I> input, C context)
        {
            ParserResult<A, C> result = parser.Parse(ref input, context);
            return new ParserResult<B, C>(converter(result.Result), result.Context);
        }
    }

    public class ErrorHandlerParser<I, O, C> : IParser<I, O, C>
    {
        private IParser<I, O, C> parser = null;
        private Func<ParserException<I>, ParserResult<O, C>> handler = null;

        public ErrorHandlerParser(IParser<I, O, C> parser, Func<ParserException<I>, ParserResult<O, C>> handler)
        {
            this.parser = parser;
            this.handler = handler;
        }

        public ParserResult<O, C> Parse(ref ICloneableEnumerator<I> input, C context)
        {
            try
            {
                return parser.Parse(ref input, context);
            }
            catch (ParserException<I> e)
            {
                return handler(e);
            }
        }
    }

    public class RuleParser<I, O, C> : IParser<I, O, C>
    {
        private IParser<I, O, C> parser = null;

        public void Imply(IParser<I, O, C> parser)
        {
            this.parser = parser;
        }

        public ParserResult<O, C> Parse(ref ICloneableEnumerator<I> input, C context)
        {
            return parser.Parse(ref input, context);
        }
    }

    public class TokenParser<I, C> : IParser<Lexer<I>.Token, Lexer<I>.Token, C>
        where I : IComparable
    {
        private I value;
        private string name = "";

        public TokenParser(I value, string name)
        {
            this.value = value;
            this.name = name;
        }

        public ParserResult<Lexer<I>.Token, C> Parse(ref ICloneableEnumerator<Lexer<I>.Token> input, C context)
        {
            if (input.Available && input.Current.Tag.CompareTo(value) == 0)
            {
                Lexer<I>.Token token = input.Current;
                input.MoveNext();
                return new ParserResult<Lexer<I>.Token, C>(token, context);
            }
            else
            {
                throw new ParserException<Lexer<I>.Token>("此处需要" + name + "。", input);
            }
        }
    }

    public class StringParser<I, C> : IParser<Lexer<I>.Token, Lexer<I>.Token, C>
    {
        private string[] values;
        private string name = "";

        public StringParser(string[] values, string name)
        {
            this.values = values;
            this.name = name;
        }

        public ParserResult<Lexer<I>.Token, C> Parse(ref ICloneableEnumerator<Lexer<I>.Token> input, C context)
        {
            if (input.Available)
            {
                string inputValue = input.Current.Value;
                string value = values.Where(s => inputValue == s).FirstOrDefault();
                if (value != null)
                {
                    Lexer<I>.Token token = input.Current;
                    input.MoveNext();
                    return new ParserResult<Lexer<I>.Token, C>(token, context);
                }
            }
            throw new ParserException<Lexer<I>.Token>("此处需要" + name + "。", input);
        }
    }
}
