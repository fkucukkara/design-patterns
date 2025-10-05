using System.Reflection;

namespace DesignPatterns.Infrastructure;

/// <summary>
/// Manages the discovery and execution of design pattern demonstrations.
/// Uses reflection to find all pattern demos and provides an interactive menu.
/// </summary>
public class PatternMenuManager
{
    private readonly List<IPatternDemo> _patterns;

    public PatternMenuManager()
    {
        _patterns = DiscoverPatterns();
    }

    /// <summary>
    /// Starts the interactive menu for pattern demonstrations.
    /// </summary>
    public void Run()
    {
        Console.Clear();
        Console.WriteLine("Design Patterns Demo - 23 GoF Patterns");
        Console.WriteLine("======================================");

        while (true)
        {
            DisplayMainMenu();
            
            var choice = Console.ReadLine()?.Trim();
            
            if (string.IsNullOrEmpty(choice))
                continue;

            if (choice.Equals("q", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting...");
                break;
            }

            HandleMenuChoice(choice);
        }
    }

    private void DisplayMainMenu()
    {
        Console.WriteLine("\nMain Menu:");
        Console.WriteLine("1. Creational Patterns");
        Console.WriteLine("2. Structural Patterns");
        Console.WriteLine("3. Behavioral Patterns");
        Console.WriteLine("4. Show All Patterns");
        Console.WriteLine("Q. Quit");
        Console.Write("Select: ");
    }

    private void HandleMenuChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                ShowPatternCategory("Creational");
                break;
            case "2":
                ShowPatternCategory("Structural");
                break;
            case "3":
                ShowPatternCategory("Behavioral");
                break;
            case "4":
                ShowAllPatterns();
                break;
            default:
                Console.WriteLine("Invalid option. Try again.");
                break;
        }
    }

    private void ShowPatternCategory(string category)
    {
        var categoryPatterns = _patterns.Where(p => p.GetType().Namespace?.Contains(category) == true).ToList();
        
        if (!categoryPatterns.Any())
        {
            Console.WriteLine($"No patterns found for category: {category}");
            return;
        }

        Console.Clear();
        Console.WriteLine($"{category} Patterns:");

        for (int i = 0; i < categoryPatterns.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {categoryPatterns[i].PatternName}");
        }

        Console.WriteLine("0. Back to Main Menu");
        Console.Write("Select a pattern: ");

        var choice = Console.ReadLine()?.Trim();
        
        if (choice == "0")
            return;

        if (int.TryParse(choice, out int index) && index > 0 && index <= categoryPatterns.Count)
        {
            DemonstratePattern(categoryPatterns[index - 1]);
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void ShowAllPatterns()
    {
        Console.Clear();
        Console.WriteLine("All Available Patterns:");

        var groupedPatterns = _patterns
            .GroupBy(p => GetPatternCategory(p))
            .OrderBy(g => g.Key);

        foreach (var group in groupedPatterns)
        {
            Console.WriteLine($"\n{group.Key}:");
            foreach (var pattern in group.OrderBy(p => p.PatternName))
            {
                Console.WriteLine($"  - {pattern.PatternName}");
            }
        }

        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }

    private void DemonstratePattern(IPatternDemo pattern)
    {
        Console.Clear();
        Console.WriteLine($"{pattern.PatternName} Pattern");
        Console.WriteLine(new string('-', pattern.PatternName.Length + 8));
        Console.WriteLine($"Description: {pattern.Description}\n");

        try
        {
            pattern.Demonstrate();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during demonstration: {ex.Message}");
        }
    }

    private List<IPatternDemo> DiscoverPatterns()
    {
        var patterns = new List<IPatternDemo>();
        var assembly = Assembly.GetExecutingAssembly();

        var patternTypes = assembly.GetTypes()
            .Where(t => typeof(IPatternDemo).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var type in patternTypes)
        {
            try
            {
                if (Activator.CreateInstance(type) is IPatternDemo instance)
                {
                    patterns.Add(instance);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not create instance of {type.Name}: {ex.Message}");
            }
        }

        return patterns.OrderBy(p => p.PatternName).ToList();
    }

    private static string GetPatternCategory(IPatternDemo pattern)
    {
        var namespaceParts = pattern.GetType().Namespace?.Split('.') ?? Array.Empty<string>();
        return namespaceParts.Length > 2 ? namespaceParts[2] : "Unknown";
    }
}