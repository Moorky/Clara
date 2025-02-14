internal class Program
{
    private static void Main(string[] args)
    {
        try
        {
            Clara.Core.Controller.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}