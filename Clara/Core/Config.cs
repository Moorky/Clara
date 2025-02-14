using System.Text.Json;
using System.Text.Json.Nodes;
using Clara.Utils;

namespace Clara.Core
{
    public static class Config
    {
        private static readonly string configPath = Session.rootPath + "/config.json";
        private static JsonObject configData = new JsonObject();

        public static void Initialize()
        {
            if (!File.Exists(configPath))
            {
                Log.Info($"Config file not found. Creating a new one at '{configPath}'.");
                CreateConfig();
            }
            else if (!IsValid())
            {
                Log.Warn($"Config file at '{configPath}' is invalid. Creating a new one and renaming the old one.");
                CreateBackup();
                CreateConfig();
            }
            else
            {
                ReadConfig();
            }
        }

        public static T? Get<T>(string key)
        {
            ReadConfig();

            if (configData == null)
            {
                Log.Error($"Config data is null. Returning default for key '{key}'.");
                return default;
            }

            if (!configData.ContainsKey(key))
            {
                Log.Error($"Key '{key}' not found in config data. Returning default.");
                return default;
            }

            try
            {
                JsonNode? node = configData[key];
                if (node == null)
                {
                    return default;
                }

                return node.Deserialize<T>();
            }
            catch (Exception e)
            {
                Log.Error($"Error while converting key '{key}' to type {typeof(T).Name}: {e.Message}");
                return default;
            }
        }

        public static void Set<T>(string key, T value)
        {
            if (configData == null)
            {
                Log.Warn("Config data is null. Creating a new JsonObject.");
                configData = new JsonObject();
            }

            JsonNode? node = JsonSerializer.SerializeToNode(value);
            configData[key] = node;

            WriteConfig();
        }

        public static JsonObject GetAll()
        {
            ReadConfig();

            if (configData == null)
            {
                Log.Fatal("Config data is null. Returning an empty JsonObject.");
                return new JsonObject();
            }

            return configData;
        }

        private static void CreateConfig()
        {
            Utils.Path.Create(configPath, true);

            configData = new JsonObject { };

            WriteConfig();
        }

        private static bool IsValid()
        {
            try
            {
                string configJson = File.ReadAllText(configPath);
                JsonNode? node = JsonNode.Parse(configJson);
                return node is JsonObject;
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
                JsonNode? node = JsonNode.Parse(configJson);

                if (node is JsonObject obj)
                {
                    configData = obj;
                }
                else
                {
                    Log.Warn("Config file does not contain a valid JSON object. Reinitializing config.");
                    configData = new JsonObject();
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
                string configJson = configData.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(configPath, configJson);
            }
            catch (Exception e)
            {
                Log.Fatal("Error while writing config file: " + e.Message);
            }
        }
    }
}
