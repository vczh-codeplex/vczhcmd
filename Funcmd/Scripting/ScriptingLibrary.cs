using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;

namespace Funcmd.Scripting
{
    static class ScriptingLibrary
    {
        enum CompareResult
        {
            LessThan,
            EqualTo,
            GreaterThan,
            NotComparable
        }

        class ScriptingValueComparer : IEqualityComparer<ScriptingValue>
        {
            public bool Equals(ScriptingValue x, ScriptingValue y)
            {
                return ScriptingLibrary.Compare(x, y) == CompareResult.EqualTo;
            }

            public int GetHashCode(ScriptingValue obj)
            {
                return obj.GetHashCode();
            }
        }

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
            e.DefineValue("(<)", ScriptingValue.CreateFunction(PrimitiveLt, 2));
            e.DefineValue("(<=)", ScriptingValue.CreateFunction(PrimitiveLe, 2));
            e.DefineValue("(>)", ScriptingValue.CreateFunction(PrimitiveGt, 2));
            e.DefineValue("(>=)", ScriptingValue.CreateFunction(PrimitiveGe, 2));
            e.DefineValue("(==)", ScriptingValue.CreateFunction(PrimitiveEq, 2));
            e.DefineValue("(!=)", ScriptingValue.CreateFunction(PrimitiveNe, 2));

            foreach (MethodInfo method in typeof(Math).GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                if (method.ReturnType == typeof(double) && method.GetParameters().Length > 0 && method.GetParameters().All(p => p.ParameterType == typeof(double)))
                {
                    string methodName = method.Name.ToLower();
                    if (!e.IsDefined(methodName))
                    {
                        e.DefineValue(methodName, ScriptingValue.CreateFunction(MakeDoubleFunction(method), method.GetParameters().Length));
                    }
                }
            }

            e.DefineValue("pi", ScriptingValue.CreateValue(Math.PI));
            e.DefineValue("e", ScriptingValue.CreateValue(Math.E));
        }

        private static CompareResult ConvertCompareResult(int i)
        {
            if (i < 0) return CompareResult.LessThan;
            else if (i > 0) return CompareResult.GreaterThan;
            else return CompareResult.EqualTo;
        }

        private static CompareResult Compare(ScriptingValue a, ScriptingValue b)
        {
            if (a.IsInvokable || b.IsInvokable)
            {
                return CompareResult.NotComparable;
            }
            else if (a.IsArray && b.IsArray)
            {
                return a.SequenceEqual(b, new ScriptingValueComparer()) ? CompareResult.EqualTo : CompareResult.NotComparable;
            }
            else
            {
                try
                {
                    object obja = a.Value;
                    object objb = b.Value;
                    if (obja is int)
                    {
                        if (objb is int)
                        {
                            return ConvertCompareResult(((int)obja).CompareTo((int)objb));
                        }
                        else if (objb is double)
                        {
                            return ConvertCompareResult(((double)(int)obja).CompareTo((double)objb));
                        }
                    }
                    else if (obja is double)
                    {
                        if (objb is int)
                        {
                            return ConvertCompareResult(((double)obja).CompareTo((double)(int)objb));
                        }
                        else if (objb is double)
                        {
                            return ConvertCompareResult(((double)obja).CompareTo((double)objb));
                        }
                    }
                    else if (obja is string)
                    {
                        if (objb is string)
                        {
                            return ConvertCompareResult(((string)obja).CompareTo((string)objb));
                        }
                    }
                    else if (obja is bool)
                    {
                        if (objb is bool)
                        {
                            return ConvertCompareResult(((bool)obja).CompareTo((bool)objb));
                        }
                    }
                    return ConvertCompareResult(((IComparable)obja).CompareTo(objb));
                }
                catch (Exception)
                {
                    return CompareResult.NotComparable;
                }
            }
        }

        #region Predefined Functions

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

        private static ScriptingValue PrimitiveLt(ScriptingValue[] arguments)
        {
            CompareResult result = Compare(arguments[0], arguments[1]);
            return ScriptingValue.CreateValue(result == CompareResult.LessThan);
        }

        private static ScriptingValue PrimitiveLe(ScriptingValue[] arguments)
        {
            CompareResult result = Compare(arguments[0], arguments[1]);
            return ScriptingValue.CreateValue(result == CompareResult.LessThan || result == CompareResult.EqualTo);
        }

        private static ScriptingValue PrimitiveGt(ScriptingValue[] arguments)
        {
            CompareResult result = Compare(arguments[0], arguments[1]);
            return ScriptingValue.CreateValue(result == CompareResult.GreaterThan);
        }

        private static ScriptingValue PrimitiveGe(ScriptingValue[] arguments)
        {
            CompareResult result = Compare(arguments[0], arguments[1]);
            return ScriptingValue.CreateValue(result == CompareResult.GreaterThan || result == CompareResult.EqualTo);
        }

        private static ScriptingValue PrimitiveEq(ScriptingValue[] arguments)
        {
            CompareResult result = Compare(arguments[0], arguments[1]);
            return ScriptingValue.CreateValue(result == CompareResult.EqualTo);
        }

        private static ScriptingValue PrimitiveNe(ScriptingValue[] arguments)
        {
            CompareResult result = Compare(arguments[0], arguments[1]);
            return ScriptingValue.CreateValue(result != CompareResult.EqualTo);
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

        private static ScriptingValue DoubleFunction(MethodInfo method, ScriptingValue[] argument)
        {
            return ScriptingValue.CreateValue((double)method.Invoke(null, argument.Select(v => (object)((IConvertible)v.Value).ToDouble(CultureInfo.InvariantCulture)).ToArray()));
        }

        private static Func<ScriptingValue[], ScriptingValue> MakeDoubleFunction(MethodInfo method)
        {
            return (a) => DoubleFunction(method, a);
        }

        #endregion
    }
}
