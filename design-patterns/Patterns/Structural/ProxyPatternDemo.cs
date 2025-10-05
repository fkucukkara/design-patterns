using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Structural.Proxy;

public class ProxyPatternDemo : IPatternDemo
{
    public string PatternName => "Proxy";
    public string Description => "Provides a placeholder or surrogate to control access to another object.";

    public void Demonstrate()
    {
        Console.WriteLine("🖼️ Image Proxy Example");
        
        var images = new List<IImage>
        {
            new ImageProxy("photo1.jpg"),
            new ImageProxy("photo2.jpg"),
            new ImageProxy("photo3.jpg")
        };
        
        Console.WriteLine("📋 Images created (not loaded yet)");
        
        // Images are loaded only when accessed
        Console.WriteLine("\n🎨 Displaying first image:");
        images[0].Display();
        
        Console.WriteLine("\n🎨 Displaying first image again (cached):");
        images[0].Display();
        
        Console.WriteLine("\n🎨 Displaying second image:");
        images[1].Display();
    }
}

// Subject interface
public interface IImage
{
    void Display();
}

// Real Subject
public class RealImage : IImage
{
    private readonly string filename;
    
    public RealImage(string filename)
    {
        this.filename = filename;
        LoadFromDisk();
    }
    
    private void LoadFromDisk()
    {
        Console.WriteLine($"💾 Loading {filename} from disk...");
        Thread.Sleep(1000); // Simulate expensive loading
    }
    
    public void Display()
    {
        Console.WriteLine($"🖼️ Displaying {filename}");
    }
}

// Proxy
public class ImageProxy : IImage
{
    private readonly string filename;
    private RealImage? realImage;
    
    public ImageProxy(string filename)
    {
        this.filename = filename;
    }
    
    public void Display()
    {
        // Lazy loading - create real image only when needed
        if (realImage == null)
        {
            realImage = new RealImage(filename);
        }
        realImage.Display();
    }
}