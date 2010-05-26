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
        private Program Parse(string code)
        {
            return new ScriptingParser().Parse(code);
        }

        [TestMethod]
        public void ParseSum()
        {
            Parse(
                "let sum [] = [];\r\n" +
                "let sum (x:xs) = add x (sum xs);\r\n"
                );
        }
    }
}
