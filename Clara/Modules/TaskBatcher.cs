using Clara.Core;
using Clara.Utils;

namespace Clara.Modules
{
    public class TaskBatcher : Module
    {
        private List<Batch> _batches = new List<Batch>();

        protected override string[] _menuCommands => ["Run", "Add", "Remove", "List", "Exit"];

        protected override Dictionary<string, Action<string[]>> _commandHandlers => new Dictionary<string, Action<string[]>>
        {
            { "run", parameters =>
                {
                    Log.Header("Run Batch");
                    if (parameters.Length < 1)
                    {
                        Log.Error("Missing batch name.");
                        return;
                    }
                    RunBatch(parameters[0]);
                }
            },
            { "add", parameters =>
                {
                    Log.Header("Add Batch");
                    if (parameters.Length < 1)
                    {
                        Log.Error("Missing batch name.");
                        return;
                    }
                    AddBatch(parameters[0], parameters.Skip(1).ToList());
                }
            },
            { "remove", parameters =>
                {
                    Log.Header("Remove Batch");
                    if (parameters.Length < 1)
                    {
                        Log.Error("Missing batch name.");
                        return;
                    }
                    RemoveBatch(parameters[0]);
                }
            },
            { "list", parameters =>
                {
                    Log.Header("List of all Batches");
                    ListBatches();
                }
            }
        };

        protected override void Menu()
        {
            List<string> argsList = new List<string>();

            int selected = Utils.Menu.Run(GetType().Name, _menuCommands);
            string chosenCommand = _menuCommands[selected];

            switch (chosenCommand)
            {
                case "Run":
                    argsList.Add("run");
                    argsList.Add(Input.Get("Name"));
                    break;

                case "Add":
                    argsList.Add("add");
                    argsList.Add(Input.Get("Name"));
                    List<string> commands = new List<string>();
                    while (Utils.Menu.Run("Add another command?", ["Yes", "No"]) == 0)
                    {
                        commands.Add(Input.Get("Command"));
                    }
                    argsList.AddRange(commands);
                    break;

                case "Remove":
                    argsList.Add("remove");
                    argsList.Add(Input.Get("Name"));
                    break;

                case "List":
                    argsList.Add("list");
                    break;

                case "Exit":
                    return;
            }

            args = argsList.ToArray();
        }

        protected override void Enter()
        {
            ReadConfig();
        }

        protected override void Exit()
        {
            WriteConfig();
        }

        private void ReadConfig()
        {
            List<Batch>? tmp = Config.Get<List<Batch>>("TaskBatcher");
            if (tmp != null)
            {
                _batches = tmp;
            }
        }

        private void WriteConfig()
        {
            Config.Set("TaskBatcher", _batches);
        }

        private void RunBatch(string name)
        {
            Batch batch = _batches.Find(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(batch.Name))
            {
                Log.Error("Batch not found.");
                return;
            }

            Log.Header("Running Batch: " + name);
            foreach (string command in batch.Commands)
            {
                Log.Info("Running command: " + command);
                (string module, string[] args) = Input.ParseCommand(command);
                Controller.RunModule(module, args);
            }
        }

        private void ListBatches()
        {
            if (_batches.Count == 0)
            {
                Log.Info("No Batch found.");
                return;
            }

            foreach (Batch batch in _batches)
            {
                Log.Info(batch.Name);
            }
        }

        private void AddBatch(string name, List<string> commands)
        {
            if (_batches.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Log.Error("Batch already exists.");
                return;
            }

            _batches.Add(new Batch
            {
                Name = name,
                Commands = commands
            });
        }

        private void RemoveBatch(string name)
        {
            int count = _batches.RemoveAll(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (count == 0)
            {
                Log.Error("Batch not found.");
            }
            else
            {
                Log.Success($"Batch {name} removed. Occurrences: {count}");
            }
        }

        [Serializable]
        private struct Batch
        {
            public string Name { get; set; }
            public List<string> Commands { get; set; }
        }
    }
}
