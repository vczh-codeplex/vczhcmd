using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    static class ScriptingLibrary
    {
        public static void LoadLibrary(ScriptingEnvironment e)
        {
            e.DefineValue("pure", ScriptingValue.CreateValue(new PureMonad()));
            e.DefineValue("ordered", ScriptingValue.CreateValue(new OrderedMonad()));
            e.DefineValue("state", ScriptingValue.CreateFunction(State, 1));
            e.DefineValue("continue", ScriptingValue.CreateFunction(Continue, 1));
            e.DefineValue("create_state", new ScriptingValue(RuntimeValueWrapper.CreateFunction(StateMonad.ReturnStateMonadValue,2)));
        }

        private static ScriptingValue State(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateValue(new StateMonad(arguments[0].ValueWrapper));
        }

        private static ScriptingValue Continue(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateValue(true);
        }
    }
}
