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

        private int length;

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

            this.length = initialLength;
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

            this.length = numBits;
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
                return this.length;
            }
            set
            {
                // If they're increasing the length of the bitlist, allocate more bytes. Easy.
                //
                // If they're decreasing the length of the BitList, it's a bit more complicated.
                // - We might want to decrease the length of the array if they reduce the length by enough.
                // - The last logical bit in the list might fall in the middle of the last remaining
                //   byte, so we have to clear any set bits above the last logical index.
                // - We're throwing away bits, so we have to count how many we're throwing away and
                //   reduce `Count` appropriately.


                int newLastIndex = length - 1;

                if( value > this.length )
                {
                    EnsureCapacity( length - 1 );
                }
                else
                {
                    // We're shrinking the logical size. Keep in mind that we may or may not shrink
                    // the array.

                    int newArraySize = ArraySize( value );

                    if( newArraySize < this.data.Length )
                    {

                    }
                }
            }
        }

        public int Count { get; private set; }

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
            if( requestedIndex >= this.length )
            {
                Array.Resize( ref this.data, ArraySize( requestedIndex + 1 ) );
            }
        }

        private int ArraySize( int numBits )
        {
            int numBytes;
            
            numBytes = BitsToBytes( numBits );
            numBytes = MathEx.RoundUpToMultiple( numBytes, arrayBlockSize );

            return numBytes;
        }

    }
}
