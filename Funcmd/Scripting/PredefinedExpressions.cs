using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    class PrimitiveExpression : Expression
    {
        public object Value { get; set; }

        public override bool Match(RuntimeContext context, RuntimeValueWrapper valueWrapper)
        {
            if (!valueWrapper.IsInvokable)
            {
                return Value.Equals(valueWrapper.RuntimeObject);
            }
            return base.Match(context, valueWrapper);
        }
    }

    class IdentifierExpression : Expression
    {
        public string Name { get; set; }

        public override bool Match(RuntimeContext context, RuntimeValueWrapper valueWrapper)
        {
            if (context.Values.ContainsKey(Name))
            {
                throw new Exception(string.Format("{0}不可重复定义。", Name));
            }
            else
            {
                context.Values.Add(Name, valueWrapper);
                return true;
            }
        }
    }

    class FlagExpression : Expression
    {
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            return obj is FlagExpression && Name == (obj as FlagExpression).Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Match(RuntimeContext context, RuntimeValueWrapper valueWrapper)
        {
            if (context.Values.ContainsKey(Name))
            {
                throw new Exception(string.Format("{0}不可重复定义。", Name));
            }
            else
            {
                context.Values.Add(Name, new RuntimeValueWrapper(new RuntimeEvaluatedValue(this), context));
                return true;
            }
        }
    }

    class ArrayExpression : Expression
    {
        public List<Expression> Elements { get; set; }

        public override bool Match(RuntimeContext context, RuntimeValueWrapper valueWrapper)
        {
            if (!valueWrapper.IsInvokable)
            {
                RuntimeValueWrapper[] valueWrappers = valueWrapper.RuntimeObject as RuntimeValueWrapper[];
                if (valueWrappers.Length == Elements.Count)
                {
                    for (int i = 0; i < Elements.Count; i++)
                    {
                        if (!Elements[i].Match(context, valueWrappers[i]))
                        {
                            return false;
                        }
                    }
                    return true;
                }
            }
            return base.Match(context, valueWrapper);
        }
    }

    class ListExpression : Expression
    {
        public List<Expression> Elements { get; set; }

        public override bool Match(RuntimeContext context, RuntimeValueWrapper valueWrapper)
        {
            if (!valueWrapper.IsInvokable)
            {
                RuntimeValueWrapper[] valueWrappers = valueWrapper.RuntimeObject as RuntimeValueWrapper[];
                if (valueWrappers.Length >= Elements.Count - 1)
                {
                    for (int i = 0; i < Elements.Count - 1; i++)
                    {
                        if (!Elements[i].Match(context, valueWrappers[i]))
                        {
                            return false;
                        }
                    }
                    return Elements[Elements.Count - 1].Match(
                        context,
                        new RuntimeValueWrapper(
                            new RuntimeEvaluatedValue(
                                valueWrappers.Skip(Elements.Count - 2).ToArray()),
                                context
                            )
                        );
                }
            }
            return base.Match(context, valueWrapper);
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
