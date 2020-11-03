using System.Collections.Generic;
using System.IO;

namespace MultiWord
{
    public static class FileHandler
    {
        public static List<string> ReadFile(string name)
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
