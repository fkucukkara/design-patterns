using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Structural.Bridge;

public class BridgePatternDemo : IPatternDemo
{
    public string PatternName => "Bridge";
    public string Description => "Separates abstraction from implementation so both can vary independently.";

    public void Demonstrate()
    {
        Console.WriteLine("🌉 Remote Control Bridge Example");
        
        var tv = new TV();
        var radio = new Radio();
        
        var basicRemote = new BasicRemote(tv);
        var advancedRemote = new AdvancedRemote(radio);
        
        basicRemote.Power();
        basicRemote.VolumeUp();
        
        advancedRemote.Power();
        advancedRemote.Mute();
    }
}

// Abstraction
public class BasicRemote
{
    protected IDevice device;
    
    public BasicRemote(IDevice device) => this.device = device;
    
    public void Power() 
    { 
        if (device.IsEnabled()) 
            device.Disable(); 
        else 
            device.Enable(); 
    }
    public void VolumeUp() => device.SetVolume(device.GetVolume() + 10);
}

// Refined Abstraction
public class AdvancedRemote : BasicRemote
{
    public AdvancedRemote(IDevice device) : base(device) { }
    
    public void Mute() => device.SetVolume(0);
}

// Implementation Interface
public interface IDevice
{
    bool IsEnabled();
    void Enable();
    void Disable();
    int GetVolume();
    void SetVolume(int volume);
}

// Concrete Implementations
public class TV : IDevice
{
    private bool enabled;
    private int volume = 50;
    
    public bool IsEnabled() => enabled;
    public void Enable() { enabled = true; Console.WriteLine("📺 TV is ON"); }
    public void Disable() { enabled = false; Console.WriteLine("📺 TV is OFF"); }
    public int GetVolume() => volume;
    public void SetVolume(int volume) { this.volume = volume; Console.WriteLine($"📺 TV volume: {volume}"); }
}

public class Radio : IDevice
{
    private bool enabled;
    private int volume = 30;
    
    public bool IsEnabled() => enabled;
    public void Enable() { enabled = true; Console.WriteLine("📻 Radio is ON"); }
    public void Disable() { enabled = false; Console.WriteLine("📻 Radio is OFF"); }
    public int GetVolume() => volume;
    public void SetVolume(int volume) { this.volume = volume; Console.WriteLine($"📻 Radio volume: {volume}"); }
}