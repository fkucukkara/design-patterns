using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Creational.Builder;

/// <summary>
/// Demonstrates the Builder pattern with a complex configuration object.
/// The Builder pattern constructs complex objects step by step, allowing for
/// different representations of the same construction process.
/// </summary>
public class BuilderPatternDemo : IPatternDemo
{
    public string PatternName => "Builder";

    public string Description => "Constructs complex objects step by step. " +
                               "Useful when creating objects with many optional parameters " +
                               "or when the construction process should allow different representations.";

    public void Demonstrate()
    {
        Console.WriteLine("🏗️ Database Configuration Builder Example");
        Console.WriteLine();

        // Demonstrate different ways to build database configurations
        DemonstrateDevelopmentConfig();
        Console.WriteLine();
        
        DemonstrateProductionConfig();
        Console.WriteLine();
        
        DemonstrateCustomConfig();
        Console.WriteLine();
        
        DemonstrateFluentBuilder();
    }

    private static void DemonstrateDevelopmentConfig()
    {
        Console.WriteLine("🔧 Development Database Configuration:");
        
        var devConfig = new DatabaseConfigurationBuilder()
            .SetConnectionString("Server=localhost;Database=DevDB;")
            .SetTimeout(30)
            .EnableLogging()
            .SetPoolSize(10)
            .Build();
            
        Console.WriteLine(devConfig);
    }

    private static void DemonstrateProductionConfig()
    {
        Console.WriteLine("🚀 Production Database Configuration:");
        
        var prodConfig = new DatabaseConfigurationBuilder()
            .SetConnectionString("Server=prod-server;Database=ProdDB;Encrypt=true;")
            .SetTimeout(60)
            .EnableConnectionPooling()
            .SetPoolSize(100)
            .EnableRetryLogic(maxRetries: 3)
            .SetCommandTimeout(120)
            .Build();
            
        Console.WriteLine(prodConfig);
    }

    private static void DemonstrateCustomConfig()
    {
        Console.WriteLine("⚙️ Custom Database Configuration:");
        
        var customConfig = new DatabaseConfigurationBuilder()
            .SetConnectionString("Server=custom-server;Database=CustomDB;")
            .SetTimeout(45)
            .EnableLogging()
            .EnableConnectionPooling()
            .SetPoolSize(50)
            .EnableRetryLogic(maxRetries: 5)
            .SetCommandTimeout(90)
            .Build();
            
        Console.WriteLine(customConfig);
    }

    private static void DemonstrateFluentBuilder()
    {
        Console.WriteLine("✨ Fluent Builder with Method Chaining:");
        
        // Demonstrate the fluent interface in a single chain
        var fluentConfig = DatabaseConfigurationBuilder
            .CreateNew()
            .ForServer("fluent-server")
            .WithDatabase("FluentDB")
            .WithEncryption()
            .WithTimeout(75)
            .WithConnectionPooling(poolSize: 25)
            .WithRetryLogic(maxRetries: 2)
            .WithLogging()
            .Build();
            
        Console.WriteLine(fluentConfig);
    }
}

/// <summary>
/// The complex product that the builder constructs.
/// Represents a database configuration with many optional settings.
/// </summary>
public class DatabaseConfiguration
{
    public string ConnectionString { get; init; } = string.Empty;
    public int TimeoutSeconds { get; init; } = 30;
    public bool IsLoggingEnabled { get; init; }
    public bool IsConnectionPoolingEnabled { get; init; }
    public int PoolSize { get; init; } = 10;
    public bool IsRetryLogicEnabled { get; init; }
    public int MaxRetries { get; init; }
    public int CommandTimeoutSeconds { get; init; } = 30;

    public override string ToString()
    {
        var config = new List<string>
        {
            $"  📡 Connection: {ConnectionString}",
            $"  ⏱️ Timeout: {TimeoutSeconds}s",
            $"  📝 Logging: {(IsLoggingEnabled ? "Enabled" : "Disabled")}",
            $"  🏊 Connection Pooling: {(IsConnectionPoolingEnabled ? $"Enabled (Size: {PoolSize})" : "Disabled")}",
            $"  🔄 Retry Logic: {(IsRetryLogicEnabled ? $"Enabled (Max: {MaxRetries})" : "Disabled")}",
            $"  ⌛ Command Timeout: {CommandTimeoutSeconds}s"
        };

        return string.Join(Environment.NewLine, config);
    }
}

/// <summary>
/// The builder class that constructs DatabaseConfiguration objects step by step.
/// Provides both traditional builder methods and fluent interface methods.
/// </summary>
public class DatabaseConfigurationBuilder
{
    private string _connectionString = string.Empty;
    private int _timeoutSeconds = 30;
    private bool _isLoggingEnabled;
    private bool _isConnectionPoolingEnabled;
    private int _poolSize = 10;
    private bool _isRetryLogicEnabled;
    private int _maxRetries;
    private int _commandTimeoutSeconds = 30;

    /// <summary>
    /// Creates a new builder instance for fluent interface.
    /// </summary>
    public static DatabaseConfigurationBuilder CreateNew() => new();

    // Traditional builder methods
    public DatabaseConfigurationBuilder SetConnectionString(string connectionString)
    {
        _connectionString = connectionString;
        return this;
    }

    public DatabaseConfigurationBuilder SetTimeout(int seconds)
    {
        _timeoutSeconds = seconds;
        return this;
    }

    public DatabaseConfigurationBuilder EnableLogging()
    {
        _isLoggingEnabled = true;
        return this;
    }

    public DatabaseConfigurationBuilder EnableConnectionPooling()
    {
        _isConnectionPoolingEnabled = true;
        return this;
    }

    public DatabaseConfigurationBuilder SetPoolSize(int size)
    {
        _poolSize = size;
        _isConnectionPoolingEnabled = true; // Automatically enable pooling
        return this;
    }

    public DatabaseConfigurationBuilder EnableRetryLogic(int maxRetries)
    {
        _isRetryLogicEnabled = true;
        _maxRetries = maxRetries;
        return this;
    }

    public DatabaseConfigurationBuilder SetCommandTimeout(int seconds)
    {
        _commandTimeoutSeconds = seconds;
        return this;
    }

    // Fluent interface methods with more expressive names
    public DatabaseConfigurationBuilder ForServer(string server)
    {
        _connectionString = $"Server={server};";
        return this;
    }

    public DatabaseConfigurationBuilder WithDatabase(string database)
    {
        _connectionString += $"Database={database};";
        return this;
    }

    public DatabaseConfigurationBuilder WithEncryption()
    {
        _connectionString += "Encrypt=true;";
        return this;
    }

    public DatabaseConfigurationBuilder WithTimeout(int seconds)
    {
        _timeoutSeconds = seconds;
        return this;
    }

    public DatabaseConfigurationBuilder WithConnectionPooling(int poolSize = 10)
    {
        _isConnectionPoolingEnabled = true;
        _poolSize = poolSize;
        return this;
    }

    public DatabaseConfigurationBuilder WithRetryLogic(int maxRetries)
    {
        _isRetryLogicEnabled = true;
        _maxRetries = maxRetries;
        return this;
    }

    public DatabaseConfigurationBuilder WithLogging()
    {
        _isLoggingEnabled = true;
        return this;
    }

    /// <summary>
    /// Builds the final DatabaseConfiguration object.
    /// </summary>
    public DatabaseConfiguration Build()
    {
        return new DatabaseConfiguration
        {
            ConnectionString = _connectionString,
            TimeoutSeconds = _timeoutSeconds,
            IsLoggingEnabled = _isLoggingEnabled,
            IsConnectionPoolingEnabled = _isConnectionPoolingEnabled,
            PoolSize = _poolSize,
            IsRetryLogicEnabled = _isRetryLogicEnabled,
            MaxRetries = _maxRetries,
            CommandTimeoutSeconds = _commandTimeoutSeconds
        };
    }

    /// <summary>
    /// Resets the builder to its initial state for reuse.
    /// </summary>
    public DatabaseConfigurationBuilder Reset()
    {
        _connectionString = string.Empty;
        _timeoutSeconds = 30;
        _isLoggingEnabled = false;
        _isConnectionPoolingEnabled = false;
        _poolSize = 10;
        _isRetryLogicEnabled = false;
        _maxRetries = 0;
        _commandTimeoutSeconds = 30;
        return this;
    }
}