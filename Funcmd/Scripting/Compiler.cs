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
     * case exp of
     *   pattern1->v1;
     *   pattern2->v2;
     * end
     * do(MONAD_PROVIDER)//default = pure expression monad
     *   let name patterns = expression;
     *   expression;
     * end
     * \x,y=>x+y
     * [DECLARATION]
     * let name patterns = expression;
     */

    public enum TokenType
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
        Infer,
        Equal,
        Semicolon,
        Lambda,
        Blank,
        Keyword,
    }

    public class ScriptingParser : LexerParserBase<TokenType, Program>
    {
        protected override bool TokenFilter(Lexer<TokenType>.Token token)
        {
            return token.Tag != TokenType.Blank;
        }

        protected override void Initialize(out Lexer<TokenType> lexer, out IParser<Lexer<TokenType>.Token, Program, object> parser)
        {
            lexer = new Lexer<TokenType>();
            lexer.AddToken(@"case|of|end|let|do|var", TokenType.Keyword);
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
            lexer.AddToken(@"=>", TokenType.Infer);
            lexer.AddToken(@"=", TokenType.Equal);
            lexer.AddToken(@";", TokenType.Semicolon);
            lexer.AddToken(@"\\", TokenType.Lambda);
            lexer.AddToken(@"\s+", TokenType.Blank);

            var expression = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var simple = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var primitive = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var program = new RuleParser<Lexer<TokenType>.Token, Program, object>();

            var INTEGER = tk(TokenType.Integer, "表达式").Convert(t => (Expression)new PrimitiveExpression()
            {
                TokenPosition = t,
                Value = int.Parse(t.Value)
            });
            var FLOAT = tk(TokenType.Float, "表达式").Convert(t => (Expression)new PrimitiveExpression()
            {
                TokenPosition = t,
                Value = double.Parse(t.Value)
            });
            var STRING = tk(TokenType.String, "表达式").Convert(t => (Expression)new PrimitiveExpression()
            {
                TokenPosition = t,
                Value = Escape(t.Value.Substring(1, t.Value.Length - 2))
            });
            var BOOLEAN = Alt(tk("true"), tk("false")).Convert(t => (Expression)new PrimitiveExpression()
            {
                TokenPosition = t,
                Value = t.Value == "true"
            });

            var id = tk(TokenType.Identifier, "表达式").Convert(t => (Expression)new IdentifierExpression()
            {
                TokenPosition = t,
                Name = t.Value
            });
            var flag = tk(TokenType.Flag, "表达式").Convert(t => (Expression)new FlagExpression()
            {
                TokenPosition = t,
                Name = t.Value
            });

            var array = Seq(
                tk("["),
                Seq(expression, tk(",").Right(expression).Loop()).Opt(),
                tk("]")
                )
                .Convert(p =>
                {
                    if (p.Value2.Count() == 0)
                    {
                        return (Expression)new ArrayExpression()
                        {
                            TokenPosition = p.Value1,
                            Elements = new List<Expression>()
                        };
                    }
                    else
                    {
                        return (Expression)new ArrayExpression()
                        {
                            TokenPosition = p.Value1,
                            Elements = new Expression[] { p.Value2.First().Value1 }.Concat(p.Value2.First().Value2).ToList()
                        };
                    }
                });

            var list = tk("(").Right(Seq(expression, tk(":").Right(expression).Loop())).Left(tk(")")).Convert(p =>
            {
                if (p.Value2.Count() == 0)
                {
                    return p.Value1;
                }
                else
                {
                    return (Expression)new ListExpression()
                    {
                        TokenPosition = p.Value1.TokenPosition,
                        Elements = new Expression[] { p.Value1 }.Concat(p.Value2).ToList()
                    };
                }
            });

            simple.Imply(Alt(
                INTEGER, FLOAT, STRING, BOOLEAN, id, flag, array, list));

            var match = Seq(
                tk("case").Right(expression).Left(tk("of")),
                Seq(expression, tk("=>").Right(expression)).Left(tk(";")).Loop().Left(tk("end"))
                )
                .Convert(p =>
                {
                    return (Expression)new CaseExpression()
                    {
                        TokenPosition = p.Value1.TokenPosition,
                        Pairs = p.Value2.Select(pair => new CaseExpression.CasePair()
                        {
                            Pattern = pair.Value1,
                            Expression = pair.Value2
                        }).ToList()
                    };
                });

            var monad = Seq(
                Seq(tk("do"), tk("(").Right(expression).Left(tk(")")).Opt()),
                expression.Left(tk(";")).Loop().Left(tk("end"))
                )
                .Convert(p =>
                {
                    return (Expression)new DoExpression()
                    {
                        TokenPosition = p.Value1.Value1,
                        MonadProvider = p.Value1.Value2.Count() == 0 ? null : p.Value1.Value2.First(),
                        Expressions = p.Value2.ToList()
                    };
                });

            var monadvar = Seq(tk("var"), expression, tk("="), expression).Convert(p =>
                {
                    return (Expression)new VarExpression()
                    {
                        TokenPosition = p.Value1,
                        Pattern = p.Value2,
                        Expression = p.Value4
                    };
                });

            var lambda = Seq(
                tk("\\").Right(Seq(id, tk(",").Right(id).Loop())),
                tk("=>").Right(expression)
                )
                .Convert(p =>
                {
                    return (Expression)new LambdaExpression()
                    {
                        TokenPosition = p.Value1.Value1.TokenPosition,
                        Parameters = new string[] { (p.Value1.Value1 as IdentifierExpression).Name }
                            .Concat(p.Value1.Value2.Select(v => (v as IdentifierExpression).Name))
                            .ToList(),
                        Expression = p.Value2
                    };
                });

            var def = Seq(
                tk("let").Right(id),
                simple.Loop(),
                tk("=").Right(expression)
                )
                .Convert(p =>
                {
                    return (Expression)new DefinitionExpression()
                    {
                        TokenPosition = p.Value1.TokenPosition,
                        Name = (p.Value1 as IdentifierExpression).Name,
                        Patterns = p.Value2.ToList(),
                        Expression = p.Value3
                    };
                });

            primitive.Imply(Alt(
                match, monad, monadvar, lambda, def, simple));

            expression.Imply(Seq(primitive, primitive.Loop()).Convert(p =>
            {
                var t = p.Value1;
                foreach (var exp in p.Value2)
                {
                    t = new InvokeExpression()
                    {
                        TokenPosition = t.TokenPosition,
                        Function = t,
                        Argument = exp
                    };
                }
                return t;
            }));

            program.Imply(expression.Left(tk(";")).LoopToEnd().Convert(es => new Program() { Definitions = es.ToList() }));

            parser = program;
        }

        private string Escape(string s)
        {
            return s;
        }
    }
}
