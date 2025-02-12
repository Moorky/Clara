using Clara.Core;

namespace Clara.Utils
{
    public static class Salutation
    {
        public static void Welcome()
        {
            CheckNewUser();
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

        public static void Farewell()
        {
            Log.Clara($"Goodbye, {Config.Get("Username")}!");
        }
    }
}
