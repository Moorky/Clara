using Clara.Utils;

namespace Clara.Core
{
    public abstract class Module
    {
        protected string[] args = [];
        protected abstract string[] _menuCommands { get; }
        protected abstract Dictionary<string, Action<string[]>> _commandHandlers { get; }

        public virtual void Run(string[] args)
        {
            this.args = args;

            Console.Clear();
            if (args == null || args.Length == 0)
            {
                Menu();
            }
            Log.Header("START " + GetType().Name);

            Enter();
            if (this.args.Length > 0)
            {
                Execute();
            }
            Exit();

            Log.Info($"Executed: '{GetType().Name} {string.Join(" ", args)}'");
            Log.Header("END " + GetType().Name);
        }

        protected abstract void Menu();

        protected abstract void Enter();

        protected virtual void Execute()
        {
            string command = args[0].ToLower();
            if (_commandHandlers.ContainsKey(command))
            {
                _commandHandlers[command](args.Skip(1).ToArray());
            }
            else
            {
                Log.Error($"Unknown command: '{command}'");
            }
        }

        protected abstract void Exit();
    }
}
