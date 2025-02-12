using Clara.Core;
using Clara.Utils;

namespace Clara.Modules
{
    public static class MultiTask
    {
        private static List<Tasks> _multiTasks = new List<Tasks>();

        public static void Run(string[] args)
        {
            ReadMultiTasks();
            WriteMultiTasks();

            if (args.Length == 0)
            {
                Process();
            }
            else
            {
                switch (args[0])
                {
                    case "list":
                        List();
                        break;

                    case "add":
                        Add(args[1], args[2..].ToList());
                        break;

                    case "remove":
                        Remove(args[1]);
                        break;

                    default:
                        Log.Error("Invalid argument!");
                        break;
                }
            }
        }

        private static void ReadMultiTasks()
        {
            List<Tasks>? tmp = Config.Get<List<Tasks>>("MultiTasks");

            if (tmp != null && tmp != default)
            {
                _multiTasks = tmp;
            }
        }

        private static void WriteMultiTasks()
        {
            Config.Set("MultiTasks", _multiTasks);
        }

        private static void Process()
        {
            foreach (Tasks tasks in _multiTasks)
            {
                Log.Header($"Running task: {tasks.Name}");

                foreach (string command in tasks.Commands)
                {
                    (string task, string[] args) = Input.ParseCommand(command);

                    TaskProcessor.RunTask(task, args);
                }
            }
        }

        private static void List()
        {
            if (_multiTasks.Count == 0)
            {
                Log.Info("No multi-tasks found.");
                return;
            }

            Log.Header("Multi-Tasks");

            foreach (Tasks tasks in _multiTasks)
            {
                Log.Info(tasks.Name);
            }
        }

        private static void Add(string name, List<string> commands)
        {
            _multiTasks.Add(new Tasks
            {
                Name = name,
                Commands = commands
            });

            WriteMultiTasks();
        }

        private static void Remove(string name)
        {
            _multiTasks.RemoveAll(x => x.Name == name);

            WriteMultiTasks();
        }

        [Serializable]
        private struct Tasks
        {
            public string Name { get; set; }
            public List<string> Commands { get; set; }
        }
    }
}
