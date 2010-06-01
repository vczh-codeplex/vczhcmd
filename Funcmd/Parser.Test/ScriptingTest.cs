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
        private object Add(object[] arguments)
        {
            return (int)arguments[0] + (int)arguments[1];
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
            Parse(
                "let sum ['leaf, n] = n;\r\n" +
                "let sum ['tree, m, n] = add (sum m) (sum n);\r\n"
                );
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
            Parse(
                "let agg f i [] = i;\r\n" +
                "let agg f i (x:xs) = agg f (f i x) xs;\r\n" +
                "let main = agg (\\a,b=>add a b) 0 [1,2,3,4,5];\r\n"
                );
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
