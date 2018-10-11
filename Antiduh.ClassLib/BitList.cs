using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antiduh.ClassLib
{
    public class BitList : IEnumerable<bool>
    {
        /// <summary>
        /// Specifies the smallest amount of change in the data array's length - the array's length
        /// will always be a multiple of this number. This is used to reduce the amount of memory
        /// re-allocation churn that occurs when changing the length of the BitList.
        /// </summary>
        private const int arrayBlockSize = 1024;

        /// <summary>
        /// The number of logical bits stored in the list.
        /// </summary>
        private int bitLength;

        /// <summary>
        /// The buffer for storing set bits. Note, the buffer may have room for more bits than the
        /// list logically contains. For instance, if bitLength = 5, then the buffer is one byte and
        /// the top 3 bits are unused.
        /// </summary>
        private byte[] data;

        public BitList()
            : this( 0 )
        {
        }

        public BitList( int initialLength )
        {
            if( initialLength < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( initialLength ), initialLength, "Must be zero or greater." );
            }

            this.bitLength = initialLength;
            this.data = new byte[BitsToBytes( initialLength )];
        }

        public BitList( byte[] buffer, int numBits )
        {
            var neededBytes = BitsToBytes( numBits );

            if( buffer == null )
            {
                throw new ArgumentNullException( nameof( buffer ) );
            }

            if( numBits < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( numBits ), numBits, "Must be zero or greater." );
            }

            if( buffer.Length < neededBytes )
            {
                throw new ArgumentException( "The length of the array is too short to store the number of requested bits." );
            }

            this.bitLength = numBits;
            this.data = new byte[neededBytes];

            for( int i = 0; i < this.data.Length; i++ )
            {
                this.data[i] = buffer[i];
            }
        }
        
        /// <summary>
        /// Gets or sets the number of bits stored in the BitList.
        /// </summary>
        public int Length
        {
            get
            {
                return this.bitLength;
            }
            set
            {
                // Increasing the bitlength of the list may not change the array length.
                // - Increasing the bitlength by 1 may mean that we go from storing 2 bits to 3. No
                // change at all in the number of bytes needed to handle that.
                // - The array is allocated in large blocks (eg, 1024 bytes). If the bitlength of the
                // list is only 3 bits, then 1023 bytes are past the logical end of the list.
                //
                // Unused bits are always zero. We clear bits when they become unused by any means.
                //
                // When we increase the bitlength of the list, we assume any newly used bytes are
                // already zero and no clearing is necessary.
                //
                // When we decrease the bitlength of the list, we clear any bits that are past the
                // logical end of the list. This includes bits in any completely unused bytes, as well
                // as bits that might be in a partially-used byte.
                //
                // Increasing the size of the list:
                // - Reallocate the array if we don't have enough bytes to store the requested amount.
                // - Save the new bitlength of the list.
                //
                // Decreasing the size of the list:
                // - Reallocate the array if we have enough bytes to give up a whole chunk.
                // - Make sure any bits past the logical end of the list are zeroed.

                /*
                    Example. Let's say that our block size is 8 bytes. The list could look like this:

                    |<---------- data.Length = 8 bytes ---------------------------->|
                    |<---------- bitLength = 43 bits --------->|                    |
                    |                                          |                    |
                    +-------+-------+-------+-------+-------+-------+-------+-------+
                    |   0   |   1   |   2   |   3   |   4   |   5   |   6   |   7   |
                    +-------+-------+-------+-------+-------+-------+-------+-------+

                    - 6 total bytes are in use - BitsToBytes( 43 ) = 6
                    - 43 mod 8 = 3, meaning that the last byte is partially used. 3 bits are in use, 5 are not.
                    - Bytes 0-4 are completely used. 
                    - Byte 5 is partially used.
                    - Bytes 6 and 7 are completely unused, and are past the logical end of the list.
                */

                int newBitLength = value;
                int newLastIndex = newBitLength - 1;

                if( newBitLength == this.bitLength )
                {
                    return;
                }
                else if( newBitLength > this.bitLength )
                {
                    EnsureCapacity( newLastIndex );
                }
                else
                {
                    // - Resize the array if we can give up an entire chunk.
                    // - Clear bits that are past the logical end of the array. This could be bits in
                    //   unused bytes, or high bits in a partially used byte.

                    int newArrayLength = CalcArrayLength( newBitLength );

                    if( newArrayLength < this.data.Length )
                    {
                        Array.Resize( ref this.data, newArrayLength );
                    }

                    // How many bits are used in the the partially-used block (if one exists).
                    int partialBits = newBitLength % 8;

                    // The last byte used by the list. May be partially filled, or completely filled.
                    int lastByte = BitsToBytes( newBitLength ) - 1;

                    // See if we have a partially used byte and clear its high bits if so.
                    if( partialBits > 0 )
                    {
                        // Generate a mask that matches only the bottom partial bits.
                        byte mask = (byte)(( 1 << partialBits ) - 1);

                        // Zero the high bits using the mask.
                        this.data[lastByte] &= mask;
                    }

                    // Clear all bytes after the last byte.
                    int firstUnused = lastByte + 1;
                    Array.Clear( this.data, firstUnused, this.data.Length -  firstUnused );
                }
            }
        }
        
        public bool this[int index]
        {
            get
            {
                return Get( index );
            }
            set
            {
                Set( index, value );
            }
        }

        public void Set( int index, bool value )
        {
        }

        public bool Get( int index )
        {
            throw new NotImplementedException();
        }

        public void CopyTo( byte[] buffer, int start )
        {
        }

        public void CopyFrom( byte[] buffer, int start, int numBits )
        {
            
        }

        public void ShiftUp( int shiftCount )
        {
        }

        public void ShiftDown( int shiftCount )
        {
        }

        public void Or( BitList other )
        {
        }

        public void And( BitList other )
        {
        }

        public void Xor( BitList other )
        {
        }

        public void Concatenate( BitList other )
        {
        }

        public void Not()
        {
        }

        public BitList Clone()
        {
            throw new NotImplementedException();
        }

        public BitList Substring( int firstIndex, int numBits )
        {
            throw new NotImplementedException();
        }

        public IEnumerator<bool> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private void EnsureCapacity( int requestedIndex )
        {
            int newBitLength = requestedIndex + 1;

            if( newBitLength > this.bitLength )
            {
                int newByteLength = CalcArrayLength( newBitLength );

                // Resize the array if need. May not be needed if there's room in the current chunk.
                if( newByteLength > this.data.Length )
                {
                    Array.Resize( ref this.data, newByteLength );
                }

                // Save how many bits are in use now.
                this.bitLength = newBitLength;
            }
        }

        /// <summary>
        /// Calculates the number of bytes needed to store the given number of bits.
        /// </summary>
        /// <param name="numBits"></param>
        /// <returns></returns>
        public static int BitsToBytes( int numBits )
        {
            if( numBits < 0 )
            {
                throw new ArgumentOutOfRangeException( nameof( numBits ), numBits, "Must be zero or greater." );
            }

            if( numBits == 0 )
            {
                return 0;
            }

            return ( numBits - 1 ) / 8 + 1;
        }

        private static int CalcArrayLength( int numBits )
        {
            int numBytes;
            
            numBytes = BitsToBytes( numBits );
            numBytes = MathEx.RoundUpToMultiple( numBytes, arrayBlockSize );

            return numBytes;
        }
    }
}
