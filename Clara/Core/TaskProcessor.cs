using Clara.Modules;

namespace Clara.Core
{
    public static class TaskProcessor
    {
        private static void Enter()
        {
            AutoTasks.Run();
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
            string task = input.Contains(" ") ? input.Split(" ")[0] : input;
            string[] args = input.Contains(" ") ? input.Split(" ")[1..] : new string[0];

            bool result = RunTask(task, args);

            return result;
        }

        public static bool RunTask(string task, string[] args)
        {
            switch (task)
            {
                case "auto-tasks":
                    AutoTasks.Run(args);
                    break;

                case "start-program":
                    StartProgram.Run(args);
                    break;

                case "help":
                case "h":
                    Help.Run(args);
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
