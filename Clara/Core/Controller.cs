namespace Clara.Core
{
    public static class Controller
    {
        public static void Start()
        {
            Session.Start();

            TaskProcessor.Run();
        }

        public static void Stop()
        {
            Session.Stop();
        }
    }
}
