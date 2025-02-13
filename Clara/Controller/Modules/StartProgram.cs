using System.Diagnostics;
using Clara.Core;

namespace Clara.Modules
{
    public class StartProgram : Module
    {
        protected override void Enter()
        {
            
        }

        protected override void Exit()
        {
            
        }

        protected override void Process()
        {
            if (args.Length == 0)
            {
                Log.Warn("Arguments missing.");
                Log.Info("Usage: start-program <program>");
                return;
            }
            else if (args.Length > 1)
            {
                Log.Warn("Too many arguments.");
                Log.Info("Usage: start-program <program>");
                return;
            }
            else
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = args[0],
                        UseShellExecute = true
                    };

                    System.Diagnostics.Process.Start(psi);

                    Log.Success($"Program '{args[0]}' started.");
                }
                catch (System.ComponentModel.Win32Exception)
                {
                    Log.Failure($"Program '{args[0]}' not found.");
                }
                catch (Exception ex)
                {
                    Log.Failure($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
