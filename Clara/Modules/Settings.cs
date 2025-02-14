using Clara.Core;
using Clara.Utils;
using Microsoft.Win32;
using System.Diagnostics;

namespace Clara.Modules
{
    public class Settings : Module
    {
        private string appName = Process.GetCurrentProcess().ProcessName;

        private string appPath = Session.exePath;

        protected override string[] _menuCommands => ["Autostart", "Exit"];

        protected override Dictionary<string, Action<string[]>> _commandHandlers => new Dictionary<string, Action<string[]>>
        {
            ["autostart"] = (args) => ToggleAutoStart()
        };

        protected override void Enter()
        {

        }

        protected override void Exit()
        {

        }

        private void ToggleAutoStart()
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (key == null)
                    throw new Exception("Failed to open registry key for autostart.");

                var value = key.GetValue(appName);
                if (value != null)
                {
                    string existingPath = value.ToString().Trim('"');
                    string currentPath = appPath.Trim('"');
                    if (string.Equals(existingPath, currentPath, StringComparison.OrdinalIgnoreCase))
                    {
                        key.DeleteValue(appName, false);
                        Log.Success("Clara will no longer start automatically at startup.");
                        return;
                    }
                }
                key.SetValue(appName, $"\"{appPath}\"");
                Log.Success("Clara will now start automatically at startup.");
            }
        }

    }
}
