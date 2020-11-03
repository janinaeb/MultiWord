using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

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
            
            dictionary.Sort((a, b) => a.Length.CompareTo(b.Length));

            // Find all possible matching words
            List<string> allMatchingWords = dictionary
                .Where(word => word.GetLetterCount().ExistsIn(allTextLetters))
                .ToList();

            Console.WriteLine($"Found at total {allMatchingWords.Count()} matching words.");

            // Get the most optimal matching setup
            List<string> bestMatchingWords = CalculateOptimalMatches(allMatchingWords, allTextLetters).Result;
            File.WriteAllLines($"{filePath}Output-Örebro-Karlstad.txt", bestMatchingWords);

            // Write output messages
            Console.WriteLine($"Found most optimally {bestMatchingWords.Count()} matching words.");
            Console.WriteLine("Opening the file directory for you now...");

            Process.Start("explorer.exe", filePath);
            
            Console.WriteLine("Bye!");
        }

        private static async Task<List<string>> CalculateOptimalMatches(List<string> allMatchingWords, Dictionary<char, int> allTextLetters)
        {
            // Get the most optimal matching setup
            // by excluding a word at a time
            List<string> bestMatchingWords = CalculateMatches(allMatchingWords, allTextLetters, new List<int>());
            Console.WriteLine($"We begin at {bestMatchingWords.Count} matches.");
            List<Task> matchingWorkChecks = new List<Task>();
            for (int i = 0; i < allMatchingWords.Count; i++)
            {
                int j = i;
                matchingWorkChecks.Add(Task.Factory.StartNew(() =>
                {
                    List<string> currentMatchingWords = CalculateMatches(allMatchingWords, allTextLetters, new List<int> { j });
                    if (currentMatchingWords.Count > bestMatchingWords.Count)
                    {
                        Console.WriteLine($"{currentMatchingWords.Count} matches are better than {bestMatchingWords.Count}...");
                        bestMatchingWords = currentMatchingWords;
                    }
                }));
            }
            await Task.WhenAll(matchingWorkChecks);
            return bestMatchingWords;
        }

        private static List<string> CalculateMatches(List<string> allMatchingWords, Dictionary<char, int> textLetters, List<int> excludeIndexes)
        {
            List<string> currentMatchingWords = new List<string>();
            Dictionary<char, int> textLettersLeft = new Dictionary<char, int>(textLetters);
            for (int i = 0; i < allMatchingWords.Count; i++)
            {
                if (excludeIndexes.Contains(i))
                {
                    continue;
                }
                // Stop loop if no letters left
                if (!textLettersLeft.Any())
                {
                    break;
                }
                var matchingWord = allMatchingWords[i];
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
                }
            }
            return currentMatchingWords;
        }
    }
}
