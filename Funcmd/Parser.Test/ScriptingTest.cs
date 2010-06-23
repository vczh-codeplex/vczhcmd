using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Funcmd.Scripting;

namespace Parser.Test
{
    [TestClass]
    public class ScriptingTest
    {
        private ScriptingValue Add(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateValue((int)arguments[0].Value + (int)arguments[1].Value);
        }

        private ScriptingValue Sub(ScriptingValue[] arguments)
        {
            return ScriptingValue.CreateValue((int)arguments[0].Value - (int)arguments[1].Value);
        }

        private ScriptingEnvironment Parse(string code)
        {
            ScriptingEnvironment environment = new Scripting().Parse(code);
            environment.DefineValue("add", ScriptingValue.CreateFunction(Add, 2));
            environment.DefineValue("sub", ScriptingValue.CreateFunction(Sub, 2));
            return environment;
        }

        [TestMethod]
        public void ParseSumArray()
        {
            var context = Parse(
                "let sum [] = 0;\r\n" +
                "let sum (x:xs) = add x (sum xs);\r\n"
                );
            var sum = context["sum"];
            var array = ScriptingValue.CreateArray(1, 2, 3, 4, 5);
            var result = sum.Invoke(array);
            Assert.AreEqual(15, (int)result.Value);
        }

        [TestMethod]
        public void ParseSumTree()
        {
            var context = Parse(
                "let sum ['leaf, n] = n;\r\n" +
                "let sum ['tree, m, n] = add (m :> sum) (sum n);\r\n"
                );
            var sum = context["sum"];
            var tree = ScriptingValue.CreateArray(
                    new Flag("tree"),
                    ScriptingValue.CreateArray(
                        new Flag("tree"),
                        ScriptingValue.CreateArray(
                            new Flag("leaf"),
                            1
                        ),
                        ScriptingValue.CreateArray(
                            new Flag("leaf"),
                            2
                        )
                    ),
                    ScriptingValue.CreateArray(
                        new Flag("tree"),
                        ScriptingValue.CreateArray(
                            new Flag("leaf"),
                            3
                        ),
                        ScriptingValue.CreateArray(
                            new Flag("leaf"),
                            4
                        )
                    )
                );
            var result = sum.Invoke(tree);
            Assert.AreEqual(10, (int)result.Value);
        }

        [TestMethod]
        public void ParseCountArray()
        {
            var context = Parse(
                "let count [] = 0;\r\n" +
                "let count (x:xs) = add 1 (count xs);\r\n" +
                "let main = count [1,2.2,\"3\",true,false];\r\n"
                );
            var count = context["count"];
            var array = ScriptingValue.CreateArray(1, 2, 3, 4, 5);
            var result = count.Invoke(array);
            Assert.AreEqual(5, (int)result.Value);
            Assert.AreEqual(5, context["main"].Value);
        }

        [TestMethod]
        public void ParseAggregate()
        {
            var context = Parse(
                "let agg f i [] = i;\r\n" +
                "let agg f i (x:xs) = agg f (f i x) xs;\r\n" +
                "let main = agg (\\a,b=>add a b) 0 (1:2:3:4:5:[]);\r\n"
                );
            Assert.AreEqual(15, context["main"].Value);
        }

        [TestMethod]
        public void ParseCase()
        {
            var context = Parse(
                "let first (x:xs) = x;\r\n" +
                "let sum (x:xs) = case x of\r\n" +
                "  'leaf => first xs;\r\n" +
                "  'tree => case xs of\r\n" +
                "    [a,b]=>add (sum a) (sum b);\r\n" +
                "  end;\r\n" +
                "end;\r\n" +
                "let main = sum ['tree, ['tree, ['leaf, 1], ['leaf, 2]], ['tree, ['leaf, 3], ['leaf, 4]]];"
                );
            Assert.AreEqual(10, (int)context["main"].Value);
        }

        [TestMethod]
        public void ParseFab()
        {
            var context = Parse(
                "let fab 0 = 1;\r\n" +
                "let fab 1 = 1;\r\n" +
                "let fab n = add (fab (sub n 2)) (fab (sub n 1));\r\n" +
                "let main = fab 9;\r\n"
                );
            Assert.AreEqual(55, context["main"].Value);
        }

        [TestMethod]
        public void ParseDefaultMonad()
        {
            var context = Parse(
                "let main = do\r\n" +
                "  var a = 1;\r\n" +
                "  var b = 2;\r\n" +
                "  var c = add a b;\r\n" +
                "  c;\r\n" +
                "end;\r\n"
                );
            Assert.AreEqual(3, context["main"].Value);
        }

        [TestMethod]
        public void ParsePureMonad()
        {
            var context = Parse(
                "let main = do(pure)\r\n" +
                "  var a = 1;\r\n" +
                "  var b = 2;\r\n" +
                "  var c = add a b;\r\n" +
                "  c;\r\n" +
                "end;\r\n"
                );
            Assert.AreEqual(3, context["main"].Value);
        }

        [TestMethod]
        public void ParseOrderedMonad()
        {
            var context = Parse(
                "let main = do(ordered)\r\n" +
                "  var a = 1;\r\n" +
                "  var b = 2;\r\n" +
                "  var c = add a b;\r\n" +
                "  c;\r\n" +
                "end;\r\n"
                );
            Assert.AreEqual(3, context["main"].Value);
        }

        [TestMethod]
        public void ParseStateMonad()
        {
            var context = Parse(
                "let next s = create_state s (add s 1);\r\n" +
                "let main = do(state(continue))\r\n" +
                "  var a = next;\r\n" +
                "  var b = next;\r\n" +
                "  var c = return (add a b);\r\n" +
                "  return c;\r\n" +
                "end;\r\n"
                );
            Assert.AreEqual(3, context["main"].RunStateMonad(ScriptingValue.CreateValue(1)).Value);
        }

        [TestMethod]
        public void ParseOperator()
        {
            var context = Parse(
                "let (+) a b = add a b;\r\n" +
                "let (*) a b = sub a b;\r\n" +
                "let main = 1+2*3;\r\n"
                );
            Assert.AreEqual(0, context["main"].Value);
        }

        [TestMethod]
        public void LibraryAggregate()
        {
            var context = Parse(
                "let main = ([1,2,3] :> aggregate 0 (+)) == 6;\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryDistinct()
        {
            var context = Parse(
                "let main = ([1,2,3,3,2,1] :> distinct) == [1,2,3];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryExcept()
        {
            var context = Parse(
                "let main = ([1,2,3,4,5,6] :> except [3,4]) == [1,2,5,6];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryFirst()
        {
            var context = Parse(
                "let main = ([1,2,3,4,5,6] :> first 0) == 1;\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryIntersect()
        {
            var context = Parse(
                "let main = ([1,2,3,4] :> intersect [3,4,5,6]) == [3,4];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryLast()
        {
            var context = Parse(
                "let main = ([1,2,3,4,5,6] :> last 0) == 6;\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryOrderBy()
        {
            var context = Parse(
                "let main = ([2,1,3] :> order_by unit) == [1,2,3];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryReverse()
        {
            var context = Parse(
                "let main = ([1,2,3] :> reverse) == [3,2,1];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibrarySelect()
        {
            var context = Parse(
                "let main = ([1,2,3] :> select (\\a=>a+1)) == [2,3,4];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibrarySelectMany()
        {
            var context = Parse(
                "let main = ([1,2,3] :> select_many (\\a=>[a,a+1])) == [1,2,2,3,3,4];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibrarySkip()
        {
            var context = Parse(
                "let main = ([1,2,3,4,5,6] :> skip 3) == [4,5,6];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibrarySkipWhile()
        {
            var context = Parse(
                "let main = ([1,2,3,4,5,6] :> skip_while (\\a=>a<4)) == [4,5,6];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryTake()
        {
            var context = Parse(
                "let main = ([1,2,3,4,5,6] :> take 3) == [1,2,3];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryTakeWhile()
        {
            var context = Parse(
                "let main = ([1,2,3,4,5,6] :> take_while (\\a=>a<4)) == [1,2,3];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryUnion()
        {
            var context = Parse(
                "let main = ([1,2,3,4] :> union [3,4,5,6]) == [1,2,3,4,5,6];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryWhere()
        {
            var context = Parse(
                "let main = ([1,2,3,4,5,6] :> where (\\a=>a%2==0)) == [2,4,6];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryZip()
        {
            var context = Parse(
                "let main = ([1,2,3] :> zip [4,5,6]) == [[1,4],[2,5],[3,6]];\r\n"
                );
            Assert.IsTrue((bool)context["main"].Value);
        }

        [TestMethod]
        public void LibraryString()
        {
            string[] lines = new string[]
            {
                @"to_lower ""VCZH is GENIUS!"" == ""vczh is genius!""",                                      // main0
                @"to_upper ""VCZH is GENIUS!"" == ""VCZH IS GENIUS!""",                                      // main1
                @"find ""vczh"" ""hello vczh!"" == 6",                                                       // main2
                @"find ""vczh"" ""hello world!"" == neg 1",                                                  // main3
                @"find_all ""vczh"" ""vczh is genius! hello vczh"" == [0,22]",                               // main4
                @"find_all ""vczh"" ""hello world!"" == []",                                                 // main5
                @"reg_find ""vczh"" ""hello vczh!"" == [6,""vczh""]",                                        // main6
                @"reg_find ""vczh"" ""hello world!"" == [neg 1,""""]",                                       // main7
                @"reg_find_all ""vczh"" ""vczh is genius! hello vczh"" == [[0,""vczh""],[22,""vczh""]]",     // main8
                @"reg_find_all ""vczh"" ""hello world!"" == []",                                             // main9
                @"length ""vczh"" == 4",                                                                     // main10
                @"item 2 ""vczh"" == ""z""",                                                                 // main11
                @"empty ""vczh"" == false",                                                                  // main12
                @"empty """" == true",                                                                       // main12
                @"length [1,2,3,4] == 4",                                                                    // main13
                @"item 2 [1,2,3,4] == 3",                                                                    // main14
                @"empty [1,2,3,4] == false",                                                                 // main15
                @"empty [] == true",                                                                         // main16
                @"split "","" ""12,34,56"" == [""12"",""34"",""56""]",                                       // main17
            };
            var context = Parse(lines
                .Select((a, i) => "let main" + i.ToString() + " = " + a + ";\r\n")
                .Aggregate((a, b) => a + b)
                );
            for (int i = 0; i < lines.Length; i++)
            {
                Assert.IsTrue((bool)context["main" + i.ToString()].Value, "main" + i.ToString() + " failed!");
            }
        }

        [TestMethod]
        public void LibraryConversion()
        {
            string[] lines = new string[]
            {
                @"to_int 10 == 10",                      // main0
                @"to_int 10.0 == 10",                    // main1
                @"to_int ""10"" == 10",                  // main2
                @"to_double 10 == 10.0",                 // main3
                @"to_double 10.0 == 10.0",               // main4
                @"to_double ""10"" == 10.0",             // main5
                @"to_string 10 == ""10""",               // main6
                @"to_string 10.0 == ""10""",             // main7
                @"to_string ""10"" == ""10""",           // main8
            };
            var context = Parse(lines
                .Select((a, i) => "let main" + i.ToString() + " = " + a + ";\r\n")
                .Aggregate((a, b) => a + b)
                );
            for (int i = 0; i < lines.Length; i++)
            {
                Assert.IsTrue((bool)context["main" + i.ToString()].Value, "main" + i.ToString() + " failed!");
            }
        }
    }
}
