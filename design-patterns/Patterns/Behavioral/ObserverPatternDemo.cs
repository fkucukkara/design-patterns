using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.Observer;

/// <summary>
/// Demonstrates the Observer pattern with a stock price monitoring system.
/// The Observer pattern defines a one-to-many dependency between objects so that
/// when one object changes state, all dependents are notified automatically.
/// </summary>
public class ObserverPatternDemo : IPatternDemo
{
    public string PatternName => "Observer";

    public string Description => "Defines a one-to-many dependency between objects so that when one object changes state, " +
                               "all its dependents are notified automatically. Useful for implementing event handling systems, " +
                               "model-view architectures, and publish-subscribe patterns.";

    public void Demonstrate()
    {
        Console.WriteLine("📈 Stock Market Observer Pattern Example");
        Console.WriteLine();

        DemonstrateStockPriceNotifications();
        Console.WriteLine();
        
        DemonstrateNewsPublisher();
    }

    private static void DemonstrateStockPriceNotifications()
    {
        Console.WriteLine("💹 Stock Price Monitoring System:");
        
        // Create the subject (stock)
        var appleStock = new Stock("AAPL", "Apple Inc.", 150.00m);
        
        // Create observers
        var mobileApp = new MobileAppNotifier("StockTracker Mobile");
        var emailAlert = new EmailNotifier("alerts@stocktracker.com");
        var tradingBot = new TradingBot("AutoTrader v2.1");
        var dashboard = new TradingDashboard("Main Dashboard");
        
        // Subscribe observers to the stock
        appleStock.Subscribe(mobileApp);
        appleStock.Subscribe(emailAlert);
        appleStock.Subscribe(tradingBot);
        appleStock.Subscribe(dashboard);
        
        Console.WriteLine($"📊 Initial stock price: {appleStock.Symbol} = ${appleStock.Price:F2}");
        Console.WriteLine();
        
        // Simulate price changes
        Console.WriteLine("🔄 Simulating price changes...");
        appleStock.UpdatePrice(155.25m);
        Console.WriteLine();
        
        appleStock.UpdatePrice(148.75m);
        Console.WriteLine();
        
        // Unsubscribe one observer
        Console.WriteLine("📱 Mobile app unsubscribing from notifications...");
        appleStock.Unsubscribe(mobileApp);
        Console.WriteLine();
        
        appleStock.UpdatePrice(160.50m);
    }

    private static void DemonstrateNewsPublisher()
    {
        Console.WriteLine("\n📰 News Publisher System:");
        
        var newsAgency = new NewsAgency("TechNews Central");
        
        // Create subscribers
        var website = new NewsWebsite("TechNews.com");
        var newsletter = new NewsletterService("Weekly Tech Digest");
        var socialMedia = new SocialMediaBot("@TechNewsBot");
        
        // Subscribe to different categories
        newsAgency.Subscribe(website, "Technology");
        newsAgency.Subscribe(newsletter, "Technology");
        newsAgency.Subscribe(socialMedia, "Technology");
        newsAgency.Subscribe(website, "Business");
        
        // Publish news
        newsAgency.PublishNews("Technology", "New breakthrough in quantum computing announced!");
        Console.WriteLine();
        
        newsAgency.PublishNews("Business", "Tech giants report record quarterly earnings");
        Console.WriteLine();
        
        // Unsubscribe
        newsAgency.Unsubscribe(socialMedia, "Technology");
        newsAgency.PublishNews("Technology", "AI model achieves human-level performance in complex reasoning");
    }
}

// Observer interface
/// <summary>
/// Interface that all observers must implement to receive notifications.
/// </summary>
public interface IStockObserver
{
    void Update(Stock stock, decimal oldPrice, decimal newPrice);
}

/// <summary>
/// Generic observer interface for news updates.
/// </summary>
public interface INewsObserver
{
    void OnNewsPublished(string category, string headline);
}

// Subject interface
/// <summary>
/// Interface for subjects that can be observed.
/// </summary>
public interface IStockSubject
{
    void Subscribe(IStockObserver observer);
    void Unsubscribe(IStockObserver observer);
    void NotifyObservers(decimal oldPrice);
}

// Concrete Subject
/// <summary>
/// Concrete subject representing a stock that can be observed for price changes.
/// </summary>
public class Stock : IStockSubject
{
    private readonly List<IStockObserver> _observers = [];
    private decimal _price;

    public string Symbol { get; }
    public string CompanyName { get; }
    public decimal Price 
    { 
        get => _price;
        private set => _price = value;
    }

    public Stock(string symbol, string companyName, decimal initialPrice)
    {
        Symbol = symbol;
        CompanyName = companyName;
        _price = initialPrice;
    }

    public void Subscribe(IStockObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
            Console.WriteLine($"  ✅ Observer subscribed to {Symbol}");
        }
    }

    public void Unsubscribe(IStockObserver observer)
    {
        if (_observers.Remove(observer))
        {
            Console.WriteLine($"  ❌ Observer unsubscribed from {Symbol}");
        }
    }

    public void NotifyObservers(decimal oldPrice)
    {
        Console.WriteLine($"📡 Notifying {_observers.Count} observers about {Symbol} price change...");
        
        foreach (var observer in _observers.ToList()) // ToList() to avoid modification during iteration
        {
            try
            {
                observer.Update(this, oldPrice, _price);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ⚠️ Error notifying observer: {ex.Message}");
            }
        }
    }

    public void UpdatePrice(decimal newPrice)
    {
        if (Math.Abs(newPrice - _price) > 0.01m) // Only notify if significant change
        {
            var oldPrice = _price;
            _price = newPrice;
            NotifyObservers(oldPrice);
        }
    }
}

// Concrete Observers
/// <summary>
/// Mobile app observer that displays notifications to users.
/// </summary>
public class MobileAppNotifier : IStockObserver
{
    public string AppName { get; }

    public MobileAppNotifier(string appName)
    {
        AppName = appName;
    }

    public void Update(Stock stock, decimal oldPrice, decimal newPrice)
    {
        var changePercent = ((newPrice - oldPrice) / oldPrice) * 100;
        var trend = newPrice > oldPrice ? "📈" : "📉";
        
        Console.WriteLine($"  📱 {AppName}: {trend} {stock.Symbol} ${oldPrice:F2} → ${newPrice:F2} ({changePercent:+0.00;-0.00}%)");
        
        // Simulate push notification
        if (Math.Abs(changePercent) > 2)
        {
            Console.WriteLine($"     🔔 Push notification sent: {stock.CompanyName} moved {changePercent:+0.0;-0.0}%!");
        }
    }
}

/// <summary>
/// Email notification observer for price alerts.
/// </summary>
public class EmailNotifier : IStockObserver
{
    public string EmailAddress { get; }

    public EmailNotifier(string emailAddress)
    {
        EmailAddress = emailAddress;
    }

    public void Update(Stock stock, decimal oldPrice, decimal newPrice)
    {
        Console.WriteLine($"  📧 Email to {EmailAddress}: {stock.Symbol} price alert");
        Console.WriteLine($"     📊 Price changed from ${oldPrice:F2} to ${newPrice:F2}");
        
        // Simulate email sending based on thresholds
        if (newPrice < 150)
        {
            Console.WriteLine("     🚨 LOW PRICE ALERT: Consider buying opportunity!");
        }
        else if (newPrice > 160)
        {
            Console.WriteLine("     💰 HIGH PRICE ALERT: Consider selling opportunity!");
        }
    }
}

/// <summary>
/// Automated trading bot observer that makes trading decisions based on price changes.
/// </summary>
public class TradingBot : IStockObserver
{
    public string BotName { get; }
    private readonly decimal _buyThreshold = 149.00m;
    private readonly decimal _sellThreshold = 160.00m;

    public TradingBot(string botName)
    {
        BotName = botName;
    }

    public void Update(Stock stock, decimal oldPrice, decimal newPrice)
    {
        Console.WriteLine($"  🤖 {BotName}: Analyzing {stock.Symbol} price movement...");
        
        if (newPrice <= _buyThreshold && oldPrice > _buyThreshold)
        {
            Console.WriteLine($"     💳 AUTO-BUY: Purchasing {stock.Symbol} at ${newPrice:F2}");
        }
        else if (newPrice >= _sellThreshold && oldPrice < _sellThreshold)
        {
            Console.WriteLine($"     💰 AUTO-SELL: Selling {stock.Symbol} at ${newPrice:F2}");
        }
        else
        {
            Console.WriteLine($"     ⏳ HOLD: Price ${newPrice:F2} within acceptable range");
        }
    }
}

/// <summary>
/// Trading dashboard observer that updates visual displays.
/// </summary>
public class TradingDashboard : IStockObserver
{
    public string DashboardName { get; }

    public TradingDashboard(string dashboardName)
    {
        DashboardName = dashboardName;
    }

    public void Update(Stock stock, decimal oldPrice, decimal newPrice)
    {
        var changeAmount = newPrice - oldPrice;
        var arrow = changeAmount > 0 ? "↗️" : "↘️";
        
        Console.WriteLine($"  🖥️ {DashboardName}: Updated {stock.Symbol} display");
        Console.WriteLine($"     {arrow} ${newPrice:F2} ({changeAmount:+0.00;-0.00})");
        
        // Simulate dashboard chart update
        var bars = Math.Min(10, Math.Max(1, (int)(newPrice / 10)));
        var chart = new string('█', bars);
        Console.WriteLine($"     📊 Chart: {chart} ${newPrice:F2}");
    }
}

// News Publisher System for second demonstration

/// <summary>
/// News agency that publishes news to subscribed observers.
/// Demonstrates Observer pattern with categories/topics.
/// </summary>
public class NewsAgency
{
    private readonly Dictionary<string, List<INewsObserver>> _subscribers = [];
    public string Name { get; }

    public NewsAgency(string name)
    {
        Name = name;
    }

    public void Subscribe(INewsObserver observer, string category)
    {
        if (!_subscribers.ContainsKey(category))
        {
            _subscribers[category] = [];
        }

        if (!_subscribers[category].Contains(observer))
        {
            _subscribers[category].Add(observer);
            Console.WriteLine($"  ✅ Observer subscribed to '{category}' news");
        }
    }

    public void Unsubscribe(INewsObserver observer, string category)
    {
        if (_subscribers.ContainsKey(category) && _subscribers[category].Remove(observer))
        {
            Console.WriteLine($"  ❌ Observer unsubscribed from '{category}' news");
        }
    }

    public void PublishNews(string category, string headline)
    {
        Console.WriteLine($"📰 {Name} publishing: [{category}] {headline}");
        
        if (_subscribers.ContainsKey(category))
        {
            foreach (var subscriber in _subscribers[category].ToList())
            {
                try
                {
                    subscriber.OnNewsPublished(category, headline);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"  ⚠️ Error notifying news subscriber: {ex.Message}");
                }
            }
        }
        else
        {
            Console.WriteLine($"  ℹ️ No subscribers for category '{category}'");
        }
    }
}

public class NewsWebsite : INewsObserver
{
    public string WebsiteName { get; }

    public NewsWebsite(string websiteName)
    {
        WebsiteName = websiteName;
    }

    public void OnNewsPublished(string category, string headline)
    {
        Console.WriteLine($"  🌐 {WebsiteName}: Published article in {category} section");
        Console.WriteLine($"     📝 Headline: {headline}");
    }
}

public class NewsletterService : INewsObserver
{
    public string NewsletterName { get; }

    public NewsletterService(string newsletterName)
    {
        NewsletterName = newsletterName;
    }

    public void OnNewsPublished(string category, string headline)
    {
        Console.WriteLine($"  📧 {NewsletterName}: Added to upcoming newsletter");
        Console.WriteLine($"     📄 [{category}] {headline[..Math.Min(50, headline.Length)]}...");
    }
}

public class SocialMediaBot : INewsObserver
{
    public string BotHandle { get; }

    public SocialMediaBot(string botHandle)
    {
        BotHandle = botHandle;
    }

    public void OnNewsPublished(string category, string headline)
    {
        Console.WriteLine($"  🐦 {BotHandle}: Posted to social media");
        
        // Simulate social media post formatting
        var hashtags = category.ToLower() switch
        {
            "technology" => "#tech #innovation #breakthrough",
            "business" => "#business #finance #economy",
            _ => "#news"
        };
        
        Console.WriteLine($"     💬 \"{headline}\" {hashtags}");
    }
}