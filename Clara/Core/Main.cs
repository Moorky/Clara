using Clara.Core;

namespace Clara
{
    public static class Main
    {
        private static void Enter()
        {
            Session.Start();
        }

        private static void Exit()
        {
            Session.Stop();
        }

        private static void Process()
        {
            Controller.Run();
        }

        public static void Run()
        {
            Enter();
            Process();
            Exit();
        }
    }
}
