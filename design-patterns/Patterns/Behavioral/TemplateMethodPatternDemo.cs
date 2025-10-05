using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.TemplateMethod;

public class TemplateMethodPatternDemo : IPatternDemo
{
    public string PatternName => "Template Method";
    public string Description => "Defines the skeleton of an algorithm, letting subclasses override specific steps.";

    public void Demonstrate()
    {
        Console.WriteLine("☕ Beverage Template Method Example");
        
        var tea = new Tea();
        var coffee = new Coffee();
        
        Console.WriteLine("🍵 Making tea:");
        tea.PrepareRecipe();
        
        Console.WriteLine("\n☕ Making coffee:");
        coffee.PrepareRecipe();
    }
}

// Abstract class with template method
public abstract class Beverage
{
    // Template method - defines the algorithm skeleton
    public void PrepareRecipe()
    {
        BoilWater();
        Brew();
        PourInCup();
        AddCondiments();
    }
    
    private void BoilWater()
    {
        Console.WriteLine("💧 Boiling water");
    }
    
    private void PourInCup()
    {
        Console.WriteLine("🥤 Pouring into cup");
    }
    
    // Abstract methods - subclasses must implement
    protected abstract void Brew();
    protected abstract void AddCondiments();
}

// Concrete classes
public class Tea : Beverage
{
    protected override void Brew()
    {
        Console.WriteLine("🍵 Steeping the tea");
    }
    
    protected override void AddCondiments()
    {
        Console.WriteLine("🍋 Adding lemon");
    }
}

public class Coffee : Beverage
{
    protected override void Brew()
    {
        Console.WriteLine("☕ Dripping coffee through filter");
    }
    
    protected override void AddCondiments()
    {
        Console.WriteLine("🥛 Adding sugar and milk");
    }
}