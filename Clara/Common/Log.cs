namespace Clara.Common
{
    public static class Log
    {
        private static string GetTimestamp() => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        private static string CreateLog(string level, string message) => $"[{GetTimestamp()}] [{level}] {message}";
        private static void WriteToFile(string log)
        {
            string path = $"logs/log_{Session.GetStartTime().ToString("yyyy-MM-dd_HHmmss")}.txt";

            Utils.Path.Create(path, true);

            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(log);
            }
        }

        public static void Custom(string level, string message, ConsoleColor color)
        {
            string log = CreateLog(level, message);
            Console.ForegroundColor = color;
            Console.WriteLine(log);
            Console.ResetColor();
            WriteToFile(log);
        }

        public static void Info(string message)
        {
            string log = CreateLog("INFO", message);
            Console.WriteLine(log);
            WriteToFile(log);
        }

        public static void Warn(string message)
        {
            string log = CreateLog("WARN", message);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(log);
            Console.ResetColor();
            WriteToFile(log);
        }

        public static void Error(string message)
        {
            string log = CreateLog("ERROR", message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(log);
            Console.ResetColor();
            WriteToFile(log);
        }

        public static void Success(string message)
        {
            string log = CreateLog("SUCCESS", message);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(log);
            Console.ResetColor();
            WriteToFile(log);
        }
    }
}
