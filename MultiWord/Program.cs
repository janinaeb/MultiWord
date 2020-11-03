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
            List<string> text = ReadFile(@"Files\Hamlet.txt");
            Dictionary<char, int> textLetters = string.Join("", text).GetLetterCount();

            List<string> dictionary = ReadFile(@"Files\Ordlista.txt");
            IEnumerable<string> matchingWords = dictionary
                .Where(word => word
                    .GetLetterCount()
                    .All(letter => textLetters.TryGetValue(letter.Key, out int textLetterCount) && textLetterCount >= letter.Value));

            File.WriteAllLines(@"Files\Output-Örebro-Karlstad.txt", matchingWords);

            foreach (var word in matchingWords)
            {
                Console.WriteLine(word);
            }
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
