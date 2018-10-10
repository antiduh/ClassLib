using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Antiduh.ClassLib
{
    public static class MathEx
    {
        public static int CountSetBits( int value )
        {
            // Thanks Kernighan.

            int count = 0;

            while( value > 0 )
            {
                value &= ( value - 1 );
                count++;
            }

            return count;
        }

        public static int CountSetBits( long value )
        {
            int count = 0;

            while( value > 0 )
            {
                value &= ( value - 1 );
                count++;
            }

            return count;
        }


        public static int RoundUpToMultiple( int value, int multiple )
        {
            return ( ( value + 1 ) / multiple ) * multiple;
        }
    }
}
