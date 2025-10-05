using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Structural.Flyweight;

public class FlyweightPatternDemo : IPatternDemo
{
    public string PatternName => "Flyweight";
    public string Description => "Uses sharing to efficiently support large numbers of similar objects.";

    public void Demonstrate()
    {
        Console.WriteLine("🏞️ Tree Forest Flyweight Example");
        
        var forest = new Forest();
        
        // Plant many trees - flyweight pattern saves memory
        for (int i = 0; i < 1000; i++)
        {
            forest.PlantTree(
                Random.Shared.Next(0, 100), 
                Random.Shared.Next(0, 100),
                "Oak", "Green", "tree.png"
            );
        }
        
        forest.Paint();
        
        Console.WriteLine($"🌳 Created {forest.TreeCount} trees");
        Console.WriteLine($"🎯 TreeType flyweights created: {TreeTypeFactory.CreatedTypes}");
    }
}

// Flyweight
public class TreeType
{
    private readonly string name;
    private readonly string color;
    private readonly string sprite;
    
    public TreeType(string name, string color, string sprite)
    {
        this.name = name;
        this.color = color; 
        this.sprite = sprite;
    }
    
    public void Render(int x, int y)
    {
        Console.WriteLine($"🌳 Rendering {color} {name} at ({x}, {y})");
    }
}

// Flyweight Factory
public static class TreeTypeFactory
{
    private static readonly Dictionary<string, TreeType> treeTypes = new();
    public static int CreatedTypes => treeTypes.Count;
    
    public static TreeType GetTreeType(string name, string color, string sprite)
    {
        var key = $"{name}-{color}-{sprite}";
        if (!treeTypes.ContainsKey(key))
        {
            treeTypes[key] = new TreeType(name, color, sprite);
        }
        return treeTypes[key];
    }
}

// Context
public class Tree
{
    private readonly int x, y;
    private readonly TreeType type;
    
    public Tree(int x, int y, TreeType type)
    {
        this.x = x;
        this.y = y;
        this.type = type;
    }
    
    public void Paint() => type.Render(x, y);
}

// Client
public class Forest
{
    private readonly List<Tree> trees = new();
    public int TreeCount => trees.Count;
    
    public void PlantTree(int x, int y, string name, string color, string sprite)
    {
        var type = TreeTypeFactory.GetTreeType(name, color, sprite);
        trees.Add(new Tree(x, y, type));
    }
    
    public void Paint()
    {
        Console.WriteLine("🎨 Painting forest (showing first 5 trees):");
        trees.Take(5).ToList().ForEach(tree => tree.Paint());
        if (trees.Count > 5)
            Console.WriteLine($"... and {trees.Count - 5} more trees");
    }
}