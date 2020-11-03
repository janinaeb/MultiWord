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
            List<string> text = ReadFile($"{filePath}Hamlet.txt");
            Dictionary<char, int> textLetters = string.Join("", text).GetLetterCount();

            // Get the dictionary of words to match on
            List<string> dictionary = ReadFile($"{filePath}Ordlista.txt");

            // Find all possible matching words
            IEnumerable<string> allMatchingWords = dictionary
                .Where(word => word
                    .GetLetterCount()
                    .All(letter => textLetters.TryGetValue(letter.Key, out int textLetterCount) && textLetterCount >= letter.Value));

            Console.WriteLine($"Found at total {allMatchingWords.Count()} matching words.");

            // Get the most optimal matching setup
            List<string> bestMatchingWords = new List<string>();
            List<string> currentMatchingWords = new List<string>();
            Dictionary<char, int> textLettersLeft = new Dictionary<char, int>(textLetters);
            foreach (var matchingWord in allMatchingWords)
            {
                var letters = matchingWord.GetLetterCount();
                if (letters.All(letter => textLettersLeft.TryGetValue(letter.Key, out int textLetterCount) && textLetterCount >= letter.Value))
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

            // TODO: Find the most optimal matchings!
            bestMatchingWords = currentMatchingWords;

            File.WriteAllLines($"{filePath}Output-Örebro-Karlstad.txt", bestMatchingWords);


            Console.WriteLine($"Found most optimally {bestMatchingWords.Count()} matching words.");
            Console.WriteLine("Opening the file directory for you now...");
            Process.Start("explorer.exe", filePath);
            Console.WriteLine("Bye!");
            Console.ReadLine();
        }

        private static List<string> ReadFile(string name)
        {
            string line;
            List<string> lines = new List<string>();

            // Read the file and display it line by line.  
            StreamReader file = new StreamReader(name);
            while ((line = file.ReadLine()) != null)
            {
                lines.Add(line);
            }

            file.Close();

            return lines;
        }
    }
}
