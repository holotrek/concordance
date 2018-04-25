using System;
using System.IO;

namespace Concordance
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                throw new ArgumentNullException("Please provide the name of the file containing the text you want to parse like: dotnet run ./MyText.txt.");
            }

            string filename = args[0];
            if (!File.Exists(filename))
            {
                throw new ArgumentException($"File {args[0]} not found.");
            }

            string text = File.ReadAllText(filename);
            var generator = new ConcordanceGenerator(text);
            Console.Write(generator.ToString());
            Console.WriteLine();
        }
    }
}
