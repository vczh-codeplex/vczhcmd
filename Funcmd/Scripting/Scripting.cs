using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    public class ScriptingValue : IEnumerable<ScriptingValue>
    {
        internal RuntimeValueWrapper ValueWrapper { get; set; }

        internal ScriptingValue(RuntimeValueWrapper valueWrapper)
        {
            this.ValueWrapper = valueWrapper;
        }

        public override string ToString()
        {
            return ValueWrapper.ToString();
        }

        #region Primitive

        public bool IsInvokable
        {
            get
            {
                return ValueWrapper.IsInvokable;
            }
        }

        public bool IsArray
        {
            get
            {
                return !ValueWrapper.IsInvokable && ValueWrapper.RuntimeObject is RuntimeValueWrapper[];
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

        public ScriptingValue RunStateMonad(ScriptingValue state)
        {
            return new ScriptingValue(StateMonad.RunStateMonad(ValueWrapper, state.ValueWrapper).result);
        }

        #endregion

        #region Array

        public int Length
        {
            get
            {
                return (ValueWrapper.RuntimeObject as RuntimeValueWrapper[]).Length;
            }
        }

        public ScriptingValue this[int index]
        {
            get
            {
                return new ScriptingValue((ValueWrapper.RuntimeObject as RuntimeValueWrapper[])[index]);
            }
        }

        IEnumerator<ScriptingValue> IEnumerable<ScriptingValue>.GetEnumerator()
        {
            return (ValueWrapper.RuntimeObject as RuntimeValueWrapper[]).Select(w => new ScriptingValue(w)).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return (this as IEnumerable<ScriptingValue>).GetEnumerator();
        }

        #endregion

        #region Builder

        public static ScriptingValue CreateValue(object o)
        {
            return new ScriptingValue(RuntimeValueWrapper.CreateValue(o));
        }

        public static ScriptingValue CreateArray(params object[] o)
        {
            return new ScriptingValue(RuntimeValueWrapper.CreateArray(o));
        }

        public static ScriptingValue CreateFunction(Func<ScriptingValue[], ScriptingValue> externalFunction, int parameterCount)
        {
            return new ScriptingValue(RuntimeValueWrapper.CreateFunction(
                (xs) => externalFunction(xs.Select(w => new ScriptingValue(w)).ToArray()).ValueWrapper
                , parameterCount));
        }

        #endregion
    }

    public static class ScriptingValueExtension
    {
        public static IEnumerable<T> ScriptingCast<T>(this IEnumerable<ScriptingValue> values)
        {
            return values.Select(s => s.Value).Cast<T>();
        }
    }

    public class Scripting
    {
        internal static ScriptingParser parser = new ScriptingParser();

        public ScriptingEnvironment Parse(string code)
        {
            return new ScriptingEnvironment(code == null ? new RuntimeContext() : parser.Parse(code).BuildContext());
        }
    }

    public class ScriptingEnvironment
    {
        private RuntimeContext context = new RuntimeContext();

        internal ScriptingEnvironment(RuntimeContext context)
        {
            this.context = context;
            this.context.PreviousContext = new RuntimeContext();
            ScriptingLibrary.LoadLibrary(this);
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

        public bool IsDefined(string name)
        {
            return context.PreviousContext.Values.ContainsKey(name);
        }

        public ScriptingValue ParseValue(string expression)
        {
            string id = "vczh_do_not_want_you_to_use_this_identifier_because_if_you_try_to_do_this_you_will_be_bring_into_a_dead_loop_do_you_understand_question_mark";
            RuntimeContext tempContext = Scripting.parser.Parse("let " + id + " = " + expression + ";").BuildContext();
            tempContext.PreviousContext = context;
            return new ScriptingValue(tempContext.Values[id]);
        }
    }
}
