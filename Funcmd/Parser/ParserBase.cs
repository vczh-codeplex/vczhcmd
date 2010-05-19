using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Parser
{
    public class ParserBase<I, C>
    {
        protected IParser<I, O, C> Alt<O>(params IParser<I, O, C>[] ps)
        {
            IParser<I, O, C> result = ps.First();
            foreach (IParser<I, O, C> p in ps.Skip(1))
            {
                result = result.Alt(p);
            }
            return result;
        }

        protected IParser<I, Tuple<T1, T2>, C> Seq<T1, T2>(
            IParser<I, T1, C> p1,
            IParser<I, T2, C> p2)
        {
            return p1.Join(p2).Convert(p => new Tuple<T1, T2>
                {
                    Value1 = p.First,
                    Value2 = p.Second
                });
        }

        protected IParser<I, Tuple<T1, T2, T3>, C> Seq<T1, T2, T3>(
            IParser<I, T1, C> p1,
            IParser<I, T2, C> p2,
            IParser<I, T3, C> p3)
        {
            return p1.Join(p2).Join(p3).Convert(p => new Tuple<T1, T2, T3>
            {
                Value1 = p.First.First,
                Value2 = p.First.Second,
                Value3 = p.Second
            });
        }

        protected IParser<I, Tuple<T1, T2, T3, T4>, C> Seq<T1, T2, T3, T4>(
            IParser<I, T1, C> p1,
            IParser<I, T2, C> p2,
            IParser<I, T3, C> p3,
            IParser<I, T4, C> p4)
        {
            return p1.Join(p2).Join(p3).Join(p4).Convert(p => new Tuple<T1, T2, T3, T4>
            {
                Value1 = p.First.First.First,
                Value2 = p.First.First.Second,
                Value3 = p.First.Second,
                Value4 = p.Second
            });
        }

        protected IParser<I, Tuple<T1, T2, T3, T4, T5>, C> Seq<T1, T2, T3, T4, T5>(
            IParser<I, T1, C> p1,
            IParser<I, T2, C> p2,
            IParser<I, T3, C> p3,
            IParser<I, T4, C> p4,
            IParser<I, T5, C> p5)
        {
            return p1.Join(p2).Join(p3).Join(p4).Join(p5).Convert(p => new Tuple<T1, T2, T3, T4, T5>
            {
                Value1 = p.First.First.First.First,
                Value2 = p.First.First.First.Second,
                Value3 = p.First.First.Second,
                Value4 = p.First.Second,
                Value5 = p.Second
            });
        }
    }

    public abstract class LexerParserBase<I, O, C> : ParserBase<Lexer<I>.Token, C>
            where I : IComparable
    {
        private Lexer<I> lexer = null;
        private IParser<Lexer<I>.Token, O, C> parser = null;

        protected abstract void Initialize(out Lexer<I> lexer, out IParser<Lexer<I>.Token, O, C> parser);

        protected virtual C CreateContext()
        {
            return default(C);
        }

        protected virtual bool TokenFilter(Lexer<I>.Token token)
        {
            return true;
        }

        protected IParser<Lexer<I>.Token, Lexer<I>.Token, C> tk(I value, string name = "")
        {
            return new TokenParser<I, C>(value, name);
        }

        protected RuleParser<Lexer<I>.Token, int, object> CreateRule()
        {
            return new RuleParser<Lexer<I>.Token, int, object>();
        }

        public O Parse(string input)
        {
            if (lexer == null)
            {
                Initialize(out lexer, out parser);
            }
            ICloneableEnumerator<Lexer<I>.Token> tokens = lexer.Parse(input).Where(TokenFilter).GetCloneableEnumerable().CreateCloneableEnumerator();
            ParserResult<O, C> result = parser.Parse(ref tokens, CreateContext());
            return result.Result;
        }
    }

    public static class ParserExtensions
    {
        public static IParser<I, Pair<A, B>, C> Join<I, A, B, C>(this IParser<I, A, C> a, IParser<I, B, C> b)
        {
            return new SeqParser<I, A, B, C>(a, b);
        }

        public static IParser<I, A, C> Left<I, A, B, C>(this IParser<I, A, C> a, IParser<I, B, C> b)
        {
            return a.Join(b).Convert(p => p.First);
        }

        public static IParser<I, B, C> Right<I, A, B, C>(this IParser<I, A, C> a, IParser<I, B, C> b)
        {
            return a.Join(b).Convert(p => p.Second);
        }

        public static IParser<I, O, C> Alt<I, O, C>(this IParser<I, O, C> a, IParser<I, O, C> b)
        {
            return new AltParser<I, O, C>(a, b);
        }

        public static IParser<I, IEnumerable<O>, C> Opt<I, O, C>(this IParser<I, O, C> a)
        {
            return a.Convert(o => (IEnumerable<O>)new O[] { o }).Alt(new UnitParser<I, IEnumerable<O>, C>(new O[0]));
        }

        public static IParser<I, IEnumerable<O>, C> Loop<I, O, C>(this IParser<I, O, C> a)
        {
            return new LoopParser<I, O, C>(a);
        }

        public static IParser<I, B, C> Convert<I, A, B, C>(this IParser<I, A, C> a, Func<A, B> converter)
        {
            return new ConverterParser<I, A, B, C>(a, converter);
        }

        public static IParser<I, O, C> OnError<I, O, C>(this IParser<I, O, C> a, Func<ParserException<I>, ParserResult<O, C>> converter)
        {
            return new ErrorHandlerParser<I, O, C>(a, converter);
        }
    }
}
