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
        Close
    }

    [TestClass]
    public class ParserTest : LexerParserBase<TokenType, int, object>
    {
        [TestMethod]
        public void Calculator()
        {
        }

        protected override void Initialize(out Lexer<TokenType> lexer, out IParser<Lexer<TokenType>.Token, int, object> parser)
        {
            lexer = new Lexer<TokenType>();
            lexer.AddToken(@"\d+", TokenType.Number);
            lexer.AddToken(@"\+|-", TokenType.Add);
            lexer.AddToken(@"\*|/", TokenType.Mul);
            lexer.AddToken(@"\(", TokenType.Open);
            lexer.AddToken(@"\)", TokenType.Close);

            var factor = CreateRule();
            var term = CreateRule();
            var exp = CreateRule();

            var NUMBER = tk(TokenType.Number, "数字");
            var ADD = tk(TokenType.Add);
            var MUL = tk(TokenType.Mul);
            var OPEN = tk(TokenType.Open, "左括号");
            var CLOSE = tk(TokenType.Close, "右括号");

            parser = exp;
        }
    }
}
