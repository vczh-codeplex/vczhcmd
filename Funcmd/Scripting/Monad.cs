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

    class StateMonad : Monad
    {
        public class StatePackage
        {
            public RuntimeValueWrapper state = null;
            public RuntimeValueWrapper result = null;
        }

        private RuntimeValueWrapper continueFunction;

        public StateMonad(RuntimeValueWrapper continueFunction)
        {
            this.continueFunction = continueFunction;
        }

        public override RuntimeValueWrapper Execute(DoExpression e, RuntimeContext context)
        {
            Func<RuntimeValueWrapper[], RuntimeValueWrapper> monadFunction = arguments =>
            {
                StatePackage result = new StatePackage()
                {
                    result = null,
                    state = arguments[0]
                };
                RuntimeContext newContext = new RuntimeContext()
                {
                    PreviousContext = context
                };
                newContext.Values.Add("return", new RuntimeValueWrapper(new RuntimeExternalValue()
                {
                    ExternalFunction = ReturnStateMonadValue,
                    ParameterCount = 2
                }, context));
                foreach (Expression expression in e.Expressions)
                {
                    VarExpression var = expression as VarExpression;
                    if (var == null)
                    {
                        expression.BuildContext(newContext);
                        result = RunStateMonad(new RuntimeValueWrapper(new RuntimeUnevaluatedValue(expression), context), result.state);
                    }
                    else
                    {
                        result = RunStateMonad(new RuntimeValueWrapper(new RuntimeUnevaluatedValue(var.Expression), context), result.state);
                        if (!var.Pattern.Match(context, result.result))
                        {
                            throw new Exception("模式匹配不成功。");
                        }
                    }
                    if (!(bool)continueFunction.Invoke(result.state).RuntimeObject)
                    {
                        break;
                    }
                }
                if (result.result == null)
                {
                    result.result = new RuntimeValueWrapper(new RuntimeEvaluatedValue(new object()), context);
                }
                return new RuntimeValueWrapper(new RuntimeEvaluatedValue(result), context);
            };
            return new RuntimeValueWrapper(new RuntimeExternalValue()
            {
                ExternalFunction = monadFunction,
                ParameterCount = 1
            }, context);
        }

        public static StatePackage RunStateMonad(RuntimeValueWrapper monad, RuntimeValueWrapper state)
        {
            return monad.Invoke(state).RuntimeObject as StatePackage;
        }

        public static RuntimeValueWrapper ReturnStateMonadValue(RuntimeValueWrapper[] arguments)
        {
            return new RuntimeValueWrapper(new RuntimeEvaluatedValue(new StatePackage()
            {
                result = arguments[0],
                state = arguments[1]
            }), new RuntimeContext());
        }
    }
}
