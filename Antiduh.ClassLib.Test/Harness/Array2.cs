using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antiduh.ClassLib.Test
{
    public static class Array2
    {
        public static bool CompareArrays<T>( T[] left, T[] right )
        {
            if( left.Length != right.Length )
            {
                throw new ArgumentException( "Arrays must be of equal length." );
            }

            for( int i = 0; i < left.Length; i++ )
            {
                if( left[i].Equals( right[i] ) == false )
                {
                    return false;
                }
            }

            return true;
        }
    }
}
