namespace Clara.Core
{
    public static class Main
    {
        private static void Enter()
        {
            Session.Start();
        }

        private static void Process()
        {
            Controller.Run();
        }

        private static void Exit()
        {
            Session.Stop();
        }

        public static void Run()
        {
            Enter();
            Process();
            Exit();
        }
    }
}
