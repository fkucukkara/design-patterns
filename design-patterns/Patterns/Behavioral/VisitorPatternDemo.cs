using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.Visitor;

public class VisitorPatternDemo : IPatternDemo
{
    public string PatternName => "Visitor";
    public string Description => "Defines operations to be performed on elements without changing their classes.";

    public void Demonstrate()
    {
        Console.WriteLine("🎯 Shape Visitor Example");
        
        var shapes = new List<IShape>
        {
            new Circle(5),
            new Rectangle(4, 6),
            new Triangle(3, 4)
        };
        
        var areaCalculator = new AreaCalculator();
        var perimeterCalculator = new PerimeterCalculator();
        
        Console.WriteLine("📐 Calculating areas:");
        foreach (var shape in shapes)
        {
            shape.Accept(areaCalculator);
        }
        
        Console.WriteLine("\n📏 Calculating perimeters:");
        foreach (var shape in shapes)
        {
            shape.Accept(perimeterCalculator);
        }
    }
}

// Visitor interface
public interface IShapeVisitor
{
    void Visit(Circle circle);
    void Visit(Rectangle rectangle);
    void Visit(Triangle triangle);
}

// Element interface
public interface IShape
{
    void Accept(IShapeVisitor visitor);
}

// Concrete Elements
public class Circle : IShape
{
    public double Radius { get; }
    
    public Circle(double radius) => Radius = radius;
    
    public void Accept(IShapeVisitor visitor) => visitor.Visit(this);
}

public class Rectangle : IShape
{
    public double Width { get; }
    public double Height { get; }
    
    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }
    
    public void Accept(IShapeVisitor visitor) => visitor.Visit(this);
}

public class Triangle : IShape
{
    public double Base { get; }
    public double Height { get; }
    
    public Triangle(double baseLength, double height)
    {
        Base = baseLength;
        Height = height;
    }
    
    public void Accept(IShapeVisitor visitor) => visitor.Visit(this);
}

// Concrete Visitors
public class AreaCalculator : IShapeVisitor
{
    public void Visit(Circle circle)
    {
        var area = Math.PI * circle.Radius * circle.Radius;
        Console.WriteLine($"🔵 Circle area: {area:F2}");
    }
    
    public void Visit(Rectangle rectangle)
    {
        var area = rectangle.Width * rectangle.Height;
        Console.WriteLine($"⬜ Rectangle area: {area:F2}");
    }
    
    public void Visit(Triangle triangle)
    {
        var area = 0.5 * triangle.Base * triangle.Height;
        Console.WriteLine($"🔺 Triangle area: {area:F2}");
    }
}

public class PerimeterCalculator : IShapeVisitor
{
    public void Visit(Circle circle)
    {
        var perimeter = 2 * Math.PI * circle.Radius;
        Console.WriteLine($"🔵 Circle perimeter: {perimeter:F2}");
    }
    
    public void Visit(Rectangle rectangle)
    {
        var perimeter = 2 * (rectangle.Width + rectangle.Height);
        Console.WriteLine($"⬜ Rectangle perimeter: {perimeter:F2}");
    }
    
    public void Visit(Triangle triangle)
    {
        // Simplified - assuming right triangle
        var hypotenuse = Math.Sqrt(triangle.Base * triangle.Base + triangle.Height * triangle.Height);
        var perimeter = triangle.Base + triangle.Height + hypotenuse;
        Console.WriteLine($"🔺 Triangle perimeter: {perimeter:F2}");
    }
}