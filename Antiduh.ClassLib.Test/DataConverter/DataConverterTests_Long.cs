using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Antiduh.ClassLib.Test
{
    [TestClass]
    public class DataConverterTests_Long
    {
        Dictionary<long, byte[]> vectors = new Dictionary<long, byte[]>
        {
            { 0, new byte[] { 0,0,0,0, 0,0,0,0 } },
            { 1, new byte[] { 1,0,0,0, 0,0,0,0 } },
            { -1, new byte[] { 0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF } },
            { unchecked( (long)0xDEADBEEFDEADBEEF ), new byte[] { 0xEF,0xBE,0xAD,0xDE, 0xEF,0xBE,0xAD,0xDE } },
            { long.MinValue, new byte[] { 0,0,0,0, 0,0,0,128 } },
            { long.MaxValue, new byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0x7F } }
        };

        [TestMethod]
        public void DataConverter_WriteLongLE_Rejects_InvalidBuffer()
        {
            byte[] small = new byte[7];
            byte[] exact = new byte[8];
            byte[] large = new byte[9];

            Assert2.Throws<ArgumentException>( () => DataConverter.WriteLongLE( 0, small, 0 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.WriteLongLE( 0, exact, 1 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.WriteLongLE( 0, large, 2 ) );
        }

        [TestMethod]
        public void DataConverter_ReadLongLE_Rejects_InvalidBuffer()
        {
            byte[] small = new byte[7];
            byte[] exact = new byte[8];
            byte[] large = new byte[9];

            Assert2.Throws<ArgumentException>( () => DataConverter.ReadLongLE( small, 0 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.ReadLongLE( exact, 1 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.ReadLongLE( large, 2 ) );
        }

        [TestMethod]
        public void DataConverter_WriteLongLE_KnownVectors()
        {
            byte[] data = new byte[8];

            foreach( long key in vectors.Keys )
            {
                Array.Clear( data, 0, data.Length );

                DataConverter.WriteLongLE( key, data, 0 );

                Assert.IsTrue( Array2.CompareArrays( data, vectors[key] ) );
            }
        }

        [TestMethod]
        public void DataConverter_LongLE_RoundTrip()
        {
            byte[] data = new byte[8];

            foreach( long key in vectors.Keys )
            {
                Array.Clear( data, 0, data.Length );

                DataConverter.WriteLongLE( key, data, 0 );

                long test = DataConverter.ReadLongLE( data, 0 );

                Assert.AreEqual( key, test );
            }
        }
    }
}
