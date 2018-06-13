using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Antiduh.ClassLib.Test
{
    [TestClass]
    public class DataConverterTests_Int
    {
        Dictionary<int, byte[]> vectors = new Dictionary<int, byte[]>
        {
            { 0, new byte[] { 0,0,0,0 } },
            { 1, new byte[] { 1,0,0,0 } },
            { -1, new byte[] { 0xFF,0xFF,0xFF,0xFF } },
            { unchecked( (int)0xDEADBEEF ), new byte[] { 0xEF,0xBE,0xAD,0xDE } },
            { int.MinValue, new byte[] {0, 0, 0, 128 } },
            { int.MaxValue, new byte[] {0xFF, 0xFF, 0xFF, 0x7F } }
        };

        [TestMethod]
        public void DataConverter_WriteIntLE_Rejects_InvalidBuffer()
        {
            byte[] small = new byte[3];
            byte[] exact = new byte[4];
            byte[] large = new byte[5];

            Assert2.Throws<ArgumentException>( () => DataConverter.WriteIntLE( 0, small, 0 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.WriteIntLE( 0, exact, 1 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.WriteIntLE( 0, large, 2 ) );
        }

        [TestMethod]
        public void DataConverter_ReadIntLE_Rejects_InvalidBuffer()
        {
            byte[] small = new byte[3];
            byte[] exact = new byte[4];
            byte[] large = new byte[5];

            Assert2.Throws<ArgumentException>( () => DataConverter.ReadIntLE( small, 0 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.ReadIntLE( exact, 1 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.ReadIntLE( large, 2 ) );
        }

        [TestMethod]
        public void DataConverter_WriteIntLE_KnownVectors()
        {
            byte[] data = new byte[4];

            foreach( int key in vectors.Keys )
            {
                Array.Clear( data, 0, data.Length );

                DataConverter.WriteIntLE( key, data, 0 );

                Assert.IsTrue( Array2.CompareArrays( data, vectors[key] ) );
            }
        }

        [TestMethod]
        public void DataConverter_IntLE_RoundTrip()
        {
            byte[] data = new byte[4];

            foreach( int key in vectors.Keys )
            {
                Array.Clear( data, 0, data.Length );

                DataConverter.WriteIntLE( key, data, 0 );

                int test = DataConverter.ReadIntLE( data, 0 );

                Assert.AreEqual( key, test );
            }
        }
    }
}