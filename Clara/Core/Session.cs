using Clara.Utils;

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

            Console.WriteLine();

            Salutation.Welcome();
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

        private static void Terminate()
        {
            Salutation.Farewell();

            Console.WriteLine();

            Log.Info("Started at: " + Session.GetStartTime().ToString("yyyy-MM-dd HH:mm:ss"));
            Log.Info("Uptime: " + Session.GetUptime().ToString(@"dd\.hh\:mm\:ss"));

            Console.WriteLine();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
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

                Terminate();
            }
        }

        public static TimeSpan GetUptime() => DateTime.Now - startTime;
        public static DateTime GetStartTime() => startTime;
    }
}
