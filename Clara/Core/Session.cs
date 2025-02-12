namespace Clara.Core
{
    public static class Session
    {
        private static bool isRunning;
        private static DateTime startTime;

        private static void Initialize()
        {
            Log.Header("Starting session...");

            startTime = DateTime.Now;

            Config.Initialize();

            Log.Success("Session started!");
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
