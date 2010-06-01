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

        private ScriptingEnvironment Parse(string code)
        {
            ScriptingEnvironment environment = new Scripting().Parse(code);
            environment.DefineValue("add", ScriptingValue.CreateFunction(Add, 2));
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
                "let sum ['tree, m, n] = add (sum m) (sum n);\r\n"
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
                "let main = agg (\\a,b=>add a b) 0 [1,2,3,4,5];\r\n"
                );
            Assert.AreEqual(15, context["main"].Value);
        }

        [TestMethod]
        public void ParseCase()
        {
            Parse(
                "let sum (x:xs) = case x of\r\n" +
                "  'leaf => first xs;\r\n" +
                "  'tree => case xs of\r\n" +
                "    [a,b]=>add a b;\r\n" +
                "  end;\r\n" +
                "end;\r\n"
                );
        }

        [TestMethod]
        public void ParseMonad1()
        {
            Parse(
                "let main = do\r\n" +
                "  var a = readline;\r\n" +
                "  var b = readline;\r\n" +
                "  var c = return (add a b);\r\n" +
                "  writeline c;\r\n" +
                "end;\r\n"
                );
        }

        [TestMethod]
        public void ParseMonad2()
        {
            Parse(
                "let main = do(io)\r\n" +
                "  var a = readline;\r\n" +
                "  var b = readline;\r\n" +
                "  var c = return (add a b);\r\n" +
                "  writeline c;\r\n" +
                "end;\r\n"
                );
        }
    }
}
