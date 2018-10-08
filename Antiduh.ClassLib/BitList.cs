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

        public int Length
        {
            get
            {
                return this.length;
            }
            set
            {

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
                int numBytes = BitsToBytes( requestedIndex + 1 );
                byte[] newData = new byte[numBytes];

                Array.Copy( this.data, newData, this.data.Length );

                this.data = newData;
            }
        }
    }
}
