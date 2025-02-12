using Clara.Modules;
using Clara.Utils;

namespace Clara.Core
{
    public static class TaskProcessor
    {
        private static void Enter()
        {
            MultiTask.Run(Array.Empty<string>());
        }

        public static void Run()
        {
            Enter();

            while (Process()) ;

            Exit();
        }

        private static bool Process()
        {
            string input = User.Input("Command").ToLower();

            (string task, string[] args) = Input.ParseCommand(input);

            return RunTask(task, args);
        }

        public static bool RunTask(string task, string[] args)
        {
            switch (task)
            {
                case "auto-start":
                    AutoStart.Run(args);
                    break;

                case "multi-task":
                    MultiTask.Run(args);
                    break;

                case "start-program":
                    StartProgram.Run(args);
                    break;

                case "help":
                case "h":
                    Help.Run(args);
                    break;

                case "hi":
                case "hello":
                case "hey":
                    Log.Clara("Hello!");
                    break;

                case "exit":
                case "quit":
                case "q":
                    return false;

                default:
                    Log.Warn("Invalid command.");
                    Log.Info("Type 'help' for a list of commands.");
                    break;
            }

            return true;
        }

        private static void Exit()
        {

        }
    }
}
