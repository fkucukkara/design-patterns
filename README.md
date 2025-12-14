# Design Patterns Educational Demo

A professional implementation of the 23 classic Gang of Four (GoF) design patterns in **C# 14** with **.NET 10.0**. This project serves as a practical reference for mid to senior-level software engineers.

## Overview

Clean, straightforward implementations of all 23 GoF design patterns with:

- **Simple, focused examples** that demonstrate core concepts
- **Professional C# code** following modern best practices
- **Interactive console application** for easy exploration
- **No unnecessary complexity** - direct and to the point

## Project Structure

```
design-patterns/
├── Infrastructure/
│   ├── IPatternDemo.cs         # Pattern interface
│   └── PatternMenuManager.cs   # Console menu system
├── Patterns/
│   ├── Creational/             # 5 creational patterns
│   ├── Structural/             # 7 structural patterns
│   └── Behavioral/             # 11 behavioral patterns
└── Program.cs                  # Application entry point
```

## Usage

The application provides an interactive menu to explore patterns by category:

1. **Select a category** (Creational, Structural, or Behavioral)
2. **Choose a specific pattern** to see its implementation
3. **View the demonstration** with clear, focused examples

Each pattern implementation includes:
- Core pattern structure
- Simple, practical example
- Clean, readable code
- Professional implementation approach

## Getting Started

### Prerequisites
- .NET 10.0 SDK or later
- Any C# IDE (Visual Studio, VS Code, Rider)

### Running the Application
```bash
git clone https://github.com/fkucukkara/design-patterns.git
cd design-patterns
dotnet run --project design-patterns
```

## Implemented Patterns

### Creational Patterns (5/5)
- **Factory Method** - Payment processor creation
- **Abstract Factory** - Cross-platform UI components  
- **Builder** - Database configuration builder
- **Prototype** - Document template cloning
- **Singleton** - Configuration manager

### Structural Patterns (7/7)
- **Adapter** - Legacy system integration
- **Bridge** - Device abstraction
- **Composite** - File system hierarchy
- **Decorator** - Feature enhancement
- **Facade** - System simplification
- **Flyweight** - Memory optimization
- **Proxy** - Lazy loading

### Behavioral Patterns (11/11)
- **Chain of Responsibility** - Request handling
- **Command** - Action encapsulation
- **Iterator** - Collection traversal
- **Mediator** - Object communication
- **Memento** - State preservation
- **Observer** - Event notification
- **State** - State-dependent behavior
- **Strategy** - Algorithm selection
- **Template Method** - Algorithm skeleton
- **Visitor** - Operation separation
   cd design-patterns
   ```

2. **Build the project:**
   ```bash
   dotnet build
   ```

3. **Run the application:**
   ```bash
   dotnet run --project design-patterns
   ```

4. **Navigate the interactive menu** to explore different patterns:
   - Choose pattern categories (Creational, Structural, Behavioral)
   - Select specific patterns to see live demonstrations
   - Follow along with the console output to understand each pattern

## 📚 Implemented Patterns

### Creational Patterns (5/5) ✅
- [x] **Factory Method** - Payment processor creation system
- [x] **Abstract Factory** - Cross-platform UI component families
- [x] **Builder** - Complex database configuration construction
- [x] **Prototype** - Document template cloning system
- [x] **Singleton** - Configuration and resource management

### Structural Patterns (7/7) ✅
- [x] **Adapter** - Payment gateway integration system
- [x] **Bridge** - Remote control for different devices
- [x] **Composite** - File system hierarchy representation
- [x] **Decorator** - Coffee ordering and text formatting system
- [x] **Facade** - Home theater system simplification
- [x] **Flyweight** - Tree forest memory optimization
- [x] **Proxy** - Image lazy loading system

### Behavioral Patterns (11/11) ✅
- [x] **Chain of Responsibility** - Support ticket handling system
- [x] **Command** - Smart home automation with undo functionality
- [x] **Iterator** - Book collection traversal system
- [x] **Mediator** - Chat room communication system
- [x] **Memento** - Text editor undo/redo functionality
- [x] **Observer** - Stock price monitoring and news publishing
- [x] **State** - Traffic light state transitions
- [x] **Strategy** - Shipping cost calculation and payment processing
- [x] **Template Method** - Beverage preparation algorithm
- [x] **Visitor** - Shape calculation operations

## 💡 Pattern Examples Overview

### Factory Method Pattern
**Scenario:** Payment processor creation system
- Creates different payment processors (Credit Card, PayPal, Bank Transfer, Crypto)
- Demonstrates object creation without specifying exact classes
- Shows how to handle unsupported payment types gracefully

### Abstract Factory Pattern
**Scenario:** Cross-platform UI component system
- Creates families of related UI components (Windows, macOS, Linux themes)
- Ensures components from the same family work together
- Demonstrates platform-specific implementations

### Builder Pattern
**Scenario:** Database configuration system
- Constructs complex database configurations step by step
- Provides both traditional builder and fluent interface approaches
- Handles optional parameters and validation

### Prototype Pattern
**Scenario:** Document template system
- Clones document templates for customization
- Demonstrates both shallow and deep cloning approaches
- Uses JSON serialization for reliable deep cloning

### Singleton Pattern
**Scenario:** Configuration and resource management
- Shows thread-safe implementations using `Lazy<T>`
- Demonstrates practical use cases (logging, caching, configuration)
- Compares different singleton implementation approaches

### Decorator Pattern
**Scenario:** Coffee ordering and text formatting system
- Adds functionality to objects dynamically without altering their structure
- Demonstrates multiple decorators (milk, sugar, caramel, etc.) for coffee customization
- Shows text formatting decorators (bold, italic, underline, color)
- Illustrates flexible composition of features

### Command Pattern
**Scenario:** Smart home automation system
- Encapsulates requests as objects for parameterization and queuing
- Demonstrates macro commands that execute multiple actions
- Implements comprehensive undo/redo functionality
- Shows command scheduling and queuing capabilities

### Observer Pattern
**Scenario:** Stock market monitoring system
- Implements subject-observer relationships for price notifications
- Shows one-to-many dependencies with automatic updates
- Demonstrates category-based subscriptions with news publishing

### Strategy Pattern
**Scenario:** Shipping cost calculation system
- Encapsulates different algorithms for shipping cost calculation
- Allows runtime algorithm selection
- Shows multiple strategy examples (shipping, payment, sorting)

## 🛠️ Technology Features

### Modern C# 14 Features Used
- **File-scoped namespaces** for cleaner code organization
- **Nullable reference types** for better null safety
- **Pattern matching** with switch expressions
- **Primary constructors** where appropriate
- **Record types** for immutable data models
- **Global using statements** for common imports

### .NET 10.0 Features
- **Latest performance improvements**
- **Enhanced nullable annotations**
- **Improved JSON serialization**
- **Better async/await patterns**

### Best Practices Demonstrated
- **SOLID principles** throughout the codebase
- **Dependency injection** patterns where applicable
- **Exception handling** with meaningful error messages
- **Documentation** with XML comments and examples
- **Clean code** principles with descriptive naming
- **Separation of concerns** with proper layering

## 🎓 Learning Path

### For Beginners
1. Start with **Creational Patterns** to understand object creation
2. Move to **Structural Patterns** to learn about object composition
3. Explore **Behavioral Patterns** to understand object interaction

### For Each Pattern
1. **Read the description** in the interactive menu
2. **Run the demonstration** to see the pattern in action
3. **Examine the code** to understand the implementation
4. **Try modifying** the examples to test your understanding

### Suggested Study Order
1. **Factory Method** - Simplest creational pattern
2. **Observer** - Fundamental behavioral pattern
3. **Strategy** - Commonly used behavioral pattern
4. **Adapter** - Essential structural pattern
5. **Decorator** - Flexible structural pattern
6. **Command** - Powerful behavioral pattern with undo support
7. **Singleton** - Widely used but often misused
8. Continue with remaining patterns

## 🔧 Development Setup

### Building and Testing
```bash
# Clean build
dotnet clean
dotnet build

# Run with verbose output
dotnet run --project design-patterns --verbosity detailed

# Build for release
dotnet build --configuration Release
```

### Code Style
The project follows the coding standards defined in `.editorconfig`:
- File-scoped namespaces
- 4-space indentation
- PascalCase for public members
- camelCase for private fields
- Comprehensive XML documentation

## 📖 Additional Resources

### Design Patterns References
- **Gang of Four Book:** "Design Patterns: Elements of Reusable Object-Oriented Software"
- **Head First Design Patterns** - Excellent for beginners
- **Refactoring.Guru** - Interactive pattern explanations
- **Microsoft Documentation** - C# and .NET best practices

### C# and .NET Resources
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/)
- [.NET 10.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)

## License
[![MIT License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

This project is licensed under the MIT License. See the [`LICENSE`](LICENSE) file for details.
