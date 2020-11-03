using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace MultiWord
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"Files\";

            // Get all letters from the text to search
            List<string> text = FileHandler.ReadFile($"{filePath}Hamlet.txt");
            Dictionary<char, int> allTextLetters = string.Join("", text).GetLetterCount();

            // Get the dictionary of words to match on
            List<string> dictionary = FileHandler.ReadFile($"{filePath}Ordlista.txt");

            // Find all possible matching words
            IEnumerable<string> allMatchingWords = dictionary
                .Where(word => word.GetLetterCount().ExistsIn(allTextLetters));

            Console.WriteLine($"Found at total {allMatchingWords.Count()} matching words.");

            // Get the most optimal matching setup
            List<string> bestMatchingWords = new List<string>();
            List<string> currentMatchingWords = CalculateMatches(allMatchingWords, allTextLetters);

            // TODO: Find the most optimal matchings!
            bestMatchingWords = currentMatchingWords;

            File.WriteAllLines($"{filePath}Output-Örebro-Karlstad.txt", bestMatchingWords);

            // Write output messages
            Console.WriteLine($"Found most optimally {bestMatchingWords.Count()} matching words.");
            Console.WriteLine("Opening the file directory for you now...");

            Process.Start("explorer.exe", filePath);
            
            Console.WriteLine("Bye!");
        }

        private static List<string> CalculateMatches(IEnumerable<string> allMatchingWords, Dictionary<char, int> textLetters)
        {
            List<string> currentMatchingWords = new List<string>();
            Dictionary<char, int> textLettersLeft = new Dictionary<char, int>(textLetters);
            foreach (var matchingWord in allMatchingWords)
            {
                var letters = matchingWord.GetLetterCount();
                if (letters.ExistsIn(textLettersLeft))
                {
                    // Add match and subtract the letters from count
                    currentMatchingWords.Add(matchingWord);
                    foreach (var letter in letters)
                    {
                        textLettersLeft[letter.Key] -= letter.Value;
                        if (textLettersLeft[letter.Key] <= 0)
                        {
                            textLettersLeft.Remove(letter.Key);
                        }
                    }

                    // Stop loop if no letters left
                    if (!textLettersLeft.Any())
                    {
                        break;
                    }
                }
            }
            return currentMatchingWords;
        }
    }
}
