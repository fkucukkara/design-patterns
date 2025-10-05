using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.Strategy;

/// <summary>
/// Demonstrates the Strategy pattern with a shipping cost calculation system.
/// The Strategy pattern defines a family of algorithms, encapsulates each one,
/// and makes them interchangeable at runtime.
/// </summary>
public class StrategyPatternDemo : IPatternDemo
{
    public string PatternName => "Strategy";

    public string Description => "Defines a family of algorithms, encapsulates each one, and makes them interchangeable. " +
                               "Useful when you have multiple ways to perform a task and want to choose the algorithm " +
                               "at runtime based on context or configuration.";

    public void Demonstrate()
    {
        Console.WriteLine("📦 Shipping Cost Calculation Strategy Example");
        Console.WriteLine();

        DemonstrateShippingStrategies();
        Console.WriteLine();
        
        DemonstratePaymentStrategies();
        Console.WriteLine();
        
        DemonstrateSortingStrategies();
    }

    private static void DemonstrateShippingStrategies()
    {
        Console.WriteLine("🚚 Shipping Cost Strategies:");
        
        // Create a package to ship
        var package = new Package
        {
            Weight = 5.5m,
            Dimensions = new Dimensions(12, 8, 6),
            Origin = "New York, NY",
            Destination = "Los Angeles, CA",
            IsFragile = true
        };

        // Create shipping calculator with different strategies
        var shippingCalculator = new ShippingCalculator();

        // Try different shipping strategies
        var strategies = new List<IShippingStrategy>
        {
            new StandardShippingStrategy(),
            new ExpressShippingStrategy(),
            new OvernightShippingStrategy(),
            new InternationalShippingStrategy()
        };

        foreach (var strategy in strategies)
        {
            shippingCalculator.SetStrategy(strategy);
            var cost = shippingCalculator.CalculateShippingCost(package);
            var deliveryTime = shippingCalculator.GetEstimatedDeliveryTime(package);
            
            Console.WriteLine($"  📋 {strategy.GetType().Name.Replace("Strategy", "")}:");
            Console.WriteLine($"     💰 Cost: ${cost:F2}");
            Console.WriteLine($"     📅 Delivery: {deliveryTime}");
            Console.WriteLine();
        }
    }

    private static void DemonstratePaymentStrategies()
    {
        Console.WriteLine("💳 Payment Processing Strategies:");
        
        var paymentProcessor = new PaymentProcessor();
        var amount = 250.00m;

        // Different payment strategies
        var paymentStrategies = new List<IPaymentStrategy>
        {
            new CreditCardPaymentStrategy("1234-5678-9012-3456", "John Doe", "123"),
            new PayPalPaymentStrategy("john.doe@email.com"),
            new BankTransferStrategy("123456789", "987654321"),
            new CryptoPaymentStrategy("1A1zP1eP5QGefi2DMPTfTL5SLmv7DivfNa")
        };

        foreach (var strategy in paymentStrategies)
        {
            paymentProcessor.SetPaymentStrategy(strategy);
            var result = paymentProcessor.ProcessPayment(amount);
            
            Console.WriteLine($"  {strategy.GetType().Name.Replace("Strategy", "")}:");
            Console.WriteLine($"     {(result.IsSuccessful ? "✅" : "❌")} {result.Message}");
            Console.WriteLine($"     🆔 Reference: {result.TransactionId}");
            Console.WriteLine();
        }
    }

    private static void DemonstrateSortingStrategies()
    {
        Console.WriteLine("🔢 Data Sorting Strategies:");
        
        var numbers = new List<int> { 64, 34, 25, 12, 22, 11, 90, 5 };
        var sorter = new DataSorter();

        Console.WriteLine($"  📊 Original data: [{string.Join(", ", numbers)}]");
        Console.WriteLine();

        // Different sorting strategies
        var sortingStrategies = new List<ISortStrategy>
        {
            new BubbleSortStrategy(),
            new QuickSortStrategy(),
            new MergeSortStrategy()
        };

        foreach (var strategy in sortingStrategies)
        {
            var dataCopy = new List<int>(numbers);
            sorter.SetSortingStrategy(strategy);
            
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            sorter.Sort(dataCopy);
            stopwatch.Stop();
            
            Console.WriteLine($"  🔄 {strategy.GetType().Name.Replace("Strategy", "")}:");
            Console.WriteLine($"     📈 Result: [{string.Join(", ", dataCopy)}]");
            Console.WriteLine($"     ⏱️ Time: {stopwatch.ElapsedTicks} ticks");
            Console.WriteLine();
        }
    }
}

// Strategy interfaces
/// <summary>
/// Strategy interface for shipping cost calculation.
/// </summary>
public interface IShippingStrategy
{
    decimal CalculateCost(Package package);
    string GetEstimatedDeliveryTime(Package package);
    string GetDescription();
}

/// <summary>
/// Strategy interface for payment processing.
/// </summary>
public interface IPaymentStrategy
{
    PaymentResult ProcessPayment(decimal amount);
    bool ValidatePaymentDetails();
}

/// <summary>
/// Strategy interface for sorting algorithms.
/// </summary>
public interface ISortStrategy
{
    void Sort(List<int> data);
    string GetAlgorithmName();
}

// Context classes
/// <summary>
/// Context class that uses shipping strategies.
/// </summary>
public class ShippingCalculator
{
    private IShippingStrategy? _strategy;

    public void SetStrategy(IShippingStrategy strategy)
    {
        _strategy = strategy;
    }

    public decimal CalculateShippingCost(Package package)
    {
        if (_strategy is null)
            throw new InvalidOperationException("Shipping strategy not set");
            
        return _strategy.CalculateCost(package);
    }

    public string GetEstimatedDeliveryTime(Package package)
    {
        if (_strategy is null)
            throw new InvalidOperationException("Shipping strategy not set");
            
        return _strategy.GetEstimatedDeliveryTime(package);
    }
}

/// <summary>
/// Context class that uses payment strategies.
/// </summary>
public class PaymentProcessor
{
    private IPaymentStrategy? _paymentStrategy;

    public void SetPaymentStrategy(IPaymentStrategy strategy)
    {
        _paymentStrategy = strategy;
    }

    public PaymentResult ProcessPayment(decimal amount)
    {
        if (_paymentStrategy is null)
            return new PaymentResult { IsSuccessful = false, Message = "No payment strategy set" };

        if (!_paymentStrategy.ValidatePaymentDetails())
            return new PaymentResult { IsSuccessful = false, Message = "Invalid payment details" };

        return _paymentStrategy.ProcessPayment(amount);
    }
}

/// <summary>
/// Context class that uses sorting strategies.
/// </summary>
public class DataSorter
{
    private ISortStrategy? _sortingStrategy;

    public void SetSortingStrategy(ISortStrategy strategy)
    {
        _sortingStrategy = strategy;
    }

    public void Sort(List<int> data)
    {
        if (_sortingStrategy is null)
            throw new InvalidOperationException("Sorting strategy not set");
            
        _sortingStrategy.Sort(data);
    }
}

// Supporting models
public class Package
{
    public decimal Weight { get; set; }
    public Dimensions Dimensions { get; set; } = new(0, 0, 0);
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public bool IsFragile { get; set; }
}

public record Dimensions(decimal Length, decimal Width, decimal Height)
{
    public decimal Volume => Length * Width * Height;
}

public class PaymentResult
{
    public bool IsSuccessful { get; set; }
    public string Message { get; set; } = string.Empty;
    public string TransactionId { get; set; } = string.Empty;
}

// Concrete shipping strategies
public class StandardShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(Package package)
    {
        decimal baseCost = package.Weight * 2.50m;
        decimal sizeFactor = package.Dimensions.Volume * 0.1m;
        decimal fragileCharge = package.IsFragile ? 5.00m : 0;
        
        return baseCost + sizeFactor + fragileCharge;
    }

    public string GetEstimatedDeliveryTime(Package package)
    {
        return "5-7 business days";
    }

    public string GetDescription()
    {
        return "Standard ground shipping with basic tracking";
    }
}

public class ExpressShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(Package package)
    {
        decimal baseCost = package.Weight * 5.00m;
        decimal sizeFactor = package.Dimensions.Volume * 0.15m;
        decimal fragileCharge = package.IsFragile ? 10.00m : 0;
        decimal expressCharge = 15.00m;
        
        return baseCost + sizeFactor + fragileCharge + expressCharge;
    }

    public string GetEstimatedDeliveryTime(Package package)
    {
        return "2-3 business days";
    }

    public string GetDescription()
    {
        return "Express shipping with priority handling and enhanced tracking";
    }
}

public class OvernightShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(Package package)
    {
        decimal baseCost = package.Weight * 8.00m;
        decimal sizeFactor = package.Dimensions.Volume * 0.25m;
        decimal fragileCharge = package.IsFragile ? 20.00m : 0;
        decimal overnightCharge = 35.00m;
        
        return baseCost + sizeFactor + fragileCharge + overnightCharge;
    }

    public string GetEstimatedDeliveryTime(Package package)
    {
        return "Next business day by 10:30 AM";
    }

    public string GetDescription()
    {
        return "Overnight delivery with signature confirmation";
    }
}

public class InternationalShippingStrategy : IShippingStrategy
{
    public decimal CalculateCost(Package package)
    {
        decimal baseCost = package.Weight * 12.00m;
        decimal sizeFactor = package.Dimensions.Volume * 0.3m;
        decimal fragileCharge = package.IsFragile ? 25.00m : 0;
        decimal internationalFee = 45.00m;
        decimal customsFee = 20.00m;
        
        return baseCost + sizeFactor + fragileCharge + internationalFee + customsFee;
    }

    public string GetEstimatedDeliveryTime(Package package)
    {
        return "7-14 business days (customs dependent)";
    }

    public string GetDescription()
    {
        return "International shipping with customs handling and insurance";
    }
}

// Concrete payment strategies
public class CreditCardPaymentStrategy : IPaymentStrategy
{
    private readonly string _cardNumber;
    private readonly string _cardHolder;
    private readonly string _cvv;

    public CreditCardPaymentStrategy(string cardNumber, string cardHolder, string cvv)
    {
        _cardNumber = cardNumber;
        _cardHolder = cardHolder;
        _cvv = cvv;
    }

    public PaymentResult ProcessPayment(decimal amount)
    {
        // Simulate credit card processing
        var transactionId = $"CC{DateTime.Now:yyyyMMddHHmmss}{Random.Shared.Next(1000, 9999)}";
        
        return new PaymentResult
        {
            IsSuccessful = true,
            Message = $"Credit card payment of ${amount:F2} processed successfully",
            TransactionId = transactionId
        };
    }

    public bool ValidatePaymentDetails()
    {
        return !string.IsNullOrEmpty(_cardNumber) && 
               !string.IsNullOrEmpty(_cardHolder) && 
               !string.IsNullOrEmpty(_cvv);
    }
}

public class PayPalPaymentStrategy : IPaymentStrategy
{
    private readonly string _email;

    public PayPalPaymentStrategy(string email)
    {
        _email = email;
    }

    public PaymentResult ProcessPayment(decimal amount)
    {
        var transactionId = $"PP{DateTime.Now:yyyyMMdd}{Random.Shared.Next(100000, 999999)}";
        
        return new PaymentResult
        {
            IsSuccessful = true,
            Message = $"PayPal payment of ${amount:F2} processed via {_email}",
            TransactionId = transactionId
        };
    }

    public bool ValidatePaymentDetails()
    {
        return !string.IsNullOrEmpty(_email) && _email.Contains('@');
    }
}

public class BankTransferStrategy : IPaymentStrategy
{
    private readonly string _accountNumber;
    private readonly string _routingNumber;

    public BankTransferStrategy(string accountNumber, string routingNumber)
    {
        _accountNumber = accountNumber;
        _routingNumber = routingNumber;
    }

    public PaymentResult ProcessPayment(decimal amount)
    {
        var transactionId = $"BT{DateTime.Now:yyyyMMdd}{Random.Shared.Next(1000000, 9999999)}";
        
        return new PaymentResult
        {
            IsSuccessful = true,
            Message = $"Bank transfer of ${amount:F2} initiated",
            TransactionId = transactionId
        };
    }

    public bool ValidatePaymentDetails()
    {
        return !string.IsNullOrEmpty(_accountNumber) && !string.IsNullOrEmpty(_routingNumber);
    }
}

public class CryptoPaymentStrategy : IPaymentStrategy
{
    private readonly string _walletAddress;

    public CryptoPaymentStrategy(string walletAddress)
    {
        _walletAddress = walletAddress;
    }

    public PaymentResult ProcessPayment(decimal amount)
    {
        var transactionId = $"0x{Random.Shared.Next():X}{Random.Shared.Next():X}";
        
        return new PaymentResult
        {
            IsSuccessful = true,
            Message = $"Cryptocurrency payment of ${amount:F2} initiated",
            TransactionId = transactionId
        };
    }

    public bool ValidatePaymentDetails()
    {
        return !string.IsNullOrEmpty(_walletAddress) && _walletAddress.Length >= 26;
    }
}

// Concrete sorting strategies
public class BubbleSortStrategy : ISortStrategy
{
    public void Sort(List<int> data)
    {
        int n = data.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (data[j] > data[j + 1])
                {
                    (data[j], data[j + 1]) = (data[j + 1], data[j]);
                }
            }
        }
    }

    public string GetAlgorithmName() => "Bubble Sort";
}

public class QuickSortStrategy : ISortStrategy
{
    public void Sort(List<int> data)
    {
        QuickSort(data, 0, data.Count - 1);
    }

    private static void QuickSort(List<int> arr, int low, int high)
    {
        if (low < high)
        {
            int pi = Partition(arr, low, high);
            QuickSort(arr, low, pi - 1);
            QuickSort(arr, pi + 1, high);
        }
    }

    private static int Partition(List<int> arr, int low, int high)
    {
        int pivot = arr[high];
        int i = low - 1;

        for (int j = low; j < high; j++)
        {
            if (arr[j] < pivot)
            {
                i++;
                (arr[i], arr[j]) = (arr[j], arr[i]);
            }
        }
        (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
        return i + 1;
    }

    public string GetAlgorithmName() => "Quick Sort";
}

public class MergeSortStrategy : ISortStrategy
{
    public void Sort(List<int> data)
    {
        if (data.Count <= 1) return;
        
        var temp = new int[data.Count];
        MergeSort(data, temp, 0, data.Count - 1);
    }

    private static void MergeSort(List<int> arr, int[] temp, int left, int right)
    {
        if (left < right)
        {
            int mid = (left + right) / 2;
            MergeSort(arr, temp, left, mid);
            MergeSort(arr, temp, mid + 1, right);
            Merge(arr, temp, left, mid, right);
        }
    }

    private static void Merge(List<int> arr, int[] temp, int left, int mid, int right)
    {
        int i = left, j = mid + 1, k = left;

        while (i <= mid && j <= right)
        {
            if (arr[i] <= arr[j])
                temp[k++] = arr[i++];
            else
                temp[k++] = arr[j++];
        }

        while (i <= mid) temp[k++] = arr[i++];
        while (j <= right) temp[k++] = arr[j++];

        for (i = left; i <= right; i++)
            arr[i] = temp[i];
    }

    public string GetAlgorithmName() => "Merge Sort";
}