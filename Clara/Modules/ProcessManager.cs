using System.Diagnostics;
using Clara.Core;
using Clara.Utils;

namespace Clara.Modules
{
    public class ProcessManager : Module
    {
        protected override Dictionary<string, Action> _menuHandlers => new Dictionary<string, Action>
        {
            { "start", () => args.Add(Input.Get("Process")) },
            { "stop", () => args.Add(Input.Get("Process")) },
            { "exit", () => { } }
        };

        protected override Dictionary<string, Action<string[]>> _argHandlers => new Dictionary<string, Action<string[]>>()
        {
            { "start", parameters =>
                {
                    if (parameters.Length == 0)
                        Log.Error("Argument missing.");

                    else if (parameters.Length > 1)
                        Log.Error("Too many arguments.");

                    else
                        Start(parameters[0]);
                }
            },
            { "stop", parameters =>
            {
                    if (parameters.Length == 0)
                        Log.Error("Argument missing.");

                    else if (parameters.Length > 1)
                        Log.Error("Too many arguments.");

                    else
                        Stop(parameters[0]);
                }
            },
            { "exit", parameters =>
                {
                    Log.Info("Exiting...");
                }
            }
        };

        private void Start(string process)
        {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = process,
                        UseShellExecute = true
                    };

                    Process.Start(psi);

                    Log.Success($"Program '{process}' started.");
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    Log.Failure($"Program '{process}' not found.");
                }
                catch (Exception ex)
                {
                    Log.Failure($"An error occurred: {ex.Message}");
                }
        }

        private void Stop(string process)
        {
            try
            {
                Process[] processes = Process.GetProcessesByName(process);

                if (processes.Length == 0)
                {
                    Log.Failure($"No process found with name '{process}'.");
                    return;
                }

                foreach (Process p in processes)
                {
                    p.CloseMainWindow();

                    if (!p.WaitForExit(5000))
                    {
                        Log.Warn($"Process '{process}' did not exit in time. Killing process...");
                        p.Kill();
                    }

                    Log.Success($"Process '{process}' stopped.");
                }
            }
            catch (System.ComponentModel.Win32Exception)
            {
                Log.Failure($"Program '{process}' not found.");
            }
            catch (Exception ex)
            {
                Log.Failure($"An error occurred: {ex.Message}");
            }
        }
    }
}
