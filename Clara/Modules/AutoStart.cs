using Clara.Core;
using Clara.Utils;
using Microsoft.Win32;

namespace Clara.Modules
{
    public class AutoStart : Module
    {
        private string appName = "Clara";
        private string appPath = Session.exePath;

        protected override string[] _menuCommands => throw new NotImplementedException();

        protected override Dictionary<string, Action<string[]>> _commandHandlers => throw new NotImplementedException();

        protected override void Menu()
        {

        }

        protected override void Enter()
        {
            
        }

        protected override void Exit()
        {
            
        }

        private bool IsAutoStartSet()
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false))
            {
                if (key == null)
                    throw new Exception("Failed to open registry key for autostart.");

                object? value = key.GetValue(appName);
                if (value != null)
                {
                    // Remove surrounding quotes (if any) for a fair comparison
                    string existingPath = value.ToString().Trim('"');
                    string currentPath = appPath.Trim('"');

                    return string.Equals(existingPath, currentPath, StringComparison.OrdinalIgnoreCase);
                }
                return false;
            }
        }

        private void SetAutoStart(bool enable)
        {
            using (RegistryKey? key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true))
            {
                if (key == null)
                    throw new Exception("Failed to open registry key for autostart.");

                if (enable)
                {
                    key.SetValue(appName, $"\"{appPath}\"");
                }
                else
                {
                    key.DeleteValue(appName, false);
                }
            }
        }
    }
}
