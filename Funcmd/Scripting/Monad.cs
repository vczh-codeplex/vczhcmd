using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    abstract class Monad
    {
        public abstract RuntimeValueWrapper Execute(DoExpression e, RuntimeContext context);
    }

    class PureMonad : Monad
    {
        public override RuntimeValueWrapper Execute(DoExpression e, RuntimeContext context)
        {
            RuntimeContext newContext = new RuntimeContext()
            {
                PreviousContext = context
            };
            foreach (Expression expression in e.Expressions)
            {
                expression.BuildContext(newContext);
            }
            if (e.Expressions.Count > 0)
            {
                return new RuntimeValueWrapper(new RuntimeUnevaluatedValue(e.Expressions.Last()), newContext);
            }
            else
            {
                return new RuntimeValueWrapper(new RuntimeEvaluatedValue(new object()), context);
            }
        }
    }

    class OrderedMonad : Monad
    {
        public override RuntimeValueWrapper Execute(DoExpression e, RuntimeContext context)
        {
            RuntimeValueWrapper result = null;
            RuntimeContext newContext = new RuntimeContext()
            {
                PreviousContext = context
            };
            foreach (Expression expression in e.Expressions)
            {
                VarExpression var = expression as VarExpression;
                if (var == null)
                {
                    expression.BuildContext(newContext);
                    result = expression.Execute(newContext);
                }
                else
                {
                    result = new RuntimeValueWrapper(new RuntimeUnevaluatedValue(var.Expression), context);
                    if (!var.Pattern.Match(context, result))
                    {
                        throw new Exception("模式匹配不成功。");
                    }
                }
            }
            if (result == null)
            {
                return new RuntimeValueWrapper(new RuntimeEvaluatedValue(new object()), context);
            }
            else
            {
                return result;
            }
        }
    }
}
