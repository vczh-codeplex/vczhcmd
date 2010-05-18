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

        protected IParser<I, Pair<T1, T2>, C> Seq<T1, T2>(
            IParser<I, T1, C> p1,
            IParser<I, T2, C> p2)
        {
            return p1.Join(p2);
        }

        protected IParser<I, Pair<Pair<T1, T2>, T3>, C> Seq<T1, T2, T3>(
            IParser<I, T1, C> p1,
            IParser<I, T2, C> p2,
            IParser<I, T3, C> p3)
        {
            return p1.Join(p2).Join(p3);
        }

        protected IParser<I, Pair<Pair<Pair<T1, T2>, T3>, T4>, C> Seq<T1, T2, T3, T4>(
            IParser<I, T1, C> p1,
            IParser<I, T2, C> p2,
            IParser<I, T3, C> p3,
            IParser<I, T4, C> p4)
        {
            return p1.Join(p2).Join(p3).Join(p4);
        }

        protected IParser<I, Pair<Pair<Pair<Pair<T1, T2>, T3>, T4>, T5>, C> Seq<T1, T2, T3, T4, T5>(
            IParser<I, T1, C> p1,
            IParser<I, T2, C> p2,
            IParser<I, T3, C> p3,
            IParser<I, T4, C> p4,
            IParser<I, T5, C> p5)
        {
            return p1.Join(p2).Join(p3).Join(p4).Join(p5);
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
