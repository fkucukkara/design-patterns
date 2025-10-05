using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Creational.AbstractFactory;

/// <summary>
/// Demonstrates the Abstract Factory pattern with a cross-platform UI component system.
/// The Abstract Factory pattern provides an interface for creating families of related objects
/// without specifying their concrete classes.
/// </summary>
public class AbstractFactoryPatternDemo : IPatternDemo
{
    public string PatternName => "Abstract Factory";

    public string Description => "Creates families of related objects without specifying their concrete classes. " +
                               "Useful when you need to ensure that products from the same family are used together " +
                               "and to make the system independent of how its products are created.";

    public void Demonstrate()
    {
        Console.WriteLine("🎨 Cross-Platform UI Component Factory Example");
        Console.WriteLine();

        // Demonstrate Windows theme
        DemonstrateTheme(new WindowsThemeFactory(), "Windows");
        
        Console.WriteLine();
        
        // Demonstrate macOS theme
        DemonstrateTheme(new MacThemeFactory(), "macOS");
        
        Console.WriteLine();
        
        // Demonstrate Linux theme
        DemonstrateTheme(new LinuxThemeFactory(), "Linux");
    }

    private static void DemonstrateTheme(IUIThemeFactory factory, string themeName)
    {
        Console.WriteLine($"🖥️ Creating {themeName} UI Components:");
        
        // Create related UI components using the factory
        var button = factory.CreateButton();
        var textBox = factory.CreateTextBox();
        var window = factory.CreateWindow();
        
        // Render the components
        button.Render();
        textBox.Render();
        window.Render();
        
        // Show they work together as a family
        Console.WriteLine($"✨ All components have consistent {themeName} styling!");
    }
}

// Abstract Factory Interface
/// <summary>
/// Abstract factory interface that defines methods for creating UI components.
/// Each concrete factory will implement this to create platform-specific components.
/// </summary>
public interface IUIThemeFactory
{
    IButton CreateButton();
    ITextBox CreateTextBox();
    IWindow CreateWindow();
}

// Abstract Product Interfaces
/// <summary>
/// Abstract product interface for buttons.
/// </summary>
public interface IButton
{
    void Render();
    void Click();
}

/// <summary>
/// Abstract product interface for text boxes.
/// </summary>
public interface ITextBox
{
    void Render();
    void SetText(string text);
}

/// <summary>
/// Abstract product interface for windows.
/// </summary>
public interface IWindow
{
    void Render();
    void Minimize();
    void Close();
}

// Concrete Factories
/// <summary>
/// Concrete factory for creating Windows-themed UI components.
/// </summary>
public class WindowsThemeFactory : IUIThemeFactory
{
    public IButton CreateButton() => new WindowsButton();
    public ITextBox CreateTextBox() => new WindowsTextBox();
    public IWindow CreateWindow() => new WindowsWindow();
}

/// <summary>
/// Concrete factory for creating macOS-themed UI components.
/// </summary>
public class MacThemeFactory : IUIThemeFactory
{
    public IButton CreateButton() => new MacButton();
    public ITextBox CreateTextBox() => new MacTextBox();
    public IWindow CreateWindow() => new MacWindow();
}

/// <summary>
/// Concrete factory for creating Linux-themed UI components.
/// </summary>
public class LinuxThemeFactory : IUIThemeFactory
{
    public IButton CreateButton() => new LinuxButton();
    public ITextBox CreateTextBox() => new LinuxTextBox();
    public IWindow CreateWindow() => new LinuxWindow();
}

// Concrete Windows Products
public class WindowsButton : IButton
{
    public void Render() => Console.WriteLine("  🔲 Rendered Windows-style button with blue theme");
    public void Click() => Console.WriteLine("  Windows button clicked with system sound");
}

public class WindowsTextBox : ITextBox
{
    public void Render() => Console.WriteLine("  📝 Rendered Windows-style text box with Segoe UI font");
    public void SetText(string text) => Console.WriteLine($"  Text set: {text}");
}

public class WindowsWindow : IWindow
{
    public void Render() => Console.WriteLine("  🪟 Rendered Windows-style window with title bar and system controls");
    public void Minimize() => Console.WriteLine("  Window minimized to taskbar");
    public void Close() => Console.WriteLine("  Window closed with fade animation");
}

// Concrete macOS Products
public class MacButton : IButton
{
    public void Render() => Console.WriteLine("  🔘 Rendered macOS-style button with rounded corners and subtle shadow");
    public void Click() => Console.WriteLine("  macOS button clicked with elegant haptic feedback");
}

public class MacTextBox : ITextBox
{
    public void Render() => Console.WriteLine("  📝 Rendered macOS-style text box with San Francisco font");
    public void SetText(string text) => Console.WriteLine($"  Text set with smooth cursor animation: {text}");
}

public class MacWindow : IWindow
{
    public void Render() => Console.WriteLine("  🍎 Rendered macOS-style window with traffic light controls");
    public void Minimize() => Console.WriteLine("  Window minimized with genie effect to dock");
    public void Close() => Console.WriteLine("  Window closed with smooth scale animation");
}

// Concrete Linux Products
public class LinuxButton : IButton
{
    public void Render() => Console.WriteLine("  🐧 Rendered Linux-style button with GTK theme");
    public void Click() => Console.WriteLine("  Linux button clicked with customizable action");
}

public class LinuxTextBox : ITextBox
{
    public void Render() => Console.WriteLine("  📝 Rendered Linux-style text box with Liberation Sans font");
    public void SetText(string text) => Console.WriteLine($"  Text set with vim-style navigation: {text}");
}

public class LinuxWindow : IWindow
{
    public void Render() => Console.WriteLine("  🪟 Rendered Linux-style window with customizable window manager");
    public void Minimize() => Console.WriteLine("  Window minimized to workspace switcher");
    public void Close() => Console.WriteLine("  Window closed with configurable behavior");
}