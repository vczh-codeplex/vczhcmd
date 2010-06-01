using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Funcmd.Scripting
{
    public class Flag
    {
        public string Name { get; private set; }

        public Flag(string name)
        {
            this.Name = name;
        }

        public override bool Equals(object obj)
        {
            return obj is FlagExpression && Name == (obj as FlagExpression).Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override string ToString()
        {
            return Name;
        }
    }

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
        public RuntimeContext Context { get; private set; }

        private void EnsureValueExecuted()
        {
            if (!Value.IsReady)
            {
                RuntimeValueWrapper valueWrapper = Value.Execute(Context);
                Value = valueWrapper.Value;
                Context = valueWrapper.Context;
            }
        }

        public RuntimeValueWrapper(RuntimeValue value, RuntimeContext context)
        {
            this.Value = value;
            this.Context = context;
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

        public RuntimeValueWrapper Invoke(RuntimeValueWrapper argument)
        {
            EnsureValueExecuted();
            return Value.Invoke(Context, argument);
        }

        public static RuntimeValueWrapper CreateValue(object o)
        {
            if (o.GetType() == typeof(RuntimeValueWrapper))
            {
                return (RuntimeValueWrapper)o;
            }
            else if (o.GetType().IsArray)
            {
                object[] a = ((IEnumerable)o).Cast<object>().ToArray();
                return new RuntimeValueWrapper(new RuntimeEvaluatedValue(a.Select(i => CreateValue(i)).ToArray()), new RuntimeContext());
            }
            else
            {
                return new RuntimeValueWrapper(new RuntimeEvaluatedValue(o), new RuntimeContext());
            }
        }

        public static RuntimeValueWrapper CreateFunction(Func<RuntimeValueWrapper[], RuntimeValueWrapper> externalFunction, int parameterCount)
        {
            return new RuntimeValueWrapper(new RuntimeExternalValue()
                {
                    ExternalFunction = externalFunction,
                    Arguments = new RuntimeValueWrapper[] { },
                    ParameterCount = parameterCount
                }, new RuntimeContext());
        }

        public static RuntimeValueWrapper CreateArray(params object[] o)
        {
            return CreateValue(o);
        }
    }

    internal abstract class RuntimeValue
    {
        public abstract bool IsReady { get; }
        public abstract bool IsInvokable { get; }
        public abstract RuntimeValueWrapper Execute(RuntimeContext context);
        public abstract RuntimeValueWrapper Invoke(RuntimeContext context, RuntimeValueWrapper argument);
        public abstract object RuntimeObject { get; }
    }
}
