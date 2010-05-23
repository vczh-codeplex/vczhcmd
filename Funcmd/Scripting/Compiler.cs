using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.Parser;

namespace Funcmd.Scripting
{
    /* [EXPRESSION]
     * 123
     * "vczh"
     * true
     * false
     * 'id
     * id
     * [v1,v2,v3]
     * v1:v2:v3
     * func p1 p2
     * case exp
     *   pattern1->v1
     *   pattern2->v2
     * do
     *   name patterns = expression
     *   expression
     * (x,y)=>(x+y)
     * [DECLARATION]
     * name patterns = expression
     */

    enum TokenType
    {
        Integer,
        Float,
        String,
        Flag,
        Identifier,
        OpenSquare,
        CloseSquare,
        OpenBracket,
        CloseBracket,
        Colon,
        Comma,
        Case,
        Lambda,
        Equal,
    }

    class Compiler : LexerParserBase<TokenType, Program>
    {
        protected override void Initialize(out Lexer<TokenType> lexer, out IParser<Lexer<TokenType>.Token, Program, object> parser)
        {
            lexer = new Lexer<TokenType>();
            lexer.AddToken(@"\d+\.\d+", TokenType.Float);
            lexer.AddToken(@"\d+", TokenType.Integer);
            lexer.AddToken(@"""([^""]|\.)*""", TokenType.String);
            lexer.AddToken(@"'[a-zA-Z_]\w*", TokenType.Flag);
            lexer.AddToken(@"[a-zA-Z_]\w*", TokenType.Identifier);
            lexer.AddToken(@"\(", TokenType.OpenBracket);
            lexer.AddToken(@"\)", TokenType.CloseBracket);
            lexer.AddToken(@"\[", TokenType.OpenSquare);
            lexer.AddToken(@"\]", TokenType.CloseSquare);
            lexer.AddToken(@":", TokenType.Colon);
            lexer.AddToken(@",", TokenType.Comma);
            lexer.AddToken(@"->", TokenType.Case);
            lexer.AddToken(@"=>", TokenType.Lambda);
            lexer.AddToken(@"=", TokenType.Equal);

            parser = null;
        }
    }
}
