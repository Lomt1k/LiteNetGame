using System;

namespace Utils
{
    public static class ByteHelper
    {
        public static byte PackBooleansToByte(params bool[] booleans)
        {
            if (booleans.Length > 8)
                throw new ArgumentException("PackBooleansToByte allows maximum of 8 values");
            
            byte result = 0;
            for (byte i = 0; i < booleans.Length; i++)
            {
                if (booleans[i])
                    result |= (byte)(1 << i);
            }

            return result;
        }

        public static bool[] GetBooleansFromByte(byte bitArray)
        {
            return new[]
            {
                ((bitArray & ( 1 << 0)) != 0),
                ((bitArray & ( 1 << 1)) != 0),
                ((bitArray & ( 1 << 2)) != 0),
                ((bitArray & ( 1 << 3)) != 0),
                ((bitArray & ( 1 << 4)) != 0),
                ((bitArray & ( 1 << 5)) != 0),
                ((bitArray & ( 1 << 6)) != 0),
                ((bitArray & ( 1 << 7)) != 0),
            };
        }
        
        
    }
}
