using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    public class ScriptingValue
    {
        internal RuntimeValueWrapper ValueWrapper { get; set; }

        internal ScriptingValue(RuntimeValueWrapper valueWrapper)
        {
            this.ValueWrapper = valueWrapper;
        }

        public bool IsInvokable
        {
            get
            {
                return ValueWrapper.IsInvokable;
            }
        }

        public object Value
        {
            get
            {
                return ValueWrapper.RuntimeObject;
            }
        }

        public ScriptingValue Invoke(ScriptingValue firstArgument, params ScriptingValue[] arguments)
        {
            RuntimeValueWrapper result = ValueWrapper.Invoke(firstArgument.ValueWrapper);
            foreach (ScriptingValue argument in arguments)
            {
                result = result.Invoke(argument.ValueWrapper);
            }
            return new ScriptingValue(result);
        }

        public static ScriptingValue CreateValue(object o)
        {
            return new ScriptingValue(RuntimeValueWrapper.CreateValue(o));
        }

        public static ScriptingValue CreateArray(params object[] o)
        {
            return new ScriptingValue(RuntimeValueWrapper.CreateArray(o));
        }

        public static ScriptingValue CreateFunction(Func<object[], object> externalFunction, int parameterCount)
        {
            return new ScriptingValue(RuntimeValueWrapper.CreateFunction(externalFunction, parameterCount));
        }
    }

    public class Scripting
    {
        private static ScriptingParser parser = new ScriptingParser();

        public ScriptingEnvironment Parse(string code)
        {
            return new ScriptingEnvironment(parser.Parse(code).BuildContext());
        }
    }

    public class ScriptingEnvironment
    {
        private RuntimeContext context = new RuntimeContext();

        internal ScriptingEnvironment(RuntimeContext context)
        {
            this.context = context;
            this.context.PreviousContext = new RuntimeContext();
        }

        public ScriptingValue this[string index]
        {
            get
            {
                return new ScriptingValue(this.context.Values[index]);
            }
        }

        public void DefineValue(string name, ScriptingValue value)
        {
            context.PreviousContext.Values.Add(name, value.ValueWrapper);
        }
    }
}
