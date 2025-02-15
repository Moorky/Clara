using Clara.Core;
using Clara.Utils;
using System.Xml.Linq;

namespace Clara.Modules
{
    public class TriggerManager : Module
    {
        private List<Trigger> _triggers = [];

        protected override Dictionary<string, Action> _menuHandlers => throw new NotImplementedException();

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
                    AddTrigger(new Trigger { Name = parameters[0] });
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
                    Log.Header("Run Trigger");
                    if (parameters.Length < 1)
                    {
                        Log.Error("Missing trigger name.");
                        return;
                    }
                    RunTrigger(new Trigger { Name = parameters[0] });
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
            List<Trigger>? tmp = Config.Get<List<Trigger>>("TaskBatcher");
            if (tmp != null)
            {
                _triggers = tmp;
            }
        }

        private void WriteConfig()
        {
            Config.Set("TaskBatcher", _triggers);
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

        private void RunTrigger(Trigger trigger)
        {
            if (!TriggerExists(trigger.Name))
            {
                Log.Error($"Trigger '{trigger.Name}' does not exist.");
                return;
            }

            Log.Header("Running Trigger: " + trigger.Name);
            foreach (string action in trigger.Actions)
            {
                Log.Info("Running Action: " + action);
                (string module, string[] args) = Input.ParseCommand(action);

                try
                {
                    Controller.RunModule(module, args);
                }
                catch (Exception ex)
                {
                    Log.Fatal($"Error running action: {action}");
                    Log.Fatal(ex.Message);
                }
            }
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
            Time
        }
    }
}
