using Clara.Core;
using Clara.Modules;
using Clara.Utils;

namespace Clara
{
    public static class Controller
    {
        private static readonly string[] _module = ["StartProgram", "Autostart", "TaskBatcher", "Exit"];

        private static void Enter()
        {

        }

        private static void Exit()
        {
            
        }

        private static void Process()
        {
            bool run = true;

            while (run)
            {
                run = RunModule(_module[Menu.Run(typeof(Controller).Name, _module)], []);

                Input.PressAnyKeyToContinue();
            }
        }

        public static void Run()
        {
            Enter();
            Process();
            Exit();
        }

        public static bool RunModule(string module, string[] args)
        {
            switch (module.ToLower())
            {
                case "startprogram":
                    new StartProgram().Run(args);
                    break;

                case "autostart":
                    new AutoStart().Run(args);
                    break;

                case "taskbatcher":
                    new TaskBatcher().Run(args);
                    break;

                case "exit":
                    return false;

                default:
                    Log.Error("Invalid module!");
                    break;
            }

            return true;
        }
    }
}
