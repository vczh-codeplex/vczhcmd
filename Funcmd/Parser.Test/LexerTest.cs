using Funcmd.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Parser.Test
{
    [TestClass()]
    public class LexerTest
    {
        private enum TokenType
        {
            Number,
            Add,
            Mul,
            Open,
            Close
        }

        private Lexer<TokenType> lexer = null;

        [TestInitialize]
        public void Initialize()
        {
            lexer = new Lexer<TokenType>();
            lexer.AddToken(@"\d+", TokenType.Number);
            lexer.AddToken(@"\+|-", TokenType.Add);
            lexer.AddToken(@"\*|/", TokenType.Mul);
            lexer.AddToken(@"\(", TokenType.Open);
            lexer.AddToken(@"\)", TokenType.Close);
        }

        [TestMethod]
        public void TestSuccessfulInput()
        {
            Assert.IsTrue(
                new TokenType[] {
                    TokenType.Open, 
                    TokenType.Number,
                    TokenType.Add, 
                    TokenType.Number,
                    TokenType.Close, 
                    TokenType.Mul,
                    TokenType.Open,
                    TokenType.Number,
                    TokenType.Add,
                    TokenType.Number,
                    TokenType.Close
                }
                .SequenceEqual(lexer.Parse("(11+22)*(33+44)")
                    .Select(t => t.Tag)
                    )
                );
        }

        [TestMethod]
        [ExpectedException(typeof(LexerException<TokenType>))]
        public void TestFailInput()
        {
            lexer.Parse("(11+22) * (33+44)").ToArray();
        }
    }
}
