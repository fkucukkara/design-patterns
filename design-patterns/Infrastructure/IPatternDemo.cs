namespace DesignPatterns.Infrastructure;

/// <summary>
/// Interface for all design pattern demonstrations.
/// Provides a consistent way to execute and describe pattern examples.
/// </summary>
public interface IPatternDemo
{
    /// <summary>
    /// Gets the name of the design pattern.
    /// </summary>
    string PatternName { get; }

    /// <summary>
    /// Gets a brief description of what the pattern does and when to use it.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Executes the pattern demonstration with example scenarios.
    /// </summary>
    void Demonstrate();
}