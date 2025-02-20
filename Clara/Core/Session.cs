﻿using Clara.Utils;

namespace Clara.Core
{
    public static class Session
    {
        private static bool isRunning;

        public readonly static DateTime startTime = DateTime.Now;
        public readonly static string exePath = Environment.ProcessPath ?? "";
        public readonly static string rootPath = System.IO.Path.GetDirectoryName(exePath) ?? "";

        private static void Initialize()
        {
            Config.Initialize();
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
            Console.WriteLine();

            Log.Info("Started at: " + Session.GetStartTime().ToString("yyyy-MM-dd HH:mm:ss"));
            Log.Info("Uptime: " + Session.GetUptime().ToString(@"dd\.hh\:mm\:ss"));

            Console.WriteLine();

            Input.PressAnyKeyToContinue();
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
