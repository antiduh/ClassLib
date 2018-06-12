using System;
using Antiduh.ClassLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Antiduh.ClassLib.Test
{
    [TestClass]
    public class DataConverterTests
    {
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
    }
}
