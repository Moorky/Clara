using Clara.Common;

namespace Clara.Settings
{
    public static class Config
    {
        private static string? configPath;

        public static void Initialize(string configPath)
        {
            Config.configPath = configPath;

            if (!File.Exists(configPath))
            {
                Log.Info("Config file not found. Creating a new one at " + configPath);

                CreateConfig();
            }
            else if (!IsValid())
            {
                Log.Warn("Config file at " + configPath + " is invalid. Creating a new one and renaming the old one.");

                CreateBackup();
                CreateConfig();
            }

            ReadConfig();
        }

        public static string Get(string key)
        {
            ReadConfig();

            // TODO: Implement

            return "";
        }

        public static void Set(string key, string value)
        {
            // TODO: Implement

            WriteConfig();
        }

        private static void CreateConfig()
        {
            if (configPath == null)
            {
                Log.Error("Config path is null.");
                return;
            }

            Utils.Path.Create(configPath, true);
        }

        private static bool IsValid()
        {
            return false;
        }

        private static void CreateBackup()
        {
            if (configPath == null)
            {
                Log.Error("Config path is null.");
                return;
            }

            string backupPath = configPath.Replace(".json", "_backup.json");

            if (File.Exists(backupPath))
            {
                File.Delete(backupPath);
            }

            File.Copy(configPath, backupPath);
            File.Delete(configPath);
        }

        private static void ReadConfig()
        {

        }

        private static void WriteConfig()
        {

        }
    }
}
