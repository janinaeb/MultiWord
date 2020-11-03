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

        public static bool ExistsIn(this Dictionary<char, int> letters, Dictionary<char, int> matchingAgainstLetters)
        {
            return letters.All(letter => matchingAgainstLetters.TryGetValue(letter.Key, out int textLetterCount) && textLetterCount >= letter.Value);
        }
    }
}
