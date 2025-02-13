using Clara.Core;

namespace Clara.Utils
{
    public static class Input
    {
        public static (string, string[]) ParseCommand(string input)
        {
            string task = input.Contains(" ") ? input.Split(" ")[0] : input;
            string[] args = input.Contains(" ") ? input.Split(" ")[1..] : new string[0];

            return (task, args);
        }

        public static void PressAnyKeyToContinue()
        {
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static string Get(string? label = null)
        {
            if (label != null)
                Console.Write($"{label}: ");

            string? input = Console.ReadLine();

            if (input != null)
                Log.User(input);

            return input ?? "";
        }
    }
}
