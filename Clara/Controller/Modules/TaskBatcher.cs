using Clara.Core;
using Clara.Utils;

namespace Clara.Modules
{
    public class TaskBatcher : Module
    {
        private List<Batch> _batches = [];
        private readonly string[] _commands = ["Run", "Add", "Remove", "List", "Exit"];

        protected override void Enter()
        {
            ReadConfig();
        }

        protected override void Exit()
        {
            WriteConfig();
        }

        protected override void Process()
        {
            if (args.Length == 0)
            {
                Menu();
            }
            else
            {
                ProcessCommand();
            }
        }

        private void Menu()
        {
            switch (_commands[Utils.Menu.Run(typeof(TaskBatcher).Name, _commands)])
            {
                case "Run":
                    Log.Header("Run Batch");
                    Run(Input.Get("Name"));
                    break;

                case "Add":
                    Log.Header("Add Batch");
                    string name = Input.Get("Name");
                    List<string> commands = [];
                    do { commands.Add(Input.Get("Command")); } while (Utils.Menu.Run("Add another command?", ["Yes","No"]) == 0);
                    Add(name, commands);
                    break;

                case "Remove":
                    Log.Header("Remove Batch");
                    Remove(Input.Get("Name"));
                    break;

                case "List":
                    Log.Header("List of all Batches");
                    List();
                    break;

                case "Exit":
                    Log.Header("Exit");
                    break;

                default:
                    Log.Error("Invalid command!");
                    break;
            }
        }

        private void ProcessCommand()
        {
            string command = args[0].ToLower();

            switch (command)
            {
                case "run":
                    Run(args[1]);
                    break;

                case "add":
                    Add(args[1], args[2..].ToList());
                    break;

                case "remove":
                    Remove(args[1]);
                    break;

                case "list":
                    List();
                    break;

                default:
                    Log.Error("Invalid command!");
                    break;
            }
        }

        private void ReadConfig()
        {
            List<Batch>? tmp = Config.Get<List<Batch>>("TaskBatcher");

            if (tmp != null && tmp != default)
            {
                _batches = tmp;
            }
        }

        private void WriteConfig()
        {
            Config.Set("TaskBatcher", _batches);
        }

        private void Run(string name)
        {
            Batch? batch = _batches.Find(x => x.Name == name);

            if (batch == null)
            {
                Log.Error("Batch not found.");
                return;
            }

            Log.Header("Running Batch: " + name);

            foreach (string command in batch?.Commands)
            {
                Log.Info("Running command: " + command);

                (string module, string[] args) = Input.ParseCommand(command);
                Controller.RunModule(module, args);
            }
        }

        private void List()
        {
            if (_batches.Count == 0)
            {
                Log.Info("No Batch found.");
                return;
            }

            foreach (Batch tasks in _batches)
            {
                Log.Info(tasks.Name);
            }
        }

        private void Add(string name, List<string> commands)
        {
            _batches.Add(new Batch
            {
                Name = name,
                Commands = commands
            });

            WriteConfig();
        }

        private void Remove(string name)
        {
            int count = _batches.RemoveAll(x => x.Name == name);

            if (count == 0)
            {
                Log.Error("Batch not found.");
            }
            else
            {
                Log.Success($"Batch {name} removed. Occurences: {count}");
            }

            WriteConfig();
        }

        [Serializable]
        private struct Batch
        {
            public string Name { get; set; }
            public List<string> Commands { get; set; }
        }
    }
}
