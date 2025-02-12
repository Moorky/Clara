namespace Clara.Core
{
    public static class Log
    {
        private static string GetTimestamp() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        private static string GetLogMessage(string level, string message) => $"[{GetTimestamp()}] [{level}] {message}";

        private static void WriteToFile(string log)
        {
            string path = $"logs/log_{Session.GetStartTime().ToString("yyyy-MM-dd_HHmmss")}.txt";

            Utils.Path.Create(path, true);

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(log);
            }
        }

        private static void Create(string level, string message, ConsoleColor color)
        {
            string log = GetLogMessage(level, message);
            Console.ForegroundColor = color;
            Console.WriteLine(log);
            Console.ResetColor();
            WriteToFile(log);
        }

        public static void Custom(string level, string message, ConsoleColor color)
        {
            Create(level, message, color);
        }

        public static void Trace(string message)
        {
            Create("TRACE", message, ConsoleColor.Gray);
        }

        public static void Info(string message)
        {
            Create("INFO", message, ConsoleColor.White);
        }

        public static void Header(string message)
        {
            Create("HEADER", message, ConsoleColor.Green);
        }

        public static void Warn(string message)
        {
            Create("WARN", message, ConsoleColor.Yellow);
        }

        public static void Error(string message)
        {
            Create("ERROR", message, ConsoleColor.Red);
        }

        public static void Fatal(string message)
        {
            Create("FATAL", message, ConsoleColor.DarkRed);
        }

        public static void Success(string message)
        {
            Create("SUCCESS", message, ConsoleColor.Green);
        }

        public static void Failure(string message)
        {
            Create("FAILURE", message, ConsoleColor.Red);
        }

        public static void Clara(string message)
        {
            Create("CLARA", message, ConsoleColor.Magenta);
        }

        public static void User(string message)
        {
            Create("USER", message, ConsoleColor.Cyan);
        }
    }
}
