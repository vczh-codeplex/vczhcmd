using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.Parser;

namespace Funcmd.Scripting
{
    public abstract class Element
    {
        public Lexer<TokenType>.Token TokenPosition { get; set; }
    }

    public abstract class Expression : Element
    {
    }

    public class Program
    {
        public List<Expression> Definitions { get; set; }
    }

    public class PrimitiveExpression : Expression
    {
        public object Value { get; set; }
    }

    public class IdentifierExpression : Expression
    {
        public string Name { get; set; }
    }

    public class FlagExpression : Expression
    {
        public string Name { get; set; }
    }

    public class ArrayExpression : Expression
    {
        public List<Expression> Elements { get; set; }
    }

    public class ListExpression : Expression
    {
        public List<Expression> Elements { get; set; }
    }

    public class InvokeExpression : Expression
    {
        public Expression Function { get; set; }
        public Expression Argument { get; set; }
    }

    public class CaseExpression : Expression
    {
        public class CasePair : Element
        {
            public Expression Pattern { get; set; }
            public Expression Expression { get; set; }
        }

        public Expression Source { get; set; }
        public List<CasePair> Pairs { get; set; }
    }

    public class DoExpression : Expression
    {
        public Expression MonadProvider { get; set; }
        public List<Expression> Expressions { get; set; }
    }

    public class LambdaExpression : Expression
    {
        public List<string> Parameters { get; set; }
        public Expression Expression { get; set; }
    }

    public class DefinitionExpression : Expression
    {
        public string Name { get; set; }
        public List<Expression> Patterns { get; set; }
        public Expression Expression { get; set; }
    }
}
