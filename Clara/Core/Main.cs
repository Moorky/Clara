using Clara.Core;
using Clara.Utils;

namespace Clara
{
    public static class Main
    {
        public static void Run()
        {
            Session.Start();
            Controller.Run();
            Session.Stop();
        }
    }
}
