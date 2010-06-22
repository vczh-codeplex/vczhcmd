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

        class ScriptingValueComparer : IEqualityComparer<ScriptingValue>, IComparer<ScriptingValue>
        {
            public bool Equals(ScriptingValue x, ScriptingValue y)
            {
                return ScriptingLibrary.Compare(x, y) == CompareResult.EqualTo;
            }

            public int GetHashCode(ScriptingValue obj)
            {
                if (obj.IsArray)
                {
                    return obj.Select(v => GetHashCode(v)).Sum();
                }
                else if (obj.IsInvokable)
                {
                    return obj.GetHashCode();
                }
                else
                {
                    return obj.Value.GetHashCode();
                }
            }

            public int Compare(ScriptingValue x, ScriptingValue y)
            {
                switch (ScriptingLibrary.Compare(x, y))
                {
                    case CompareResult.EqualTo: return 0;
                    case CompareResult.GreaterThan: return 1;
                    case CompareResult.LessThan: return -1;
                    default: throw new InvalidOperationException(string.Format("{0}和{1}无法进行比较。", x, y));
                }
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
            e.DefineValue("(&&)", ScriptingValue.CreateFunction(PrimitiveAnd, 2));
            e.DefineValue("(||)", ScriptingValue.CreateFunction(PrimitiveOr, 2));
            e.DefineValue("(^)", ScriptingValue.CreateFunction(PrimitiveXor, 2));
            e.DefineValue("not", ScriptingValue.CreateFunction(PrimitiveNot, 1));
            e.DefineValue("neg", ScriptingValue.CreateFunction(PrimitiveNeg, 1));
            e.DefineValue("unit", ScriptingValue.CreateFunction(PrimitiveUnit, 1));

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

            e.DefineValue("aggregate", ScriptingValue.CreateFunction(Aggregate, 3));
            e.DefineValue("distinct", ScriptingValue.CreateFunction(Distinct, 1));
            e.DefineValue("except", ScriptingValue.CreateFunction(Except, 2));
            e.DefineValue("first", ScriptingValue.CreateFunction(First, 2));
            e.DefineValue("intersect", ScriptingValue.CreateFunction(Intersect, 2));
            e.DefineValue("last", ScriptingValue.CreateFunction(Last, 2));
            e.DefineValue("order_by", ScriptingValue.CreateFunction(OrderBy, 2));
            e.DefineValue("reverse", ScriptingValue.CreateFunction(Reverse, 1));
            e.DefineValue("select", ScriptingValue.CreateFunction(Select, 2));
            e.DefineValue("select_many", ScriptingValue.CreateFunction(SelectMany, 2));
            e.DefineValue("skip", ScriptingValue.CreateFunction(Skip, 2));
            e.DefineValue("skip_while", ScriptingValue.CreateFunction(SkipWhile, 2));
            e.DefineValue("take", ScriptingValue.CreateFunction(Take, 2));
            e.DefineValue("take_while", ScriptingValue.CreateFunction(TakeWhile, 2));
            e.DefineValue("union", ScriptingValue.CreateFunction(Union, 2));
            e.DefineValue("where", ScriptingValue.CreateFunction(Where, 2));
            e.DefineValue("zip", ScriptingValue.CreateFunction(Zip, 2));

            /*
             * to_lower s->s
             * to_upper s->s
             * find target->source->i
             * find_all target->source->[i]
             * split s->[s]->[s]
             * trim s->s
             * sub s->i->i->s
             * length s->i
             * length [x]->i
             * reg_find pattern->source->[i,s]
             * reg_find_all pattern->source->[[i,s]]
             * stocs s->[s]
             * 
             * min x->x->x
             * max x->x->x
             */
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

        private static ScriptingValue PrimitiveAnd(ScriptingValue[] arguments)
        {
            bool a = (bool)arguments[0].Value;
            bool b = (bool)arguments[1].Value;
            return ScriptingValue.CreateValue(a && b);
        }

        private static ScriptingValue PrimitiveOr(ScriptingValue[] arguments)
        {
            bool a = (bool)arguments[0].Value;
            bool b = (bool)arguments[1].Value;
            return ScriptingValue.CreateValue(a || b);
        }

        private static ScriptingValue PrimitiveXor(ScriptingValue[] arguments)
        {
            bool a = (bool)arguments[0].Value;
            bool b = (bool)arguments[1].Value;
            return ScriptingValue.CreateValue(a ^ b);
        }

        private static ScriptingValue PrimitiveNot(ScriptingValue[] arguments)
        {
            bool a = (bool)arguments[0].Value;
            return ScriptingValue.CreateValue(!a);
        }

        private static ScriptingValue PrimitiveNeg(ScriptingValue[] arguments)
        {
            IConvertible a = (IConvertible)arguments[0].Value;
            if (a is int)
            {
                return ScriptingValue.CreateValue(-a.ToInt32(CultureInfo.InvariantCulture));
            }
            else
            {
                return ScriptingValue.CreateValue(-a.ToDouble(CultureInfo.InvariantCulture));
            }
        }

        private static ScriptingValue PrimitiveUnit(ScriptingValue[] arguments)
        {
            return arguments[0];
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

        #region Linq

        // aggregate default function list
        private static ScriptingValue Aggregate(ScriptingValue[] arguments)
        {
            return arguments[2].Aggregate(arguments[0], (a, b) => arguments[1].Invoke(a, b));
        }

        // distinct list
        private static ScriptingValue Distinct(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[0].Distinct(new ScriptingValueComparer()).ToArray());
        }

        // except remover_list source_list
        private static ScriptingValue Except(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].Except(arguments[0], new ScriptingValueComparer()).ToArray());
        }

        // first default list
        private static ScriptingValue First(ScriptingValue[] arguments)
        {
            return arguments[1].FirstOrDefault() ?? arguments[0];
        }

        // intersect list2 list1
        private static ScriptingValue Intersect(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].Intersect(arguments[0], new ScriptingValueComparer()).ToArray());
        }

        // last default list
        private static ScriptingValue Last(ScriptingValue[] arguments)
        {
            return arguments[1].LastOrDefault() ?? arguments[0];
        }

        // order_by converter list
        private static ScriptingValue OrderBy(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].OrderBy(s => arguments[0].Invoke(s), new ScriptingValueComparer()).ToArray());
        }

        // reverse list
        private static ScriptingValue Reverse(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[0].Reverse().ToArray());
        }

        // select converter list
        private static ScriptingValue Select(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].Select(v => arguments[0].Invoke(v)).ToArray());
        }

        // select_many converter list
        private static ScriptingValue SelectMany(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].SelectMany(v => arguments[0].Invoke(v)).ToArray());
        }

        // skip number list
        private static ScriptingValue Skip(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].Skip((int)arguments[0].Value).ToArray());
        }

        // skip_while predicate list
        private static ScriptingValue SkipWhile(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].SkipWhile(v => (bool)arguments[0].Invoke(v).Value).ToArray());
        }

        // take number list
        private static ScriptingValue Take(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].Take((int)arguments[0].Value).ToArray());
        }

        // take_while predicate list
        private static ScriptingValue TakeWhile(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].TakeWhile(v => (bool)arguments[0].Invoke(v).Value).ToArray());
        }

        // union list2 list1
        private static ScriptingValue Union(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].Union(arguments[0], new ScriptingValueComparer()).ToArray());
        }

        // where predicate list
        private static ScriptingValue Where(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].Where(v => (bool)arguments[0].Invoke(v).Value).ToArray());
        }

        // zip list2 list1
        private static ScriptingValue Zip(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateArray(arguments[1].Zip(arguments[0], (a, b) => ScriptingValue.CreateArray(a, b)).ToArray());
        }

        #endregion
    }
}
