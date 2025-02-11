namespace Clara.Common
{
    public static class Session
    {
        private static bool isRunning;
        private static DateTime startTime;

        private static void Initialize()
        {
            startTime = DateTime.Now;
        }

        public static void Start()
        {
            if (isRunning)
            {
                Log.Error("Session is already running.");
            }
            else
            {
                isRunning = true;

                Initialize();
            }
        }

        public static void Stop()
        {
            if (!isRunning)
            {
                Log.Error("Session is not running.");
            }
            else
            {
                isRunning = false;
            }
        }

        public static TimeSpan GetUptime() => DateTime.Now - startTime;
        public static DateTime GetStartTime() => startTime;
    }
}
