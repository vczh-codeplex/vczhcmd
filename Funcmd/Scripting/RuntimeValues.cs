using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    class RuntimeEvaluatedValue : RuntimeValue
    {
        private object runtimeObject;

        public RuntimeEvaluatedValue(object runtimeObject)
        {
            this.runtimeObject = runtimeObject;
        }

        public override bool IsReady
        {
            get
            {
                return true;
            }
        }

        public override bool IsInvokable
        {
            get
            {
                return false;
            }
        }

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return new RuntimeValueWrapper(this, context);
        }

        public override RuntimeValueWrapper Invoke(RuntimeContext context, RuntimeValueWrapper argument)
        {
            throw new NotSupportedException();
        }

        public override object RuntimeObject
        {
            get
            {
                return runtimeObject;
            }
        }
    }

    class RuntimeInvokableValue : RuntimeValue
    {
        public class IncompletedExpression
        {
            public List<Expression> Patterns { get; set; }
            public RuntimeContext Context { get; set; }
            public Expression Expression { get; set; }
        }

        public List<IncompletedExpression> IncompletedExpressions { get; private set; }

        public RuntimeInvokableValue()
        {
            this.IncompletedExpressions = new List<IncompletedExpression>();
        }

        public override bool IsReady
        {
            get
            {
                return true;
            }
        }

        public override bool IsInvokable
        {
            get
            {
                return true;
            }
        }

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return new RuntimeValueWrapper(this, context);
        }

        public override RuntimeValueWrapper Invoke(RuntimeContext context, RuntimeValueWrapper argument)
        {
            List<IncompletedExpression> newIncompletedExpressions = new List<IncompletedExpression>();
            foreach (IncompletedExpression incompletedExpression in IncompletedExpressions)
            {
                RuntimeContext newContext = new RuntimeContext();
                newContext.PreviousContext = incompletedExpression.Context.PreviousContext;
                foreach (var pair in incompletedExpression.Context.Values)
                {
                    newContext.Values.Add(pair.Key, pair.Value);
                }
                if (incompletedExpression.Patterns.First().Match(newContext, argument))
                {
                    if (incompletedExpression.Patterns.Count == 1)
                    {
                        return incompletedExpression.Expression.Execute(newContext);
                    }
                    else
                    {
                        IncompletedExpression newIncompletedExpression = new IncompletedExpression();
                        newIncompletedExpression.Context = newContext;
                        newIncompletedExpression.Expression = incompletedExpression.Expression;
                        newIncompletedExpression.Patterns = incompletedExpression.Patterns.Skip(1).ToList();
                        newIncompletedExpressions.Add(newIncompletedExpression);
                    }
                }
            }
            if (newIncompletedExpressions.Count == 0)
            {
                throw new Exception("模式匹配不成功。");
            }
            else
            {
                RuntimeInvokableValue value = new RuntimeInvokableValue();
                value.IncompletedExpressions.AddRange(newIncompletedExpressions);
                return new RuntimeValueWrapper(value, context);
            }
        }

        public override object RuntimeObject
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }

    class RuntimeUnevaluatedValue : RuntimeValue
    {
        public Expression UnevaluatedValue { get; private set; }

        public RuntimeUnevaluatedValue(Expression unevaluatedValue)
        {
            this.UnevaluatedValue = unevaluatedValue;
        }

        public override bool IsReady
        {
            get
            {
                return false;
            }
        }

        public override bool IsInvokable
        {
            get
            {
                return false;
            }
        }

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return UnevaluatedValue.Execute(context);
        }

        public override RuntimeValueWrapper Invoke(RuntimeContext context, RuntimeValueWrapper argument)
        {
            throw new NotSupportedException();
        }

        public override object RuntimeObject
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }

    class RuntimeExternalValue : RuntimeValue
    {
        public Func<RuntimeValueWrapper[], RuntimeValueWrapper> ExternalFunction { get; set; }
        public RuntimeValueWrapper[] Arguments { get; set; }
        public int ParameterCount { get; set; }

        public override bool IsReady
        {
            get
            {
                return true;
            }
        }

        public override bool IsInvokable
        {
            get
            {
                return ParameterCount > 0;
            }
        }

        public override RuntimeValueWrapper Execute(RuntimeContext context)
        {
            return new RuntimeValueWrapper(this, context);
        }

        public override RuntimeValueWrapper Invoke(RuntimeContext context, RuntimeValueWrapper argument)
        {
            if (ParameterCount > 0)
            {
                RuntimeValueWrapper[] newArguments = Arguments.Concat(new RuntimeValueWrapper[] { argument }).ToArray();
                if (ParameterCount == 1)
                {
                    return ExternalFunction(newArguments);
                }
                else
                {
                    return new RuntimeValueWrapper(new RuntimeExternalValue()
                        {
                            ExternalFunction = ExternalFunction,
                            Arguments = newArguments,
                            ParameterCount = ParameterCount - 1
                        }, context);
                }
            }
            else
            {
                throw new Exception("外界函数参数数量不能为0。");
            }
        }

        public override object RuntimeObject
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
