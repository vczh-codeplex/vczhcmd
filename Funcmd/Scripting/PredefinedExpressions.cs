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

        public override void BuildContext(RuntimeContext context)
        {
            throw new NotSupportedException();
        }
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

        public override void BuildContext(RuntimeContext context)
        {
            RuntimeInvokableValue invokableValue = new RuntimeInvokableValue();
            if (context.Values.ContainsKey(Name))
            {
                RuntimeValueWrapper valueWrapper = context.Values[Name];
                invokableValue = valueWrapper.Value as RuntimeInvokableValue;
                if (invokableValue == null || this.Patterns.Count == 0)
                {
                    throw new Exception(string.Format("{0}不可重复定义。", Name));
                }
            }
            if (this.Patterns.Count == 0)
            {
                context.Values.Add(Name, new RuntimeValueWrapper(new RuntimeUnevaluatedValue(Expression), context));
            }
            else
            {
                if (invokableValue == null)
                {
                    invokableValue = new RuntimeInvokableValue();
                    context.Values.Add(Name, new RuntimeValueWrapper(invokableValue, context));
                }

                RuntimeInvokableValue.IncompletedExpression incompletedExpression = new RuntimeInvokableValue.IncompletedExpression();
                incompletedExpression.context = new RuntimeContext();
                incompletedExpression.Expression = Expression;
                incompletedExpression.Patterns = new List<Expression>(Patterns);
                invokableValue.IncompletedExpressions.Add(incompletedExpression);
            }
        }
    }
}
