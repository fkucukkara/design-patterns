---
description: 'Guidelines for Design Patterns educational project with modern C# best practices'
applyTo: '**/*.cs'
---

# C# Design Patterns Project Guidelines

## Project Context
This is an educational project demonstrating the 23 Gang of Four (GoF) design patterns using **C# 14** and **.NET 10.0**. The code should be clear, focused, and exemplary for mid to senior-level software engineers.

## C# Language Features
- Always use C# 14 features (matching .NET 10.0 and global.json configuration)
- Leverage pattern matching, nullable reference types, and modern language constructs
- Use file-scoped namespace declarations (as seen throughout the codebase)
- Use collection expressions `[]` for initializing collections
- Prefer nullable reference type annotations (`?`) where appropriate
- Use target-typed `new()` expressions and pattern matching extensively

## Code Quality Standards
- Write comprehensive XML doc comments for all public types, interfaces, and members
- Include `<summary>` tags that explain purpose and usage context
- Add `<example>` tags for pattern demonstrations when they enhance understanding
- Handle edge cases appropriately (patterns in this project focus on demonstration clarity)
- Follow SOLID principles and Gang of Four design pattern principles
- Keep examples clear, focused, and educational - avoid unnecessary complexity
- Each pattern demo should be self-contained and easily understandable

## Naming Conventions
- Follow PascalCase for class names, method names, and public members
- Use camelCase for private fields (with underscore prefix: `_observers`, `_instance`)
- Prefix interface names with "I" (e.g., `IPatternDemo`, `IStockObserver`)
- Use meaningful, domain-specific names that express intent clearly
- Pattern classes should end with the pattern name (e.g., `SingletonPatternDemo`, `ObserverPatternDemo`)
- Avoid abbreviations except for well-known terms (API, XML, JSON, etc.)

## Formatting and Style
- Use file-scoped namespace declarations (no nested braces)
- Insert a newline before the opening curly brace of class/method definitions
- Use expression-bodied members for simple properties (e.g., `public string PatternName => "Observer";`)
- Initialize collections using collection expressions: `private readonly List<IStockObserver> _observers = [];`
- Use pattern matching and `is`/`is not` operators for null checks
- Use `nameof` instead of string literals when referring to member names

## Design Pattern Implementation Guidelines

### Pattern Demo Structure
- All pattern demonstrations must implement the `IPatternDemo` interface
- Include `PatternName` property (short name)
- Include `Description` property (when and why to use the pattern)
- Implement `Demonstrate()` method with clear, self-contained examples
- Organize patterns into appropriate namespace: `DesignPatterns.Patterns.{Creational|Structural|Behavioral}.{PatternName}`

### Pattern Categories
**Creational Patterns** (5): Control object creation mechanisms
- Singleton, Factory Method, Abstract Factory, Builder, Prototype

**Structural Patterns** (7): Compose objects to form larger structures
- Adapter, Bridge, Composite, Decorator, Facade, Flyweight, Proxy

**Behavioral Patterns** (11): Handle object collaboration and responsibility
- Chain of Responsibility, Command, Iterator, Mediator, Memento, Observer, State, Strategy, Template Method, Visitor, Interpreter

### Demonstration Best Practices
- Use emoji icons in console output for visual clarity (🏛️, 🔒, ⚡, 📊, etc.)
- Create multiple examples showing different aspects of the pattern
- Use realistic domain models (e.g., payment processing, stock monitoring, logging)
- Show both the pattern structure and practical usage
- Include examples of subscribing/unsubscribing, adding/removing, or other dynamic behavior
- Keep examples focused - demonstrate the pattern, not unrelated features

## Console Application Patterns
- Use simple Console.WriteLine for output (this is an educational console app)
- No external logging frameworks needed for this project
- Keep user interaction minimal and focused on pattern demonstration
- Use `PatternMenuManager` for discovering and running patterns via reflection

## Nullable Reference Types
- Nullable reference types are enabled in the project (`<Nullable>enable</Nullable>`)
- Declare variables non-nullable by default
- Use `?` suffix for nullable reference types where appropriate
- Always use `is null` or `is not null` instead of `== null` or `!= null`
- Use null-coalescing operators (`??`, `??=`) for concise null handling
- Trust the C# null annotations and avoid redundant null checks

## Code Examples and XML Documentation
- Every public type and member should have XML doc comments with `<summary>` tags
- Include code examples in XML comments using `<example>` tags when helpful
- Explain the "why" of the pattern, not just the "what"
- Document edge cases and important usage notes
- Use domain-specific terminology in comments (e.g., "subject", "observer", "adaptee")

## Error Handling
- Keep error handling simple and appropriate for an educational console application
- Use try-catch in Program.cs for top-level exception handling
- Pattern demonstrations should focus on happy paths unless error handling is part of the pattern
- Validate inputs where it makes sense for the pattern demonstration

## Testing
- Test projects are not currently part of this educational demo
- When adding tests in the future, focus on testing pattern implementations
- Do not emit "Act", "Arrange" or "Assert" comments
- Use descriptive test method names that explain the scenario being tested
- Follow the existing naming conventions in the codebase

## Project Dependencies
- This project intentionally has minimal dependencies (no external NuGet packages)
- Uses only built-in .NET 10.0 libraries
- Pattern demonstrations should not require external libraries
- Keep the project simple and focused on pattern implementation

## Adding New Patterns
When adding new pattern demonstrations:
1. Create a new file in the appropriate category folder (Creational/Structural/Behavioral)
2. Implement the `IPatternDemo` interface
3. Use file-scoped namespace: `DesignPatterns.Patterns.{Category}.{PatternName}`
4. Follow the established pattern structure (PatternName, Description, Demonstrate)
5. Include comprehensive XML documentation
6. Use emoji icons for visual clarity in console output
7. Provide 2-3 demonstration examples showing different aspects
8. The pattern will be automatically discovered by `PatternMenuManager` via reflection