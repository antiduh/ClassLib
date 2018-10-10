using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Antiduh.ClassLib.Test.BitListTests
{
    [TestClass]
    public class MathEx_RoundUpMultiple
    {
        [TestMethod]
        public void MathEx_RoundUpMultiple_Valid()
        {
            // Maps input value to output values.
            var testVectors = new Dictionary<int, int>()
            {
                {0, 0}, {1, 5}, {4, 5}, {5, 5}, {6, 10}, {10, 10}
            };

            foreach( var input in testVectors.Values )
            {
                int result = MathEx.RoundUpToMultiple( input, 5 );
                int expected = testVectors[result];

                Assert.AreEqual( expected, result );
            }
        }
    }
}