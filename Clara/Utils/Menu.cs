namespace Clara.Utils
{
    public static class Menu
    {
        public static int Run(string[] options)
        {
            while (true)
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("--- MENU ---");
                Console.ResetColor();

                Console.BackgroundColor = ConsoleColor.Blue;
                for (int i = 0; i < options.Length; i++)
                {
                    Console.WriteLine($"  {i + 1}. {options[i]}");
                }
                Console.ResetColor();

                ConsoleKeyInfo key = Console.ReadKey();

                for (int i = 0; i < options.Length + 1; i++)
                {
                    Console.SetCursorPosition(0, Console.CursorTop - 1);
                    Console.Write(new string(' ', Console.WindowWidth));
                }

                if (int.TryParse(key.KeyChar.ToString(), out int index) && index > 0 && index <= options.Length)
                {
                    return index - 1;
                }
            }
        }
    }
}
