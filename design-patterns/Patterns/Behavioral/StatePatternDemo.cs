using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.State;

public class StatePatternDemo : IPatternDemo
{
    public string PatternName => "State";
    public string Description => "Allows an object to alter its behavior when its internal state changes.";

    public void Demonstrate()
    {
        Console.WriteLine("🚦 Traffic Light State Example");
        
        var trafficLight = new TrafficLight();
        
        // Cycle through states
        for (int i = 0; i < 6; i++)
        {
            trafficLight.Request();
            Thread.Sleep(500);
        }
    }
}

// State interface
public interface ITrafficLightState
{
    void Handle(TrafficLight context);
}

// Context
public class TrafficLight
{
    private ITrafficLightState state;
    
    public TrafficLight()
    {
        state = new RedState();
        Console.WriteLine("🚦 Traffic light initialized");
    }
    
    public void SetState(ITrafficLightState newState)
    {
        state = newState;
    }
    
    public void Request()
    {
        state.Handle(this);
    }
}

// Concrete States
public class RedState : ITrafficLightState
{
    public void Handle(TrafficLight context)
    {
        Console.WriteLine("🔴 RED - Stop! Changing to Green...");
        context.SetState(new GreenState());
    }
}

public class GreenState : ITrafficLightState
{
    public void Handle(TrafficLight context)
    {
        Console.WriteLine("🟢 GREEN - Go! Changing to Yellow...");
        context.SetState(new YellowState());
    }
}

public class YellowState : ITrafficLightState
{
    public void Handle(TrafficLight context)
    {
        Console.WriteLine("🟡 YELLOW - Caution! Changing to Red...");
        context.SetState(new RedState());
    }
}