namespace Clara.Core
{
    public static class User
    {
        public static string Input(string label)
        {
            Console.Write(label + ": ");
            string? input = Console.ReadLine();

            if (input != null)
            {
                Log.User(input);
            }

            return input ?? "";
        }
    }
}
