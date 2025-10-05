using DesignPatterns.Infrastructure;

try
{
    var menuManager = new PatternMenuManager();
    menuManager.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Application error: {ex.Message}");
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
