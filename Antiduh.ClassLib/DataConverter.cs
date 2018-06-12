using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antiduh.ClassLib
{
    /// <summary>
    /// Converts common data types from their native type to their big-endian and little-endian
    /// representations suitable for serialization.
    /// </summary>
    public static class DataConverter
    {
        /// <summary>
        /// Writes a 32-bit signed integer to the given byte array in little-endian format. 4 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteIntLE( int value, byte[] data, int start )
        {
            if( data.Length - start < 4 )
            {
                throw new ArgumentException( "The provided array is not large enough to store the result." );
            }

            data[start + 0] = (byte)( value >> 0 );
            data[start + 1] = (byte)( value >> 8 );
            data[start + 2] = (byte)( value >> 16 );
            data[start + 3] = (byte)( value >> 24 );
        }

        /// <summary>
        /// Reads a 32-bit signed integer from a byte array that has the value stored in
        /// little-endian format. 4 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static int ReadIntLE( byte[] data, int start )
        {
            if( data.Length - start < 4 )
            {
                throw new ArgumentException( "The provided array is not large enough to read the value." );
            }

            int value = 0;

            value = data[start + 0];
            value |= data[start + 1] << 8;
            value |= data[start + 2] << 16;
            value |= data[start + 3] << 24; 

            return value;
        }

        /// <summary>
        /// Returns the bytes representing the value of the given 32-bit signed integer stored in
        /// little-endian format. 4 bytes are returned.
        /// </summary>
        /// <param name="value">The value to convert to a little-endian bytes.</param>
        /// <returns></returns>
        public static byte[] GetIntBytesLE( int value )
        {
            byte[] data = new byte[4];

            WriteIntLE( value, data, 0 );

            return data;
        }

    }
}
