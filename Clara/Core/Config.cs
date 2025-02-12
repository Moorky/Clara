using System.Text.Json;

namespace Clara.Core
{
    public static class Config
    {
        private static string configPath = "config.json";
        private static Dictionary<string, string> configData = new Dictionary<string, string>();

        public static void Initialize()
        {
            Log.Header("Initializing config...");

            if (!File.Exists(configPath))
            {
                Log.Info("Config file not found. Creating a new one at '" + configPath + "'");

                CreateConfig();
            }
            else if (!IsValid())
            {
                Log.Warn("Config file at '" + configPath + "' is invalid. Creating a new one and renaming the old one.");

                CreateBackup();
                CreateConfig();
            }

            ReadConfig();

            Log.Success("Config initialized!");
        }

        public static string Get(string key)
        {
            ReadConfig();

            if (configData == null)
            {
                Log.Error($"Config data is null. Returning empty string for key '{key}'.");
                return "";
            }
            else if (!configData.ContainsKey(key))
            {
                Log.Error($"Key '{key}' not found in config data. Returning empty string.");
                return "";
            }

            return configData[key];
        }

        public static void Set(string key, string value)
        {
            if (configData == null)
            {
                Log.Warn($"Config data is null. Creating a new dictionary.");
                configData = new Dictionary<string, string>();
            }

            if (!configData.ContainsKey(key))
            {
                Log.Info($"Key '{key}' not found in config data. Adding a new key-value pair.");
            }

            configData[key] = value;

            WriteConfig();
        }

        public static Dictionary<string, string> GetAll()
        {
            ReadConfig();

            if (configData == null)
            {
                Log.Fatal("Config data is null. Returning an empty dictionary.");
                return new Dictionary<string, string>();
            }

            return configData;
        }

        private static void CreateConfig()
        {
            Utils.Path.Create(configPath, true);

            configData.Add("Username", "");

            WriteConfig();
        }

        private static bool IsValid()
        {
            try
            {
                string configJson = File.ReadAllText(configPath);

                JsonSerializer.Deserialize<Dictionary<string, string>>(configJson);

                return true;
            }
            catch (Exception e)
            {
                Log.Error("Error while validating config file: " + e.Message);
                return false;
            }
        }

        private static void CreateBackup()
        {
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
            try
            {
                string configJson = File.ReadAllText(configPath);

                Dictionary<string, string>? temp = JsonSerializer.Deserialize<Dictionary<string, string>>(configJson);

                if (temp != null)
                {
                    configData = temp;
                }
            }
            catch (Exception e)
            {
                Log.Fatal("Error while reading config file: " + e.Message);
            }
        }

        private static void WriteConfig()
        {
            try
            {
                string configJson = JsonSerializer.Serialize(configData);

                File.WriteAllText(configPath, configJson);
            }
            catch (Exception e)
            {
                Log.Fatal("Error while writing config file: " + e.Message);
            }
        }
    }
}
