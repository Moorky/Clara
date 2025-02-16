using Clara.Core;
using Clara.Utils;

namespace Clara.Modules
{
    public class ActionTrigger : Module
    {
        private List<Trigger> _triggers = [];

        protected override Dictionary<string, Action> _menuHandlers => new Dictionary<string, Action>
        {
            { "Add", () => 
                {
                    args.Add(Input.Get("Trigger Name"));
                    args.Add(Menu.Run("Trigger Type", Enum.GetNames(typeof(TriggerType))).ToString());
                    args.Add(Input.Get("Condition"));
                    List<string> actions = new List<string>();
                    while (Menu.Run("Add Action?", ["Yes", "No"]) == 0)
                    {
                        actions.Add(Input.Get("Action"));
                    }
                    args.AddRange(actions);
                }
            },
            { "Remove", () => args.Add(Input.Get("Name")) },
            { "Run", () => { } },
            { "List", () => { } },
            { "Exit", () => { } }
        };

        protected override Dictionary<string, Action<string[]>> _argHandlers => new Dictionary<string, Action<string[]>>()
        {
            { "add", parameters =>
                {
                    Log.Header("Add Trigger");
                    if (parameters.Length < 1)
                    {
                        Log.Error("Missing trigger name.");
                        return;
                    }
                    AddTrigger(new Trigger { 
                        Name = parameters[0],
                        Type = (TriggerType)Enum.Parse(typeof(TriggerType), parameters[1]),
                        Condition = parameters[2],
                        Actions = parameters.Skip(3).ToArray() 
                    });
                }
            },
            { "remove", parameters =>
                {
                    Log.Header("Remove Trigger");
                    if (parameters.Length < 1)
                    {
                        Log.Error("Missing trigger name.");
                        return;
                    }
                    RemoveTrigger(new Trigger { Name = parameters[0] });
                }
            },
            { "run", parameters =>
                {
                    Log.Header("Triggering...");
                    RunTriggers();
                }
            },
            { "list", parameters =>
                {
                    Log.Header("List Triggers");
                    ListTriggers();
                }
            },
            { "exit", parameters =>
                {
                    Log.Info("Exiting...");
                }
            }
        };

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
            List<Trigger>? tmp = Config.Get<List<Trigger>>(GetType().Name);
            if (tmp != null)
            {
                _triggers = tmp;
            }
        }

        private void WriteConfig()
        {
            Config.Set(GetType().Name, _triggers);
        }

        private void AddTrigger(Trigger trigger)
        {
            if (TriggerExists(trigger.Name))
            {
                Log.Error($"Trigger '{trigger.Name}' already exists.");
                return;
            }

            _triggers.Add(trigger);
        }

        private void RemoveTrigger(Trigger trigger)
        {
            if (!TriggerExists(trigger.Name))
            {
                Log.Error($"Trigger '{trigger.Name}' does not exist.");
                return;
            }

            _triggers.Remove(_triggers.First(x => x.Name.Equals(trigger.Name, StringComparison.OrdinalIgnoreCase)));
        }

        private void RunTriggers()
        {
            foreach (var trigger in _triggers)
            {
                if (trigger.Type == TriggerType.Always)
                {
                    Run(trigger);
                }
                else if (trigger.Type == TriggerType.Time)
                {
                    if (TimeCondition(trigger.Condition))
                    {
                        Run(trigger);
                    }
                }
            }
        }

        private void Run(Trigger trigger)
        {
            Log.Header($"Running trigger '{trigger.Name}'. Type: {trigger.Type}");
            foreach (var action in trigger.Actions)
            {
                Log.Info($"Running action: {action}");
                (string module, string[] args) = Input.ParseCommand(action);

                try
                {
                    Controller.RunModule(module, args);
                }
                catch (Exception ex)
                {
                    Log.Failure($"Error running action: {action}");
                    Log.Failure(ex.Message);
                }
            }
        }

        private bool TimeCondition(string condition)
        {
            return true;
        }

        private void ListTriggers()
        {
            if (_triggers.Count == 0)
            {
                Log.Failure("No triggers found.");
                return;
            }

            Log.Header("List of all Triggers");
            Console.WriteLine();

            foreach (var trigger in _triggers)
            {
                Log.Header($"Trigger: {trigger.Name}");
                Log.Info($"  Type: {trigger.Type}");
                Log.Info($"  Condition: {trigger.Condition}");
                foreach (var action in trigger.Actions)
                {
                    Log.Info($"  Action: {action}");
                }

                Console.WriteLine();
            }
        }

        private bool TriggerExists(string name)
        {
            return _triggers.Exists(x => x.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        [Serializable]
        private struct Trigger
        {
            public string Name { get; set; }
            public TriggerType Type { get; set; }
            public string Condition { get; set; }
            public string[] Actions { get; set; }
        }

        [Serializable]
        public enum TriggerType
        {
            Always,
            Time
        }
    }
}
