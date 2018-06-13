using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Antiduh.ClassLib.Test
{
    [TestClass]
    public class DataConverterTests_Short
    {
        Dictionary<short, byte[]> vectors = new Dictionary<short, byte[]>
        {
            { 0, new byte[] { 0,0 } },
            { 1, new byte[] { 1,0 } },
            { -1, new byte[] { 0xFF, 0xFF } },
            { unchecked( (short)0xDEAD ), new byte[] { 0xAD,0xDE } },
            { short.MinValue, new byte[] { 0, 128 } },
            { short.MaxValue, new byte[] { 0xFF, 0x7F } }
        };

        [TestMethod]
        public void DataConverter_WriteShortLE_Rejects_InvalidBuffer()
        {
            byte[] small = new byte[1];
            byte[] exact = new byte[2];
            byte[] large = new byte[3];

            Assert2.Throws<ArgumentException>( () => DataConverter.WriteShortLE( 0, small, 0 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.WriteShortLE( 0, exact, 1 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.WriteShortLE( 0, large, 2 ) );
        }

        [TestMethod]
        public void DataConverter_ReadShortLE_Rejects_InvalidBuffer()
        {
            byte[] small = new byte[1];
            byte[] exact = new byte[2];
            byte[] large = new byte[3];

            Assert2.Throws<ArgumentException>( () => DataConverter.ReadShortLE( small, 0 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.ReadShortLE( exact, 1 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.ReadShortLE( large, 2 ) );
        }

        [TestMethod]
        public void DataConverter_WriteShortLE_KnownVectors()
        {
            byte[] data = new byte[2];

            foreach( short key in vectors.Keys )
            {
                Array.Clear( data, 0, data.Length );

                DataConverter.WriteShortLE( key, data, 0 );

                Assert.IsTrue( Array2.CompareArrays( data, vectors[key] ) );
            }
        }

        [TestMethod]
        public void DataConverter_ShortLE_RoundTrip()
        {
            byte[] data = new byte[2];

            foreach( short key in vectors.Keys )
            {
                Array.Clear( data, 0, data.Length );

                DataConverter.WriteShortLE( key, data, 0 );

                short test = DataConverter.ReadShortLE( data, 0 );

                Assert.AreEqual( key, test );
            }
        }
    }
}
