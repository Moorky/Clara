namespace Clara.Utils
{
    public static class Menu
    {
        public static int Run(string header, string[] options)
        {
            while (true)
            {
                Console.Clear();

                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine($"---- {header.ToUpper()} ----");
                Console.ResetColor();

                Console.BackgroundColor = ConsoleColor.Blue;
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"  {i + 1}. {options[i]}");
                }
                Console.ResetColor();

                ConsoleKeyInfo key = Console.ReadKey(true);

                Console.Clear();

                if (int.TryParse(key.KeyChar.ToString(), out int index) && index > 0 && index <= options.Length)
                {
                    return index - 1;
                }
            }
        }
    }
}
