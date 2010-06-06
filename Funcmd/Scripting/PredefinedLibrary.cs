using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Funcmd.Scripting
{
    static class PredefinedLibrary
    {
        public static void LoadLibrary(ScriptingEnvironment e)
        {
            e.DefineValue("pure", ScriptingValue.CreateValue(new PureMonad()));
            e.DefineValue("ordered", ScriptingValue.CreateValue(new OrderedMonad()));
        }
    }
}
