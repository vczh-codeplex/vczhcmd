using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Funcmd.Parser;

namespace Parser.Test
{
    public enum TokenType
    {
        Number,
        Add,
        Mul,
        Open,
        Close,
        Blank
    }

    [TestClass]
    public class ParserTest : LexerParserBase<TokenType, int>
    {
        [TestMethod]
        public void Calculator()
        {
            Assert.AreEqual(1, Parse("1"));
            Assert.AreEqual(3, Parse("1+2"));
            Assert.AreEqual(1, Parse("(1)"));
            Assert.AreEqual(3, Parse("(1+2)"));
            Assert.AreEqual(21, Parse("(1+2)*(3+4)"));
            Assert.AreEqual(1, Parse(" 1 "));
            Assert.AreEqual(3, Parse("1 + 2"));
            Assert.AreEqual(1, Parse("( 1 )"));
            Assert.AreEqual(3, Parse("( 1 + 2 )"));
            Assert.AreEqual(21, Parse("( 1 + 2 ) * ( 3 + 4 )"));
        }

        private int Op2(Pair<int, string, int> input)
        {
            switch (input.Value2)
            {
                case "+": return input.Value1 + input.Value3;
                case "-": return input.Value1 - input.Value3;
                case "*": return input.Value1 * input.Value3;
                case "/": return input.Value1 / input.Value3;
                default:
                    throw new ArgumentException();
            }
        }

        private int Op(Pair<int, IEnumerable<Pair<string, int>>> input)
        {
            return input.Value2.Aggregate(
                input.Value1,
                (a, b) => Op2(new Pair<int, string, int>()
                {
                    Value1 = a,
                    Value2 = b.Value1,
                    Value3 = b.Value2
                })
                );
        }

        protected override bool TokenFilter(Lexer<TokenType>.Token token)
        {
            return token.Tag != TokenType.Blank;
        }

        protected override void Initialize(out Lexer<TokenType> lexer, out IParser<Lexer<TokenType>.Token, int, object> parser)
        {
            lexer = new Lexer<TokenType>();
            lexer.AddToken(@"\d+", TokenType.Number);
            lexer.AddToken(@"\+|-", TokenType.Add);
            lexer.AddToken(@"\*|/", TokenType.Mul);
            lexer.AddToken(@"\(", TokenType.Open);
            lexer.AddToken(@"\)", TokenType.Close);
            lexer.AddToken(@"\s+", TokenType.Blank);

            var factor = CreateRule();
            var term = CreateRule();
            var exp = CreateRule();

            var NUMBER = tk(TokenType.Number, "数字").Convert(t => int.Parse(t.Value));
            var ADD = tk(TokenType.Add).Convert(t => t.Value);
            var MUL = tk(TokenType.Mul).Convert(t => t.Value);
            var OPEN = tk(TokenType.Open, "左括号");
            var CLOSE = tk(TokenType.Close, "右括号");

            factor.Imply(Alt(NUMBER, OPEN.Right(exp).Left(CLOSE)));
            term.Imply(Seq(factor, Seq(MUL, factor).Loop()).Convert(Op));
            exp.Imply(Seq(term, Seq(ADD, term).Loop()).Convert(Op));

            parser = exp;
        }
    }
}
