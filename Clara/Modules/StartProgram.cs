using System.Diagnostics;
using Clara.Core;
using Clara.Utils;

namespace Clara.Modules
{
    public class StartProgram : Module
    {
        protected override string[] _menuCommands => throw new NotImplementedException();

        protected override Dictionary<string, Action<string[]>> _commandHandlers => throw new NotImplementedException();

        protected override void Enter()
        {
            throw new NotImplementedException();
        }

        protected override void Exit()
        {
            throw new NotImplementedException();
        }

        protected override void Execute()
        {
            if (args.Count == 0)
            {
                Log.Warn("Arguments missing.");
                Log.Info("Usage: start-program <program>");
                return;
            }
            else if (args.Count > 1)
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
