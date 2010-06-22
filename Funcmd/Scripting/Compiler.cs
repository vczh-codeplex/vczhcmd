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

    internal enum TokenType
    {
        Keyword,
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
        Operator,
        Invoke,
        Blank,
    }

    internal class ScriptingParser : LexerParserBase<TokenType, Program>
    {
        protected override bool TokenFilter(Lexer<TokenType>.Token token)
        {
            return token.Tag != TokenType.Blank;
        }

        protected override void Initialize(out Lexer<TokenType> lexer, out IParser<Lexer<TokenType>.Token, Program, object> parser)
        {
            string operatorRegex = @"([+\-*/\\<>=%:&|^!])+";

            lexer = new Lexer<TokenType>();
            lexer.AddToken(@"case|of|end|let|do|var", TokenType.Keyword);
            lexer.AddToken(@"\d+\.\d+", TokenType.Float);
            lexer.AddToken(@"\d+", TokenType.Integer);
            lexer.AddToken(@"""([^""]|\.)*""", TokenType.String);
            lexer.AddToken(@"'[a-zA-Z_]\w*", TokenType.Flag);
            lexer.AddToken(@"([a-zA-Z_]\w*)|(\(" + operatorRegex + @"\))", TokenType.Identifier);
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
            lexer.AddToken(@":>", TokenType.Invoke);
            lexer.AddToken(operatorRegex, TokenType.Operator);
            lexer.AddToken(@"\s+", TokenType.Blank);

            var simple = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var primitive = new RuleParser<Lexer<TokenType>.Token, Expression, object>();

            var termExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var mulExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var addExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var andExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var orExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var compExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var bxorExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var bandExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var borExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var opExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();
            var invokeExpr = new RuleParser<Lexer<TokenType>.Token, Expression, object>();

            var expression = invokeExpr;
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
                        Source = p.Value1,
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

            termExpr.Imply(Seq(primitive, primitive.Loop()).Convert(ToInvoke));

            mulExpr.Imply(Seq(termExpr, Seq(tks(new string[] { @"*", @"/", @"%" }), termExpr).Loop()).Convert(ToOperator));
            addExpr.Imply(Seq(mulExpr, Seq(tks(new string[] { @"+", @"-" }), mulExpr).Loop()).Convert(ToOperator));
            andExpr.Imply(Seq(addExpr, Seq(tk(@"&"), addExpr).Loop()).Convert(ToOperator));
            orExpr.Imply(Seq(andExpr, Seq(tk(@"|"), andExpr).Loop()).Convert(ToOperator));
            compExpr.Imply(Seq(orExpr, Seq(tks(new string[] { @"<", @">", @"<=", @">=", @"<>", @"==" }), orExpr).Loop()).Convert(ToOperator));
            bxorExpr.Imply(Seq(compExpr, Seq(tk(@"^"), compExpr).Loop()).Convert(ToOperator));
            bandExpr.Imply(Seq(bxorExpr, Seq(tk(@"&&"), bxorExpr).Loop()).Convert(ToOperator));
            borExpr.Imply(Seq(bandExpr, Seq(tk(@"||"), bandExpr).Loop()).Convert(ToOperator));
            opExpr.Imply(Seq(borExpr, Seq(tk(TokenType.Operator), borExpr).Loop()).Convert(ToOperator));
            invokeExpr.Imply(Seq(opExpr, tk(@":>").Right(opExpr).Loop()).Convert(ToInvokeReverse));

            program.Imply(expression.Left(tk(";")).LoopToEnd().Convert(es => new Program() { Definitions = es.ToList() }));

            parser = program;
        }

        private Expression ToOperator(Pair<Expression, IEnumerable<Pair<Lexer<TokenType>.Token, Expression>>> input)
        {
            Expression result = input.Value1;
            foreach (var op in input.Value2)
            {
                result = new InvokeExpression()
                {
                    TokenPosition = op.Value1,
                    Function = new InvokeExpression()
                    {
                        TokenPosition = op.Value1,
                        Function = new IdentifierExpression()
                        {
                            TokenPosition = op.Value1,
                            Name = "(" + op.Value1.Value + ")"
                        },
                        Argument = result
                    },
                    Argument = op.Value2
                };
            }
            return result;
        }

        private Expression ToInvoke(Pair<Expression, IEnumerable<Expression>> p)
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
        }

        private Expression ToInvokeReverse(Pair<Expression, IEnumerable<Expression>> p)
        {
            var t = p.Value1;
            foreach (var exp in p.Value2)
            {
                t = new InvokeExpression()
                {
                    TokenPosition = t.TokenPosition,
                    Function = exp,
                    Argument = t
                };
            }
            return t;
        }

        private string Escape(string s)
        {
            string result = "";
            bool escaping = false;
            foreach (char c in s)
            {
                if (escaping)
                {
                    switch (c)
                    {
                        case 'r':
                            result += '\r';
                            break;
                        case 'n':
                            result += '\n';
                            break;
                        case 't':
                            result += '\t';
                            break;
                        default:
                            result += c;
                            break;
                    }
                    escaping = false;
                }
                else
                {
                    switch (c)
                    {
                        case '\\':
                            escaping = true;
                            break;
                        default:
                            result += c;
                            break;
                    }
                }
            }
            return result;
        }
    }
}
