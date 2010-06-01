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
            public RuntimeContext context { get; set; }
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
            throw new NotImplementedException();
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
}
