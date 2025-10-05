using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Creational.Singleton;

/// <summary>
/// Demonstrates the Singleton pattern with different implementation approaches.
/// The Singleton pattern ensures a class has only one instance and provides
/// global access to that instance.
/// </summary>
public class SingletonPatternDemo : IPatternDemo
{
    public string PatternName => "Singleton";

    public string Description => "Ensures a class has only one instance and provides global access to it. " +
                               "Useful for logging, configuration, database connections, and other resources " +
                               "that should have only one instance throughout the application lifecycle.";

    public void Demonstrate()
    {
        Console.WriteLine("Singleton Pattern Demonstration");
        Console.WriteLine();

        DemonstrateBasicSingleton();
        Console.WriteLine();
        
        DemonstrateThreadSafeSingleton();
        Console.WriteLine();
        
        DemonstrateLazySingleton();
        Console.WriteLine();
        
        DemonstrateConfigurationManager();
    }

    private static void DemonstrateBasicSingleton()
    {
        Console.WriteLine("🏛️ Basic Singleton (Not Thread-Safe):");
        
        var logger1 = BasicLogger.Instance;
        var logger2 = BasicLogger.Instance;
        
        logger1.Log("First message from logger1");
        logger2.Log("Second message from logger2");
        
        Console.WriteLine($"  Same instance? {ReferenceEquals(logger1, logger2)}");
        Console.WriteLine($"  Instance hash codes: {logger1.GetHashCode()} == {logger2.GetHashCode()}");
    }

    private static void DemonstrateThreadSafeSingleton()
    {
        Console.WriteLine("🔒 Thread-Safe Singleton:");
        
        var database1 = ThreadSafeDatabase.Instance;
        var database2 = ThreadSafeDatabase.Instance;
        
        database1.ExecuteQuery("SELECT * FROM Users");
        database2.ExecuteQuery("UPDATE Users SET Status = 'Active'");
        
        Console.WriteLine($"  Same instance? {ReferenceEquals(database1, database2)}");
        Console.WriteLine($"  Connection count: {database1.ConnectionCount}");
    }

    private static void DemonstrateLazySingleton()
    {
        Console.WriteLine("⚡ Lazy Singleton (Modern C# Approach):");
        
        var cache1 = ModernCache.Instance;
        var cache2 = ModernCache.Instance;
        
        cache1.Set("user:123", "John Doe");
        var user = cache2.Get("user:123");
        
        Console.WriteLine($"  Retrieved from cache: {user}");
        Console.WriteLine($"  Same instance? {ReferenceEquals(cache1, cache2)}");
        Console.WriteLine($"  Cache size: {cache1.Count}");
    }

    private static void DemonstrateConfigurationManager()
    {
        Console.WriteLine("⚙️ Practical Example - Configuration Manager:");
        
        var config = ConfigurationManager.Instance;
        
        // Set some configuration values
        config.SetValue("DatabaseConnectionString", "Server=localhost;Database=MyApp;");
        config.SetValue("ApiTimeout", "30");
        config.SetValue("EnableLogging", "true");
        
        // Retrieve configuration from another reference
        var anotherConfigRef = ConfigurationManager.Instance;
        
        Console.WriteLine($"  Database Connection: {anotherConfigRef.GetValue("DatabaseConnectionString")}");
        Console.WriteLine($"  API Timeout: {anotherConfigRef.GetValue("ApiTimeout")} seconds");
        Console.WriteLine($"  Logging Enabled: {anotherConfigRef.GetValue("EnableLogging")}");
        Console.WriteLine($"  Total config items: {config.ConfigurationCount}");
    }
}

/// <summary>
/// Basic Singleton implementation (not thread-safe).
/// This is the simplest form but should not be used in multi-threaded applications.
/// </summary>
public class BasicLogger
{
    private static BasicLogger? _instance;

    // Private constructor prevents direct instantiation
    private BasicLogger()
    {
        Console.WriteLine("  🎯 BasicLogger instance created");
    }

    public static BasicLogger Instance
    {
        get
        {
            // Not thread-safe - could create multiple instances in concurrent scenarios
            _instance ??= new BasicLogger();
            return _instance;
        }
    }

    public void Log(string message)
    {
        Console.WriteLine($"  📝 [{DateTime.Now:HH:mm:ss}] {message}");
    }
}

/// <summary>
/// Thread-safe Singleton implementation using double-checked locking.
/// This ensures thread safety while maintaining performance.
/// </summary>
public class ThreadSafeDatabase
{
    private static ThreadSafeDatabase? _instance;
    private static readonly object _lock = new();
    private int _connectionCount;

    private ThreadSafeDatabase()
    {
        Console.WriteLine("  🎯 ThreadSafeDatabase instance created");
        _connectionCount = 1;
    }

    public static ThreadSafeDatabase Instance
    {
        get
        {
            // Double-checked locking pattern
            if (_instance is null)
            {
                lock (_lock)
                {
                    _instance ??= new ThreadSafeDatabase();
                }
            }
            return _instance;
        }
    }

    public int ConnectionCount => _connectionCount;

    public void ExecuteQuery(string query)
    {
        Console.WriteLine($"  🗄️ Executing: {query}");
        _connectionCount++;
    }
}

/// <summary>
/// Modern Singleton implementation using Lazy&lt;T&gt;.
/// This is the recommended approach in modern C# - it's thread-safe and lazy by default.
/// </summary>
public class ModernCache
{
    private static readonly Lazy<ModernCache> _lazy = new(() => new ModernCache());
    private readonly Dictionary<string, string> _cache = [];

    private ModernCache()
    {
        Console.WriteLine("  🎯 ModernCache instance created (lazy initialization)");
    }

    public static ModernCache Instance => _lazy.Value;

    public int Count => _cache.Count;

    public void Set(string key, string value)
    {
        _cache[key] = value;
        Console.WriteLine($"  💾 Cached: {key} = {value}");
    }

    public string? Get(string key)
    {
        _cache.TryGetValue(key, out var value);
        return value;
    }
}

/// <summary>
/// Practical Singleton example - Configuration Manager.
/// This demonstrates a real-world use case where Singleton pattern is beneficial.
/// </summary>
public sealed class ConfigurationManager
{
    private static readonly Lazy<ConfigurationManager> _lazy = new(() => new ConfigurationManager());
    private readonly Dictionary<string, string> _configurations = [];

    private ConfigurationManager()
    {
        Console.WriteLine("  🎯 ConfigurationManager instance created");
        LoadDefaultConfigurations();
    }

    public static ConfigurationManager Instance => _lazy.Value;

    public int ConfigurationCount => _configurations.Count;

    public void SetValue(string key, string value)
    {
        _configurations[key] = value;
    }

    public string GetValue(string key, string defaultValue = "")
    {
        return _configurations.TryGetValue(key, out var value) ? value : defaultValue;
    }

    public bool HasValue(string key) => _configurations.ContainsKey(key);

    private void LoadDefaultConfigurations()
    {
        // Simulate loading default configurations
        _configurations["AppName"] = "Design Patterns Demo";
        _configurations["Version"] = "1.0.0";
        _configurations["Environment"] = "Development";
    }

    /// <summary>
    /// Example of a method that might load configurations from a file or database.
    /// </summary>
    public void LoadFromSource(string source)
    {
        Console.WriteLine($"  📂 Loading configurations from: {source}");
        // Implementation would load from actual source
    }
}