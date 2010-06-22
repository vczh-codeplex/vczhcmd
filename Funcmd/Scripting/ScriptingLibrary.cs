using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

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
            e.DefineValue("create_state", new ScriptingValue(RuntimeValueWrapper.CreateFunction(StateMonad.ReturnStateMonadValue, 2)));

            e.DefineValue("(+)", ScriptingValue.CreateFunction(PrimitiveAdd, 2));
            e.DefineValue("(-)", ScriptingValue.CreateFunction(PrimitiveSub, 2));
            e.DefineValue("(*)", ScriptingValue.CreateFunction(PrimitiveMul, 2));
            e.DefineValue("(/)", ScriptingValue.CreateFunction(PrimitiveDiv, 2));
            e.DefineValue("(%)", ScriptingValue.CreateFunction(PrimitiveMod, 2));
            e.DefineValue("(++)", ScriptingValue.CreateFunction(PrimitiveConcat, 2));
        }

        private static ScriptingValue State(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateValue(new StateMonad(arguments[0].ValueWrapper));
        }

        private static ScriptingValue Continue(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateValue(true);
        }

        private static ScriptingValue PrimitiveAdd(ScriptingValue[] arguments)
        {
            IConvertible a = (IConvertible)arguments[0].Value;
            IConvertible b = (IConvertible)arguments[1].Value;
            if (a is int && b is int)
            {
                return ScriptingValue.CreateValue(a.ToInt32(CultureInfo.InvariantCulture) + b.ToInt32(CultureInfo.InvariantCulture));
            }
            else
            {
                return ScriptingValue.CreateValue(a.ToDouble(CultureInfo.InvariantCulture) + b.ToDouble(CultureInfo.InvariantCulture));
            }
        }

        private static ScriptingValue PrimitiveSub(ScriptingValue[] arguments)
        {
            IConvertible a = (IConvertible)arguments[0].Value;
            IConvertible b = (IConvertible)arguments[1].Value;
            if (a is int && b is int)
            {
                return ScriptingValue.CreateValue(a.ToInt32(CultureInfo.InvariantCulture) - b.ToInt32(CultureInfo.InvariantCulture));
            }
            else
            {
                return ScriptingValue.CreateValue(a.ToDouble(CultureInfo.InvariantCulture) - b.ToDouble(CultureInfo.InvariantCulture));
            }
        }

        private static ScriptingValue PrimitiveMul(ScriptingValue[] arguments)
        {
            IConvertible a = (IConvertible)arguments[0].Value;
            IConvertible b = (IConvertible)arguments[1].Value;
            if (a is int && b is int)
            {
                return ScriptingValue.CreateValue(a.ToInt32(CultureInfo.InvariantCulture) * b.ToInt32(CultureInfo.InvariantCulture));
            }
            else
            {
                return ScriptingValue.CreateValue(a.ToDouble(CultureInfo.InvariantCulture) * b.ToDouble(CultureInfo.InvariantCulture));
            }
        }

        private static ScriptingValue PrimitiveDiv(ScriptingValue[] arguments)
        {
            IConvertible a = (IConvertible)arguments[0].Value;
            IConvertible b = (IConvertible)arguments[1].Value;
            if (a is int && b is int)
            {
                return ScriptingValue.CreateValue(a.ToInt32(CultureInfo.InvariantCulture) / b.ToInt32(CultureInfo.InvariantCulture));
            }
            else
            {
                return ScriptingValue.CreateValue(a.ToDouble(CultureInfo.InvariantCulture) / b.ToDouble(CultureInfo.InvariantCulture));
            }
        }

        private static ScriptingValue PrimitiveMod(ScriptingValue[] arguments)
        {
            IConvertible a = (IConvertible)arguments[0].Value;
            IConvertible b = (IConvertible)arguments[1].Value;
            if (a is int && b is int)
            {
                return ScriptingValue.CreateValue(a.ToInt32(CultureInfo.InvariantCulture) % b.ToInt32(CultureInfo.InvariantCulture));
            }
            else
            {
                return ScriptingValue.CreateValue(Math.IEEERemainder(a.ToDouble(CultureInfo.InvariantCulture), b.ToDouble(CultureInfo.InvariantCulture)));
            }
        }

        private static ScriptingValue PrimitiveConcat(ScriptingValue[] arguments)
        {
            object a = arguments[0].Value;
            object b = arguments[1].Value;
            if (a is string && b is string)
            {
                return ScriptingValue.CreateValue(((IConvertible)a).ToString(CultureInfo.InvariantCulture) + ((IConvertible)b).ToString(CultureInfo.InvariantCulture));
            }
            else
            {
                return ScriptingValue.CreateArray(arguments[0].Concat(arguments[1]).ToArray());
            }
        }
    }
}
