using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MultiWord
{
    class Program
    {
        static void Main(string[] args)
        {
            string outputFile = @"Files\Output-Örebro-Karlstad.txt";

            List<string> text = ReadFile(@"Files\Hamlet.txt");
            Dictionary<char, int> textLetters = string.Join("", text).GetLetterCount();

            List<string> dictionary = ReadFile(@"Files\Ordlista.txt");
            IEnumerable<string> matchingWords = dictionary
                .Where(word => word
                    .GetLetterCount()
                    .All(letter => textLetters.TryGetValue(letter.Key, out int textLetterCount) && textLetterCount >= letter.Value));

            File.WriteAllLines(outputFile, matchingWords);

            Console.WriteLine($"Found {matchingWords.Count()} matching words.");
            Console.WriteLine("Opening the output file for you now...");
            File.Open(outputFile, FileMode.Open);
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
