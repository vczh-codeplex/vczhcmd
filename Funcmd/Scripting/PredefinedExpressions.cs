using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
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
        public List<Expression> Elements { get; set; }
    }

    class ListExpression : Expression
    {
        public List<Expression> Elements { get; set; }
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
        public List<CasePair> Pairs { get; set; }
    }

    class DoExpression : Expression
    {
        public Expression MonadProvider { get; set; }
        public List<Expression> Expressions { get; set; }
    }

    class VarExpression : Expression
    {
        public Expression Pattern { get; set; }
        public Expression Expression { get; set; }
    }

    class LambdaExpression : Expression
    {
        public List<string> Parameters { get; set; }
        public Expression Expression { get; set; }
    }

    class DefinitionExpression : Expression
    {
        public string Name { get; set; }
        public List<Expression> Patterns { get; set; }
        public Expression Expression { get; set; }
    }
}
