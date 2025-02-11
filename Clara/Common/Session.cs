namespace Clara.Common
{
    public static class Session
    {
        private static DateTime startTime = DateTime.Now;

        public static TimeSpan GetUptime() => DateTime.Now - startTime;
        public static DateTime GetStartTime() => startTime;
    }
}
