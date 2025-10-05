using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Creational.Factory;

/// <summary>
/// Demonstrates the Factory Method pattern with a payment processing system.
/// The Factory pattern creates objects without specifying their exact classes,
/// allowing for flexible object creation based on runtime conditions.
/// </summary>
public class FactoryPatternDemo : IPatternDemo
{
    public string PatternName => "Factory Method";

    public string Description => "Creates objects without specifying their exact classes. " +
                               "Useful when the type of object needs to be determined at runtime " +
                               "based on configuration or user input.";

    public void Demonstrate()
    {
        Console.WriteLine("Payment Processing Factory Example");
        Console.WriteLine();

        // Demonstrate factory pattern with different payment methods
        ProcessPayment("credit-card", 150.00m);
        ProcessPayment("paypal", 89.99m);
        ProcessPayment("bank-transfer", 250.00m);
        ProcessPayment("crypto", 75.50m);

        // Try an unsupported payment method
        ProcessPayment("carrier-pigeon", 10.00m);
    }

    private static void ProcessPayment(string paymentType, decimal amount)
    {
        try
        {
            var processor = PaymentProcessorFactory.CreateProcessor(paymentType);
            var result = processor.ProcessPayment(amount);
            
            Console.WriteLine($"SUCCESS {paymentType}: {result}");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"ERROR {paymentType}: {ex.Message}");
        }
        
        Console.WriteLine();
    }
}

/// <summary>
/// Abstract product that all payment processors implement.
/// </summary>
public abstract class PaymentProcessor
{
    /// <summary>
    /// Processes a payment for the specified amount.
    /// </summary>
    /// <param name="amount">The amount to process</param>
    /// <returns>A confirmation message</returns>
    public abstract string ProcessPayment(decimal amount);
}

/// <summary>
/// Concrete implementation for credit card payments.
/// </summary>
public class CreditCardProcessor : PaymentProcessor
{
    public override string ProcessPayment(decimal amount)
    {
        // Simulate credit card processing logic
        var transactionId = Guid.NewGuid().ToString()[..8].ToUpper();
        return $"Credit card payment of ${amount:F2} processed. Transaction ID: {transactionId}";
    }
}

/// <summary>
/// Concrete implementation for PayPal payments.
/// </summary>
public class PayPalProcessor : PaymentProcessor
{
    public override string ProcessPayment(decimal amount)
    {
        // Simulate PayPal processing logic
        var reference = $"PP-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
        return $"PayPal payment of ${amount:F2} processed. Reference: {reference}";
    }
}

/// <summary>
/// Concrete implementation for bank transfer payments.
/// </summary>
public class BankTransferProcessor : PaymentProcessor
{
    public override string ProcessPayment(decimal amount)
    {
        // Simulate bank transfer processing logic
        var transferId = $"BT{DateTime.Now:yyMMdd}{Random.Shared.Next(100000, 999999)}";
        return $"Bank transfer of ${amount:F2} initiated. Transfer ID: {transferId}";
    }
}

/// <summary>
/// Concrete implementation for cryptocurrency payments.
/// </summary>
public class CryptoProcessor : PaymentProcessor
{
    public override string ProcessPayment(decimal amount)
    {
        // Simulate crypto processing logic
        var blockHash = Convert.ToHexString(System.Security.Cryptography.RandomNumberGenerator.GetBytes(4));
        return $"Cryptocurrency payment of ${amount:F2} confirmed. Block: 0x{blockHash}";
    }
}

/// <summary>
/// Factory class that creates payment processors based on the payment type.
/// This is the core of the Factory pattern - it encapsulates object creation logic.
/// </summary>
public static class PaymentProcessorFactory
{
    /// <summary>
    /// Creates the appropriate payment processor based on the payment type.
    /// </summary>
    /// <param name="paymentType">The type of payment method</param>
    /// <returns>A concrete payment processor instance</returns>
    /// <exception cref="ArgumentException">Thrown when the payment type is not supported</exception>
    public static PaymentProcessor CreateProcessor(string paymentType)
    {
        return paymentType.ToLowerInvariant() switch
        {
            "credit-card" or "creditcard" => new CreditCardProcessor(),
            "paypal" => new PayPalProcessor(),
            "bank-transfer" or "banktransfer" => new BankTransferProcessor(),
            "crypto" or "cryptocurrency" => new CryptoProcessor(),
            _ => throw new ArgumentException($"Unsupported payment type: {paymentType}")
        };
    }
}