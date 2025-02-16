using Clara.Utils;

namespace Clara.Core
{
    public abstract class Module
    {
        protected List<string> args = [];

        protected abstract Dictionary<string, Action> _menuHandlers { get; }

        protected abstract Dictionary<string, Action<string[]>> _argHandlers { get; }

        public virtual void Run(string[] args)
        {
            this.args = args.ToList();

            Console.Clear();
            if (args == null || args.Length == 0)
            {
                GetArgs();
            }

            Console.Clear();
            Log.Info("Command: \"" + (GetType().Name + " " + string.Join(" ", this.args)).Trim() + "\"");
            Log.Header("START " + GetType().Name);

            Enter();
            if (this.args.Count > 0)
            {
                Execute();
            }
            Exit();

            Log.Header("END " + GetType().Name);
        }

        protected virtual void GetArgs()
        {
            string command = _menuHandlers.Keys.ToArray()[Menu.Run(GetType().Name, _menuHandlers.Keys.ToArray())];

            args.Add(command);
            _menuHandlers[command]();
        }

        protected virtual void Enter() { }

        protected virtual void Execute()
        {
            string command = args[0].ToLower();
            if (_argHandlers.ContainsKey(command))
            {
                _argHandlers[command](args.Skip(1).ToArray());
            }
            else
            {
                Log.Error($"Unknown command: '{command}'");
            }
        }

        protected virtual void Exit() { }
    }
}
