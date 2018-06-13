using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Antiduh.ClassLib.Test
{
    [TestClass]
    public class DataConverterTests_Decimal
    {
        // Vectors are from Microsoft's reference page on the decimal type:
        // - https://msdn.microsoft.com/en-us/library/system.decimal.getbits(v=vs.110).aspx

        Dictionary<decimal, string> vectors = new Dictionary<decimal, string>
        {
            /* Argument         Bits[3]    Bits[2]   Bits[1]   Bits[0] */
            { 0M,               "00000000  00000000  00000000  00000000" },
            { 1M,               "00000000  00000000  00000000  00000001" },
            { 123.456M,         "00030000  00000000  00000000  0001E240" },
            { 123456M,          "00000000  00000000  00000000  0001E240" },
            { 123456789M,       "00000000  00000000  00000000  075BCD15" },
            { 0.123456789M,     "00090000  00000000  00000000  075BCD15" },
            { 100000000000000M, "00000000  00000000  00005AF3  107A4000" },
            { 79228162514264337593543950335M,   "00000000  FFFFFFFF  FFFFFFFF  FFFFFFFF" },
            { -79228162514264337593543950335M,  "80000000  FFFFFFFF  FFFFFFFF  FFFFFFFF" },
            { -7.9228162514264337593543950335M, "801C0000  FFFFFFFF  FFFFFFFF  FFFFFFFF" },
        };

        [TestMethod]
        public void DataConverter_WriteDecimalLE_Rejects_InvalidBuffer()
        {
            byte[] small = new byte[15];
            byte[] exact = new byte[16];
            byte[] large = new byte[17];

            Assert2.Throws<ArgumentException>( () => DataConverter.WriteDecimalLE( 0, small, 0 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.WriteDecimalLE( 0, exact, 1 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.WriteDecimalLE( 0, large, 2 ) );
        }

        [TestMethod]
        public void DataConverter_ReadDecimalLE_Rejects_InvalidBuffer()
        {
            byte[] small = new byte[15];
            byte[] exact = new byte[16];
            byte[] large = new byte[17];

            Assert2.Throws<ArgumentException>( () => DataConverter.ReadDecimalLE( small, 0 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.ReadDecimalLE( exact, 1 ) );
            Assert2.Throws<ArgumentException>( () => DataConverter.ReadDecimalLE( large, 2 ) );
        }

        [TestMethod]
        public void DataConverter_WriteDecimalLE_KnownVectors()
        {
            byte[] testBytes = new byte[16];
            byte[] knownBytes;

            foreach( decimal key in vectors.Keys )
            {
                Array.Clear( testBytes, 0, testBytes.Length );
               
                DataConverter.WriteDecimalLE( key, testBytes, 0 );

                knownBytes = FromStrings( vectors[key] );

                Assert.IsTrue( Array2.CompareArrays( testBytes, knownBytes ) );
            }
        }

        [TestMethod]
        public void DataConverter_DecimalLE_RoundTrip()
        {
            byte[] data = new byte[16];

            foreach( decimal key in vectors.Keys )
            {
                Array.Clear( data, 0, data.Length );

                DataConverter.WriteDecimalLE( key, data, 0 );

                decimal test = DataConverter.ReadDecimalLE( data, 0 );

                Assert.AreEqual( key, test );
            }
        }

        private byte[] FromStrings( string hexString )
        {
            /*
                hexString is a string that is of the same format as one of the rows below:

                Bits[3]   Bits[2]   Bits[1]   Bits[0]
                00000000  00000000  00000000  00000001
                00000000  00000000  00005AF3  107A4000

                This format is borrowed from:
                https://msdn.microsoft.com/en-us/library/system.decimal.getbits(v=vs.110).aspx

                As a byte array, "00000000  00000000  00005AF3  107A4000" would be written:
                { 0x00, 0x40, 0x7a, 0x10, 0xf3, 0x5a, 0x00, 0x00, ... }
            */

            byte[] result = new byte[16];
            int byteIndex = 15;

            hexString = hexString.Replace( " ", "" );

            for( int i = 0; i < hexString.Length; i += 2 )
            {
                byte chunk = Convert.ToByte( hexString.Substring( i, 2 ), 16 );

                result[byteIndex] = chunk;
                byteIndex--;
            }

            return result;
        }
    }
}
