using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Structural.Facade;

public class FacadePatternDemo : IPatternDemo
{
    public string PatternName => "Facade";
    public string Description => "Provides a simplified interface to a complex subsystem.";

    public void Demonstrate()
    {
        Console.WriteLine("🎬 Home Theater Facade Example");
        
        var homeTheater = new HomeTheaterFacade();
        
        homeTheater.WatchMovie("The Matrix");
        Console.WriteLine();
        homeTheater.EndMovie();
    }
}

// Facade
public class HomeTheaterFacade
{
    private readonly Amplifier amp = new();
    private readonly DvdPlayer dvd = new();
    private readonly Projector projector = new();
    private readonly Lights lights = new();
    private readonly Screen screen = new();
    
    public void WatchMovie(string movie)
    {
        Console.WriteLine("🎬 Get ready to watch a movie...");
        lights.Dim(10);
        screen.Down();
        projector.On();
        projector.SetInput(dvd);
        amp.On();
        amp.SetVolume(5);
        dvd.On();
        dvd.Play(movie);
    }
    
    public void EndMovie()
    {
        Console.WriteLine("🎬 Shutting movie theater down...");
        dvd.Stop();
        dvd.Off();
        amp.Off();
        projector.Off();
        screen.Up();
        lights.On();
    }
}

// Complex subsystem classes
public class Amplifier
{
    public void On() => Console.WriteLine("🔊 Amplifier on");
    public void Off() => Console.WriteLine("🔊 Amplifier off");
    public void SetVolume(int level) => Console.WriteLine($"🔊 Setting volume to {level}");
}

public class DvdPlayer
{
    public void On() => Console.WriteLine("💿 DVD Player on");
    public void Off() => Console.WriteLine("💿 DVD Player off");
    public void Play(string movie) => Console.WriteLine($"💿 Playing '{movie}'");
    public void Stop() => Console.WriteLine("💿 Stopped");
}

public class Projector
{
    public void On() => Console.WriteLine("📽️ Projector on");
    public void Off() => Console.WriteLine("📽️ Projector off");
    public void SetInput(DvdPlayer dvd) => Console.WriteLine("📽️ Setting DVD input");
}

public class Lights
{
    public void On() => Console.WriteLine("💡 Lights on");
    public void Dim(int level) => Console.WriteLine($"💡 Dimming to {level}%");
}

public class Screen
{
    public void Up() => Console.WriteLine("🎥 Screen going up");
    public void Down() => Console.WriteLine("🎥 Screen going down");
}