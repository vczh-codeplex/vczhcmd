using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Funcmd.Parser;

namespace Parser.Test
{
    [TestClass]
    public class ListEnumeratorTest
    {
        [TestMethod]
        public void CompareList()
        {
            List<int> numbers = Enumerable.Range(0, 10).ToList();
            Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(numbers.GetCloneableEnumerable()));
            Assert.IsTrue(Enumerable.Range(0, 10).SequenceEqual(Enumerable.Range(0, 10).GetCloneableEnumerable()));
        }
    }
}
