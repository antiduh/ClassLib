using System;
using System.Collections;
using System.Collections.Generic;
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
        {
        }

        public BitList( int initialLength )
        {
        }

        public BitList( byte[] data, int numBits )
        {
        }

        public int Length
        {
            get
            {
                throw new NotImplementedException();
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
        /// <param name="numBytes"></param>
        /// <returns></returns>
        public static int BitsToBytes( int numBits )
        {
            throw new NotImplementedException();
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
    }
}
