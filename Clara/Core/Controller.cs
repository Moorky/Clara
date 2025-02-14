using Clara.Utils;
using System.Reflection;

namespace Clara.Core
{
    public static class Controller
    {
        private static Dictionary<string, Func<string[], bool>> ModuleActions { get; } = LoadModules();

        public static Dictionary<string, Func<string[], bool>> LoadModules()
        {
            var modules = new Dictionary<string, Func<string[], bool>>(StringComparer.OrdinalIgnoreCase);

            Assembly assembly = typeof(Module).Assembly;
            if (assembly == null)
            {
                Log.Error("Assembly containing the Module type could not be loaded.");
                return modules;
            }

            var moduleTypes = assembly.GetTypes()
                                      .Where(t => t.IsClass && !t.IsAbstract && typeof(Module).IsAssignableFrom(t));

            foreach (var type in moduleTypes)
            {
                string moduleName = type.Name;
                modules[moduleName] = args =>
                {
                    if (Activator.CreateInstance(type) is not Module instance)
                    {
                        Log.Error($"Failed to create an instance of module: {moduleName}");
                        return true;
                    }

                    instance.Run(args);
                    return true;
                };
            }

            modules["Exit"] = args => false;

            return modules;
        }

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
            bool running = true;
            while (running)
            {
                string[] modules = ModuleActions.Keys.ToArray();
                int selectedIndex = Menu.Run(nameof(Controller), modules);
                running = RunModule(modules[selectedIndex], []);

                if (running)
                    Input.PressAnyKeyToContinue();
            }
        }

        public static bool RunModule(string module, string[] args)
        {
            if (!ModuleActions.TryGetValue(module, out var runModule))
            {
                Log.Error("Invalid module!");
                return true;
            }

            try
            {
                return runModule(args);
            }
            catch (Exception ex)
            {
                Log.Fatal($"Error running module: {module}");
                Log.Fatal(ex.Message);
                return true;
            }
        }

        public static void Run()
        {
            Enter();
            Process();
            Exit();
        }
    }
}
