using Clara.Core;
using Microsoft.Win32;

namespace Clara.Modules
{
    public class AutoStart : Module
    {
        private string appName = "Clara";
        private string appPath = Session.exePath;

        protected override void Enter()
        {

        }

        protected override void Exit()
        {

        }

        protected override void Process()
        {
            try
            {
                if (IsAutoStartSet())
                {
                    Log.Info("Disabling autostart...");
                    SetAutoStart(false);
                    Log.Success("Autostart disabled.");
                }
                else
                {
                    Log.Info("Enabling autostart...");
                    SetAutoStart(true);
                    Log.Success("Autostart enabled.");
                }
            }
            catch (Exception ex)
            {
                Log.Failure($"An error occurred: {ex.Message}");
            }
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
