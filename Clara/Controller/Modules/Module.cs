using Clara.Core;

namespace Clara.Modules
{
    public abstract class Module
    {
        protected string[] args = [];

        public virtual void Run(string[] args)
        {
            this.args = args;
            Console.Clear();

            Log.Header(GetType().Name);

            Enter();
            Process();
            Exit();
        }

        protected abstract void Enter();
        protected abstract void Exit();
        protected abstract void Process();
    }
}
