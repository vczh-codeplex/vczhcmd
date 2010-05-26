using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Funcmd.Parser;

namespace Funcmd.Scripting
{
    abstract class Element
    {
        public Lexer<TokenType>.Token TokenPosition { get; set; }
    }

    abstract class Expression : Element
    {
    }

    class Program
    {
        public List<Expression> Definitions { get; private set; }

        public Program()
        {
            Definitions = new List<Expression>();
        }
    }

    class PrimitiveExpression : Expression
    {
        public object Value { get; set; }
    }

    class IdentifierExpression : Expression
    {
        public string Name { get; set; }
    }

    class FlagExpression : Expression
    {
        public string Name { get; set; }
    }

    class ArrayExpression : Expression
    {
        public List<Expression> Elements { get; private set; }

        public ArrayExpression()
        {
            Elements = new List<Expression>();
        }
    }

    class ListExpression : Expression
    {
        public List<Expression> Elements { get; private set; }

        public ListExpression()
        {
            Elements = new List<Expression>();
        }
    }

    class InvokeExpression : Expression
    {
        public Expression Function { get; set; }
        public Expression Argument { get; set; }
    }

    class CaseExpression : Expression
    {
        public class CasePair : Element
        {
            public Expression Pattern { get; set; }
            public Expression Expression { get; set; }
        }

        public Expression Source { get; set; }
        public List<CasePair> Pairs { get; private set; }

        public CaseExpression()
        {
            Pairs = new List<CasePair>();
        }
    }

    class DoExpression : Expression
    {
        public Expression MonadProvider { get; set; }
        public List<Expression> Expressions { get; private set; }

        public DoExpression()
        {
            Expressions = new List<Expression>();
        }
    }

    class LambdaExpression : Expression
    {
        public List<string> Parameters { get; private set; }
        public Expression Expression { get; set; }

        public LambdaExpression()
        {
            Parameters = new List<string>();
        }
    }

    class DefinitionExpression : Expression
    {
        public string Name { get; set; }
        public List<Expression> Patterns { get; private set; }
        public Expression Expression { get; set; }

        public DefinitionExpression()
        {
            Patterns = new List<Expression>();
        }
    }
}
