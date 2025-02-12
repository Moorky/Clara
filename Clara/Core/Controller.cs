namespace Clara.Core
{
    public static class Controller
    {
        public static void Start()
        {
            Session.Start();

            Welcome();
        }

        private static void Welcome()
        {
            CheckNewUser();

            Console.WriteLine();

            Greeting();
        }

        private static void Greeting()
        {
            int hour = DateTime.Now.Hour;

            if (hour >= 5 && hour < 12)
            {
                Log.Clara($"Good morning, {Config.Get("Username")}!");
            }
            else if (hour >= 12 && hour < 18)
            {
                Log.Clara($"Hi, {Config.Get("Username")}!");
            }
            else if (hour >= 18 && hour < 23)
            {
                Log.Clara($"Hey, {Config.Get("Username")}!");
            }
            else
            {
                Log.Clara($"Up to no good, {Config.Get("Username")}?");
            }
        }

        private static void CheckNewUser()
        {
            if (Config.Get("Username") == "")
            {
                Console.WriteLine();

                Log.Clara("Hi I'm Clara! Who are you?");

                bool valid = false;

                while (!valid)
                {
                    string name = User.Input("Username");

                    if (name != "" && name.Length < 20)
                    {
                        Config.Set("Username", name);
                        valid = true;
                    }
                    else
                    {
                        Log.Clara("Please enter a valid username.");
                    }
                }
            }
        }

        public static void Stop()
        {
            Session.Stop();

            Farewell();

            Exit();
        }

        private static void Farewell()
        {
            Console.WriteLine();

            Log.Info("Started at: " + Session.GetStartTime().ToString("yyyy-MM-dd HH:mm:ss"));
            Log.Info("Uptime: " + Session.GetUptime().ToString(@"dd\.hh\:mm\:ss"));

            Console.WriteLine();

            Log.Clara($"Goodbye, {Config.Get("Username")}!");
        }

        private static void Exit()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
