#region Imports
using Clara.Core;
#endregion


#region Main
try
{
    Controller.Start();
    Controller.Stop();
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}
#endregion

#region Exit
Console.WriteLine();
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
#endregion