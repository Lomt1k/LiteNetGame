using System;
using System.Collections.Generic;

public static class StringExtensions
{
    public static IEnumerable<string> SplitByLength(this string str, int size)
    {
        if (string.IsNullOrEmpty(str) || size < 1)
            throw new ArgumentException();
 
        for (int i = 0; i < str.Length; i += size) 
        {
            yield return str.Substring(i, Math.Min(size, str.Length - i));
        }
    }
    
}
