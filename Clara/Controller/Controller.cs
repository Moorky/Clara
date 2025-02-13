using Clara.Core;
using Clara.Modules;
using Clara.Utils;

namespace Clara
{
    public static class Controller
    {
        private static string[] _task = new string[] { "autostart", "multitasks", "exit" };

        private static void Enter()
        {
            MultiTasks.Run(Array.Empty<string>());
        }

        private static void Process()
        {
            int index;

            do { index = Menu.Run(_task); }
            while (RunTask(_task[index], Array.Empty<string>()));
        }

        private static void Exit()
        {
            Log.Info("Goodbye!");
        }

        public static void Run()
        {
            Enter();
            Process();
            Exit();
        }

        public static bool RunTask(string task, string[] args)
        {
            switch (task)
            {
                case "autostart":
                    AutoStart.Run(args);
                    break;

                case "multitasks":
                    MultiTasks.Run(args);
                    break;

                case "exit":
                    return false;

                default:
                    Log.Error("Invalid task!");
                    break;
            }

            Input.PressAnyKeyToContinue();
            Console.Clear();

            return true;
        }
    }
}
