using Clara.Core;
using System.Text;

namespace Clara.Utils
{
    public static class Input
    {
        public static (string module, string[] args) ParseCommand(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (string.Empty, Array.Empty<string>());

            var tokens = new List<string>();
            var token = new StringBuilder();
            bool inQuotes = false;
            char quoteChar = '\0';

            foreach (char c in input)
            {
                if (char.IsWhiteSpace(c))
                {
                    if (!inQuotes)
                    {
                        if (token.Length > 0)
                        {
                            tokens.Add(token.ToString());
                            token.Clear();
                        }
                    }
                    else
                    {
                        token.Append(c);
                    }
                }
                else if (c == '"' || c == '\'')
                {
                    if (inQuotes && c == quoteChar)
                    {
                        inQuotes = false;
                        quoteChar = '\0';
                    }
                    else if (!inQuotes)
                    {
                        inQuotes = true;
                        quoteChar = c;
                    }
                    else
                    {
                        token.Append(c);
                    }
                }
                else
                {
                    token.Append(c);
                }
            }

            if (token.Length > 0)
                tokens.Add(token.ToString());

            if (tokens.Count == 0)
                return (string.Empty, Array.Empty<string>());

            string module = tokens[0];
            string[] args = tokens.Count > 1 ? tokens.Skip(1).ToArray() : Array.Empty<string>();

            return (module, args);
        }

        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static string Get(string? label = null)
        {
            if (!string.IsNullOrEmpty(label))
                Console.Write($"{label}: ");

            string? input = Console.ReadLine();

            if (!string.IsNullOrEmpty(input))
            {
                Log.User(input);
                return input.ToLowerInvariant();
            }

            return string.Empty;
        }
    }
}
