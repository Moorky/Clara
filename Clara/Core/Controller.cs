using System;
using System.Collections.Generic;
using Clara.Modules;
using Clara.Utils;

namespace Clara.Core
{
    public static class Controller
    {
        private static readonly string[] Modules = new[] { "StartProgram", "Autostart", "TaskBatcher", "Exit" };

        private static readonly Dictionary<string, Func<string[], bool>> ModuleActions = new Dictionary<string, Func<string[], bool>>(StringComparer.OrdinalIgnoreCase)
        {
            { "StartProgram", args => { new StartProgram().Run(args); return true; } },
            { "Autostart", args => { new AutoStart().Run(args); return true; } },
            { "TaskBatcher", args => { new TaskBatcher().Run(args); return true; } },
            { "Exit", args => false }
        };

        private static void Enter()
        {
            Session.Start();
        }

        private static void Exit()
        {
            Session.Stop();
        }

        private static void Process()
        {
            bool running = true;
            while (running)
            {
                int selectedIndex = Menu.Run(nameof(Controller), Modules);
                running = RunModule(Modules[selectedIndex], []);

                if (running)
                    Input.PressAnyKeyToContinue();
            }
        }

        public static bool RunModule(string module, string[] args)
        {
            if (!ModuleActions.TryGetValue(module, out var runModule))
            {
                Log.Error("Invalid module!");
                return true;
            }

            return runModule(args);
        }

        public static void Run()
        {
            Enter();
            Process();
            Exit();
        }
    }
}
