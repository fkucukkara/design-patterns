using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Structural.Decorator;

/// <summary>
/// Demonstrates the Decorator pattern with a coffee ordering system.
/// The Decorator pattern allows behavior to be added to objects dynamically
/// without altering their structure, providing a flexible alternative to subclassing.
/// </summary>
public class DecoratorPatternDemo : IPatternDemo
{
    public string PatternName => "Decorator";

    public string Description => "Allows behavior to be added to objects dynamically without altering their structure. " +
                               "Useful for extending functionality of objects in a flexible and composable way, " +
                               "especially when you need multiple combinations of features.";

    public void Demonstrate()
    {
        Console.WriteLine("☕ Coffee Shop Decorator Pattern Example");
        Console.WriteLine();

        DemonstrateBasicCoffeeOrders();
        Console.WriteLine();
        
        DemonstrateComplexCoffeeOrders();
        Console.WriteLine();
        
        DemonstrateTextFormattingDecorators();
    }

    private static void DemonstrateBasicCoffeeOrders()
    {
        Console.WriteLine("☕ Basic Coffee Orders:");
        
        // Basic coffee
        ICoffee basicCoffee = new SimpleCoffee();
        DisplayOrder(basicCoffee);
        
        // Coffee with milk
        ICoffee coffeeWithMilk = new MilkDecorator(new SimpleCoffee());
        DisplayOrder(coffeeWithMilk);
        
        // Coffee with sugar
        ICoffee coffeeWithSugar = new SugarDecorator(new SimpleCoffee());
        DisplayOrder(coffeeWithSugar);
        
        // Coffee with whipped cream
        ICoffee coffeeWithCream = new WhippedCreamDecorator(new SimpleCoffee());
        DisplayOrder(coffeeWithCream);
    }

    private static void DemonstrateComplexCoffeeOrders()
    {
        Console.WriteLine("🎂 Complex Coffee Orders (Multiple Decorators):");
        
        // Latte (Espresso + Milk + Sugar)
        ICoffee latte = new SugarDecorator(
            new MilkDecorator(
                new EspressoCoffee()));
        DisplayOrder(latte, "Latte");
        
        // Cappuccino (Espresso + Milk + Whipped Cream)
        ICoffee cappuccino = new WhippedCreamDecorator(
            new MilkDecorator(
                new EspressoCoffee()));
        DisplayOrder(cappuccino, "Cappuccino");
        
        // Luxury Coffee (All the extras!)
        ICoffee luxuryCoffee = new VanillaDecorator(
            new WhippedCreamDecorator(
                new CaramelDecorator(
                    new SugarDecorator(
                        new MilkDecorator(
                            new EspressoCoffee())))));
        DisplayOrder(luxuryCoffee, "Luxury Coffee");
        
        // Custom Order - Build step by step
        Console.WriteLine("  🔧 Building custom order step by step:");
        ICoffee customOrder = new DarkRoastCoffee();
        Console.WriteLine($"     1. Base: {customOrder.GetDescription()} - ${customOrder.GetCost():F2}");
        
        customOrder = new MilkDecorator(customOrder);
        Console.WriteLine($"     2. +Milk: {customOrder.GetDescription()} - ${customOrder.GetCost():F2}");
        
        customOrder = new SugarDecorator(customOrder);
        Console.WriteLine($"     3. +Sugar: {customOrder.GetDescription()} - ${customOrder.GetCost():F2}");
        
        customOrder = new CaramelDecorator(customOrder);
        Console.WriteLine($"     4. +Caramel: {customOrder.GetDescription()} - ${customOrder.GetCost():F2}");
        
        DisplayOrder(customOrder, "Final Custom Order");
    }

    private static void DemonstrateTextFormattingDecorators()
    {
        Console.WriteLine("📝 Text Formatting Decorator Example:");
        
        // Basic text
        ITextComponent basicText = new PlainText("Hello, World!");
        Console.WriteLine($"  📄 {basicText.Render()}");
        
        // Bold text
        ITextComponent boldText = new BoldDecorator(new PlainText("Important Message"));
        Console.WriteLine($"  📄 {boldText.Render()}");
        
        // Italic text
        ITextComponent italicText = new ItalicDecorator(new PlainText("Emphasized Text"));
        Console.WriteLine($"  📄 {italicText.Render()}");
        
        // Combined formatting
        ITextComponent complexFormatting = new UnderlineDecorator(
            new BoldDecorator(
                new ItalicDecorator(
                    new PlainText("Fully Formatted Text"))));
        Console.WriteLine($"  📄 {complexFormatting.Render()}");
        
        // Colorized text
        ITextComponent colorizedText = new ColorDecorator(
            new BoldDecorator(
                new PlainText("Colorized Bold Text")), 
            "Red");
        Console.WriteLine($"  📄 {colorizedText.Render()}");
    }

    private static void DisplayOrder(ICoffee coffee, string? orderName = null)
    {
        var name = orderName ?? "Order";
        Console.WriteLine($"  🧾 {name}:");
        Console.WriteLine($"     📋 {coffee.GetDescription()}");
        Console.WriteLine($"     💰 Total: ${coffee.GetCost():F2}");
        Console.WriteLine();
    }
}

// Component interface
/// <summary>
/// The base component interface that defines the common interface
/// for both concrete components and decorators.
/// </summary>
public interface ICoffee
{
    string GetDescription();
    decimal GetCost();
}

/// <summary>
/// Text component interface for the text formatting example.
/// </summary>
public interface ITextComponent
{
    string Render();
}

// Concrete Components (the objects that can be decorated)
/// <summary>
/// Basic coffee implementation.
/// </summary>
public class SimpleCoffee : ICoffee
{
    public string GetDescription() => "Simple Coffee";
    public decimal GetCost() => 2.00m;
}

/// <summary>
/// Espresso coffee - a stronger base.
/// </summary>
public class EspressoCoffee : ICoffee
{
    public string GetDescription() => "Espresso";
    public decimal GetCost() => 3.50m;
}

/// <summary>
/// Dark roast coffee - premium base.
/// </summary>
public class DarkRoastCoffee : ICoffee
{
    public string GetDescription() => "Dark Roast Coffee";
    public decimal GetCost() => 2.75m;
}

/// <summary>
/// Plain text component.
/// </summary>
public class PlainText : ITextComponent
{
    private readonly string _text;

    public PlainText(string text)
    {
        _text = text;
    }

    public string Render() => _text;
}

// Base Decorator class
/// <summary>
/// Base decorator class that implements the component interface
/// and contains a reference to a component object.
/// </summary>
public abstract class CoffeeDecorator : ICoffee
{
    protected readonly ICoffee _coffee;

    protected CoffeeDecorator(ICoffee coffee)
    {
        _coffee = coffee;
    }

    public virtual string GetDescription() => _coffee.GetDescription();
    public virtual decimal GetCost() => _coffee.GetCost();
}

/// <summary>
/// Base text decorator class.
/// </summary>
public abstract class TextDecorator : ITextComponent
{
    protected readonly ITextComponent _textComponent;

    protected TextDecorator(ITextComponent textComponent)
    {
        _textComponent = textComponent;
    }

    public virtual string Render() => _textComponent.Render();
}

// Concrete Decorators for Coffee
/// <summary>
/// Milk decorator that adds milk to coffee.
/// </summary>
public class MilkDecorator : CoffeeDecorator
{
    public MilkDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => $"{_coffee.GetDescription()}, Milk";
    public override decimal GetCost() => _coffee.GetCost() + 0.50m;
}

/// <summary>
/// Sugar decorator that adds sugar to coffee.
/// </summary>
public class SugarDecorator : CoffeeDecorator
{
    public SugarDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => $"{_coffee.GetDescription()}, Sugar";
    public override decimal GetCost() => _coffee.GetCost() + 0.25m;
}

/// <summary>
/// Whipped cream decorator that adds whipped cream to coffee.
/// </summary>
public class WhippedCreamDecorator : CoffeeDecorator
{
    public WhippedCreamDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => $"{_coffee.GetDescription()}, Whipped Cream";
    public override decimal GetCost() => _coffee.GetCost() + 0.75m;
}

/// <summary>
/// Vanilla decorator that adds vanilla flavoring to coffee.
/// </summary>
public class VanillaDecorator : CoffeeDecorator
{
    public VanillaDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => $"{_coffee.GetDescription()}, Vanilla";
    public override decimal GetCost() => _coffee.GetCost() + 0.60m;
}

/// <summary>
/// Caramel decorator that adds caramel syrup to coffee.
/// </summary>
public class CaramelDecorator : CoffeeDecorator
{
    public CaramelDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => $"{_coffee.GetDescription()}, Caramel";
    public override decimal GetCost() => _coffee.GetCost() + 0.80m;
}

/// <summary>
/// Extra shot decorator that adds an extra shot of espresso.
/// </summary>
public class ExtraShotDecorator : CoffeeDecorator
{
    public ExtraShotDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => $"{_coffee.GetDescription()}, Extra Shot";
    public override decimal GetCost() => _coffee.GetCost() + 1.25m;
}

/// <summary>
/// Soy milk decorator for lactose-intolerant customers.
/// </summary>
public class SoyMilkDecorator : CoffeeDecorator
{
    public SoyMilkDecorator(ICoffee coffee) : base(coffee) { }

    public override string GetDescription() => $"{_coffee.GetDescription()}, Soy Milk";
    public override decimal GetCost() => _coffee.GetCost() + 0.65m;
}

// Concrete Decorators for Text Formatting
/// <summary>
/// Bold text decorator.
/// </summary>
public class BoldDecorator : TextDecorator
{
    public BoldDecorator(ITextComponent textComponent) : base(textComponent) { }

    public override string Render() => $"**{_textComponent.Render()}**";
}

/// <summary>
/// Italic text decorator.
/// </summary>
public class ItalicDecorator : TextDecorator
{
    public ItalicDecorator(ITextComponent textComponent) : base(textComponent) { }

    public override string Render() => $"*{_textComponent.Render()}*";
}

/// <summary>
/// Underline text decorator.
/// </summary>
public class UnderlineDecorator : TextDecorator
{
    public UnderlineDecorator(ITextComponent textComponent) : base(textComponent) { }

    public override string Render() => $"_{_textComponent.Render()}_";
}

/// <summary>
/// Color text decorator.
/// </summary>
public class ColorDecorator : TextDecorator
{
    private readonly string _color;

    public ColorDecorator(ITextComponent textComponent, string color) : base(textComponent)
    {
        _color = color;
    }

    public override string Render() => $"[{_color}]{_textComponent.Render()}[/{_color}]";
}

/// <summary>
/// Font size decorator.
/// </summary>
public class FontSizeDecorator : TextDecorator
{
    private readonly int _size;

    public FontSizeDecorator(ITextComponent textComponent, int size) : base(textComponent)
    {
        _size = size;
    }

    public override string Render() => $"<size={_size}>{_textComponent.Render()}</size>";
}

/// <summary>
/// Background color decorator.
/// </summary>
public class BackgroundColorDecorator : TextDecorator
{
    private readonly string _backgroundColor;

    public BackgroundColorDecorator(ITextComponent textComponent, string backgroundColor) : base(textComponent)
    {
        _backgroundColor = backgroundColor;
    }

    public override string Render() => $"[bg={_backgroundColor}]{_textComponent.Render()}[/bg]";
}