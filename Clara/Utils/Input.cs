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
    }
}
