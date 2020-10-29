using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Antiduh.ClassLib.Test.Table
{
    [TestClass]
    public class TableTests
    {
        // Make sure works without indexes

        [TestMethod]
        public void Table_With_No_Indexes()
        {
            var table = new Table<int>();

            table.Add( 0 );

            Assert.AreEqual( 1, table.Count );
            Assert.AreEqual( 0, table[0] );
        }

        // Make sure it works with indexes

        // Make sure values added go to the correct index.

        [TestMethod]
        public void Index_Population_Test()
        {
            var table = new Table<TestResult>();
            var testIdsIndex = table.CreateIndex( result => result.TestId );

            table.Add( new TestResult( "AUTO_1", "1.0", 1 ) );
            table.Add( new TestResult( "AUTO_2", "1.0", 2 ) );
            table.Add( new TestResult( "AUTO_1", "2.0", 3 ) );
            table.Add( new TestResult( "AUTO_2", "2.0", 4 ) );

            var test1Results = testIdsIndex["AUTO_1"];
            Assert.AreEqual( 2, test1Results.Count );
            CollectionAssert.AreEqual( new[] { 1, 3 }, test1Results.Select( x => x.Value ).ToArray() );

            var test2Results = testIdsIndex["AUTO_2"];
            Assert.AreEqual( 2, test2Results.Count );
            CollectionAssert.AreEqual( new[] { 2, 4 }, test2Results.Select( x => x.Value ).ToArray() );
        }

        // Make sure adding and removing values updates the indexes correctly.
        [TestMethod]
        public void Updating_Table_Causes_Index_Updates()
        {
            var table = new Table<TestResult>();
            var testIdsIndex = table.CreateIndex( result => result.TestId );

            table.Add( new TestResult( "AUTO_1", "1.0", 1 ) );
            table.Add( new TestResult( "AUTO_1", "2.0", 2 ) );
            Assert.AreEqual( 1, testIdsIndex.Count );
            Assert.AreEqual( 2, testIdsIndex["AUTO_1"].Count );

            table.Add( new TestResult( "AUTO_2", "1.0", 3 ) );
            table.Add( new TestResult( "AUTO_2", "2.0", 4 ) );
            Assert.AreEqual( 2, testIdsIndex.Count );
            Assert.AreEqual( 2, testIdsIndex["AUTO_2"].Count );
        }


        // Make sure it regenerates new indexes when there's already values in the list.
        [TestMethod]
        public void Adding_Index_After_Adding_Data_Fills_Index()
        {
            var table = new Table<TestResult>();

            table.Add( new TestResult( "AUTO_1", "1.0", 1 ) );
            table.Add( new TestResult( "AUTO_2", "1.0", 2 ) );
            
            var testIdsIndex = table.CreateIndex( result => result.TestId );

            Assert.AreEqual( 2, testIdsIndex.Count );
            Assert.AreEqual( 1, testIdsIndex["AUTO_1"][0].Value );
            Assert.AreEqual( 2, testIdsIndex["AUTO_2"][0].Value );
        }

        [TestMethod]
        public void Composite_Keys()
        {
            var table = new Table<TestResult>();
            var resultIndex = table.CreateIndex( result => (result.TestId, result.Version) );

            table.Add( new TestResult( "AUTO_1", "1.0", 1 ) );
            table.Add( new TestResult( "AUTO_2", "1.0", 2 ) );
            table.Add( new TestResult( "AUTO_1", "2.0", 3 ) );
            table.Add( new TestResult( "AUTO_2", "2.0", 4 ) );

            Assert.AreEqual( 1, resultIndex[("AUTO_1", "1.0")][0].Value );
            Assert.AreEqual( 2, resultIndex[("AUTO_2", "1.0")][0].Value );
            Assert.AreEqual( 3, resultIndex[("AUTO_1", "2.0")][0].Value );
            Assert.AreEqual( 4, resultIndex[("AUTO_2", "2.0")][0].Value );

        }

        private class TestResult
        {
            public TestResult( string testId, string version, int value )
            {
                this.TestId = testId;
                this.Version = version;
                this.Value = value;
            }

            public string TestId { get; }
            public string Version { get; }
            public int Value { get; }
        }
    }
}
