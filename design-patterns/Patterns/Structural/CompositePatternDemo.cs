using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Structural.Composite;

public class CompositePatternDemo : IPatternDemo
{
    public string PatternName => "Composite";
    public string Description => "Composes objects into tree structures to represent part-whole hierarchies.";

    public void Demonstrate()
    {
        Console.WriteLine("🌳 File System Composite Example");
        
        var root = new Folder("Root");
        var docs = new Folder("Documents");
        var pics = new Folder("Pictures");
        
        root.Add(docs);
        root.Add(pics);
        root.Add(new File("readme.txt", 5));
        
        docs.Add(new File("report.pdf", 100));
        docs.Add(new File("presentation.pptx", 200));
        
        pics.Add(new File("photo1.jpg", 50));
        pics.Add(new File("photo2.png", 75));
        
        Console.WriteLine($"Total size: {root.GetSize()} KB");
        root.Display(0);
    }
}

// Component
public abstract class FileSystemItem
{
    protected string name;
    
    public FileSystemItem(string name) => this.name = name;
    
    public abstract int GetSize();
    public abstract void Display(int depth);
}

// Leaf
public class File : FileSystemItem
{
    private int size;
    
    public File(string name, int size) : base(name) => this.size = size;
    
    public override int GetSize() => size;
    public override void Display(int depth) => 
        Console.WriteLine($"{new string(' ', depth * 2)}📄 {name} ({size} KB)");
}

// Composite
public class Folder : FileSystemItem
{
    private List<FileSystemItem> items = new();
    
    public Folder(string name) : base(name) { }
    
    public void Add(FileSystemItem item) => items.Add(item);
    public void Remove(FileSystemItem item) => items.Remove(item);
    
    public override int GetSize() => items.Sum(item => item.GetSize());
    
    public override void Display(int depth)
    {
        Console.WriteLine($"{new string(' ', depth * 2)}📁 {name}/");
        foreach (var item in items)
            item.Display(depth + 1);
    }
}