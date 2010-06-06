using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    class PrimitiveExpression : Expression
    {
        public object Value { get; set; }

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return new RuntimeValueWrapper(new RuntimeEvaluatedValue(Value), context);
        }

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

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            while (context != null)
            {
                if (context.Values.ContainsKey(Name))
                {
                    return context.Values[Name];
                }
                context = context.PreviousContext;
            }
            throw new Exception(string.Format("{0}没有定义。", Name));
        }

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

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return new RuntimeValueWrapper(new RuntimeEvaluatedValue(new Flag(Name)), context);
        }

        public override bool Match(RuntimeContext context, RuntimeValueWrapper valueWrapper)
        {
            if (context.Values.ContainsKey(Name))
            {
                throw new Exception(string.Format("{0}不可重复定义。", Name));
            }
            else if (!valueWrapper.IsInvokable)
            {
                return new Flag(Name).Equals(valueWrapper.RuntimeObject);
            }
            else
            {
                return false;
            }
        }
    }

    class ArrayExpression : Expression
    {
        public List<Expression> Elements { get; set; }

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return new RuntimeValueWrapper(new RuntimeEvaluatedValue(
                Elements.Select(e => new RuntimeValueWrapper(new RuntimeUnevaluatedValue(e), context)).ToArray()
                ), context);
        }

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

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return new RuntimeValueWrapper(new RuntimeEvaluatedValue(
                Elements
                    .Take(Elements.Count - 1)
                    .Select(e => new RuntimeValueWrapper(new RuntimeUnevaluatedValue(e), context))
                    .Union((RuntimeValueWrapper[])Elements.Last().Execute(context).RuntimeObject)
                    .ToArray()
                ), context);
        }

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
                                valueWrappers.Skip(Elements.Count - 1).ToArray()),
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

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return Function.Execute(context).Invoke(Argument.Execute(context));
        }
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

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            RuntimeValueWrapper valueToBeMatched = Source.Execute(context);
            foreach (CasePair pair in Pairs)
            {
                RuntimeContext newContext = new RuntimeContext()
                {
                    PreviousContext = context
                };
                if (pair.Pattern.Match(newContext, valueToBeMatched))
                {
                    return new RuntimeValueWrapper(new RuntimeUnevaluatedValue(pair.Expression), newContext);
                }
            }
            throw new Exception("模式匹配不成功。");
        }
    }

    class DoExpression : Expression
    {
        public Expression MonadProvider { get; set; }
        public List<Expression> Expressions { get; set; }

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            Monad monad = null;
            if (MonadProvider == null)
            {
                monad = new PureMonad();
            }
            else
            {
                RuntimeValueWrapper monadWrapper = MonadProvider.Execute(context);
                monad = (Monad)monadWrapper.RuntimeObject;
            }
            return monad.Execute(this, context);
        }
    }

    class VarExpression : Expression
    {
        public Expression Pattern { get; set; }
        public Expression Expression { get; set; }

        public override void BuildContext(RuntimeContext context)
        {
            if (!Pattern.Match(context, new RuntimeValueWrapper(new RuntimeUnevaluatedValue(Expression), context)))
            {
                throw new Exception("模式匹配不成功。");
            }
        }
    }

    class LambdaExpression : Expression
    {
        public List<string> Parameters { get; set; }
        public Expression Expression { get; set; }

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            RuntimeInvokableValue.IncompletedExpression incompletedExpression = new RuntimeInvokableValue.IncompletedExpression()
            {
                Context = new RuntimeContext()
                {
                    PreviousContext = context
                },
                Expression = Expression,
                Patterns = Parameters.Select(n => (Expression)new IdentifierExpression() { Name = n }).ToList(),
            };
            RuntimeInvokableValue value = new RuntimeInvokableValue();
            value.IncompletedExpressions.Add(incompletedExpression);
            return new RuntimeValueWrapper(value, context);
        }
    }

    class DefinitionExpression : Expression
    {
        public string Name { get; set; }
        public List<Expression> Patterns { get; set; }
        public Expression Expression { get; set; }

        public override void BuildContext(RuntimeContext context)
        {
            RuntimeInvokableValue invokableValue = null;
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

                RuntimeInvokableValue.IncompletedExpression incompletedExpression = new RuntimeInvokableValue.IncompletedExpression()
                {
                    Context = new RuntimeContext()
                    {
                        PreviousContext = context
                    },
                    Expression = Expression,
                    Patterns = new List<Expression>(Patterns),
                };
                invokableValue.IncompletedExpressions.Add(incompletedExpression);
            }
        }
    }
}
