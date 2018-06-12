using System;

namespace Antiduh.ClassLib
{
    /// <summary>
    /// Converts common data types from their native type to their big-endian and little-endian
    /// representations suitable for serialization.
    /// </summary>
    public static class DataConverter
    {
        /// <summary>
        /// Writes a 16-bit signed integer to the given byte array in little-endian format. 2 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteShortLE( short value, byte[] data, int start )
        {
            CheckWriteSize( data, start, 2 );

            data[start + 0] = (byte)( value >> 0 );
            data[start + 1] = (byte)( value >> 8 );
        }

        /// <summary>
        /// Reads a 16-bit signed integer from a byte array storing the value in
        /// little-endian format. 2 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static short ReadShortLE( byte[] data, int start )
        {
            CheckReadSize( data, start, 2 );

            short value;

            value = (short)( data[start + 0] + ( data[start + 1] << 8 ) );

            return value;
        }

        /// <summary>
        /// Returns the value of the given 16-bit signed integer as a byte array stored in
        /// little-endian format. 2 bytes are returned.
        /// </summary>
        /// <param name="value">The value to convert to a little-endian bytes.</param>
        /// <returns></returns>
        public static byte[] GetShortBytesLE( short value )
        {
            byte[] data = new byte[2];

            WriteShortLE( value, data, 0 );

            return data;
        }

        /// <summary>
        /// Writes a 32-bit signed integer to the given byte array in little-endian format. 4 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteIntLE( int value, byte[] data, int start )
        {
            CheckWriteSize( data, start, 4 );

            data[start + 0] = (byte)( value >> 0 );
            data[start + 1] = (byte)( value >> 8 );
            data[start + 2] = (byte)( value >> 16 );
            data[start + 3] = (byte)( value >> 24 );
        }

        /// <summary>
        /// Reads a 32-bit signed integer from a byte array storing the value in
        /// little-endian format. 4 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static int ReadIntLE( byte[] data, int start )
        {
            CheckReadSize( data, start, 4 );

            int value;

            value = data[start + 0];
            value |= data[start + 1] << 8;
            value |= data[start + 2] << 16;
            value |= data[start + 3] << 24;

            return value;
        }

        /// <summary>
        /// Returns the value of the given 32-bit signed integer as a byte array stored in
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

        /// <summary>
        /// Writes a 64-bit signed integer to the given byte array in little-endian format. 8 bytes
        /// are written to the array.
        /// </summary>
        /// <param name="value">The value to write to the byte array.</param>
        /// <param name="data">The array to write to.</param>
        /// <param name="start">The first index in the array to write to.</param>
        public static void WriteLongLE( long value, byte[] data, int start )
        {
            CheckWriteSize( data, start, 8 );

            data[start + 0] = (byte)( value >> 0 );
            data[start + 1] = (byte)( value >> 8 );
            data[start + 2] = (byte)( value >> 16 );
            data[start + 3] = (byte)( value >> 24 );

            data[start + 4] = (byte)( value >> 32 );
            data[start + 5] = (byte)( value >> 40 );
            data[start + 6] = (byte)( value >> 48 );
            data[start + 7] = (byte)( value >> 56 );
        }

        /// <summary>
        /// Reads a 64-bit signed integer from a byte array storing the value in
        /// little-endian format. 8 bytes are read from the array.
        /// </summary>
        /// <param name="data">The array to read from.</param>
        /// <param name="start">The first index in the array to read from.</param>
        public static long ReadLongLE( byte[] data, int start )
        {
            CheckReadSize( data, start, 8 );
            long value;

            value = (long)data[start + 0];
            value += (long)data[start + 1] << 8;
            value += (long)data[start + 2] << 16;
            value += (long)data[start + 3] << 24;

            value += (long)data[start + 4] << 32;
            value += (long)data[start + 5] << 40;
            value += (long)data[start + 6] << 48;
            value += (long)data[start + 7] << 56;

            return value;
        }

        /// <summary>
        /// Returns the value of the given 64-bit signed integer as a byte array stored in
        /// little-endian format. 8 bytes are returned.
        /// </summary>
        /// <param name="value">The value to convert to a little-endian bytes.</param>
        /// <returns></returns>
        public static byte[] GetLongBytesLE( long value )
        {
            byte[] data = new byte[8];

            WriteLongLE( value, data, 0 );

            return data;
        }

        private static void CheckWriteSize( byte[] data, int start, int neededSize )
        {
            if( data.Length - start < neededSize )
            {
                throw new ArgumentException( "The provided array is not large enough to read the value." );
            }
        }

        private static void CheckReadSize( byte[] data, int start, int neededSize )
        {
            if( data.Length - start < neededSize )
            {
                throw new ArgumentException( "The provided array is not large enough to contain the value to read." );
            }
        }
    }
}