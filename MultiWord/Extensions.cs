using System;
using System.Collections.Generic;
using System.Linq;

namespace MultiWord
{
    public static class Extensions
    {
        public static Dictionary<char, int> GetLetterCount(this string word)
        {
            return word
                .ToUpperInvariant()
                .ToCharArray()
                .GroupBy(letter => letter)
                .ToDictionary(
                    key => key.Key,
                    value => value.Count()
            );
        }
    }
}
