using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    public class ScriptingValue
    {
        private RuntimeValueWrapper ValueWrapper { get; set; }

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
    }

    public class Scripting
    {
        private static ScriptingParser parser = new ScriptingParser();

        public Dictionary<string, ScriptingValue> Parse(string code)
        {
            return parser.Parse(code).BuildContext().Values.ToDictionary(p => p.Key, p => new ScriptingValue(p.Value));
        }
    }
}
