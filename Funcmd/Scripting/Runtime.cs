using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    internal class RuntimeContext
    {
        public RuntimeContext PreviousContext { get; set; }
        public Dictionary<string, RuntimeValueWrapper> Values { get; private set; }

        public RuntimeContext()
        {
            this.Values = new Dictionary<string, RuntimeValueWrapper>();
        }
    }

    internal class RuntimeValueWrapper
    {
        public RuntimeValue Value { get; private set; }
        public RuntimeContext context { get; private set; }

        private void EnsureValueExecuted()
        {
            if (!Value.IsReady)
            {
                Value = Value.Execute(context);
            }
        }

        public RuntimeValueWrapper(RuntimeValue value, RuntimeContext context)
        {
            this.Value = value;
            this.context = context;
        }

        public bool IsInvokable
        {
            get
            {
                EnsureValueExecuted();
                return Value.IsInvokable;
            }
        }

        public object RuntimeObject
        {
            get
            {
                EnsureValueExecuted();
                return Value.RuntimeObject;
            }
        }
    }

    internal abstract class RuntimeValue
    {
        public abstract bool IsReady { get; }
        public abstract bool IsInvokable { get; }
        public abstract RuntimeValue Execute(RuntimeContext context);
        public abstract RuntimeValue Invoke(RuntimeContext context, RuntimeValueWrapper argument);
        public abstract object RuntimeObject { get; }
    }
}
