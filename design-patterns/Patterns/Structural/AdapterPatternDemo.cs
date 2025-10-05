using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Structural.Adapter;

/// <summary>
/// Demonstrates the Adapter pattern with a payment gateway integration system.
/// The Adapter pattern allows incompatible interfaces to work together by
/// wrapping an existing class with a new interface.
/// </summary>
public class AdapterPatternDemo : IPatternDemo
{
    public string PatternName => "Adapter";

    public string Description => "Allows incompatible interfaces to work together. " +
                               "Useful when integrating with third-party libraries or legacy systems " +
                               "that have different interfaces than what your application expects.";

    public void Demonstrate()
    {
        Console.WriteLine("🔌 Payment Gateway Adapter Example");
        Console.WriteLine();

        // Demonstrate using different payment gateways through a common interface
        DemonstratePaymentProcessing();
        Console.WriteLine();
        
        DemonstrateDataSourceAdapters();
    }

    private static void DemonstratePaymentProcessing()
    {
        Console.WriteLine("💳 Processing payments through different gateways:");
        
        // Create adapters for different payment systems
        var stripeAdapter = new StripePaymentAdapter(new StripePaymentGateway());
        var paypalAdapter = new PayPalPaymentAdapter(new PayPalAPI());
        var squareAdapter = new SquarePaymentAdapter(new SquareProcessor());
        
        // Use them through the common interface
        var paymentProcessors = new List<IPaymentProcessor>
        {
            stripeAdapter,
            paypalAdapter,
            squareAdapter
        };
        
        var paymentRequest = new PaymentRequest
        {
            Amount = 99.99m,
            Currency = "USD",
            CustomerEmail = "customer@example.com",
            Description = "Test payment"
        };
        
        foreach (var processor in paymentProcessors)
        {
            try
            {
                var result = processor.ProcessPayment(paymentRequest);
                Console.WriteLine($"  ✅ {processor.GetType().Name}: {result.Message}");
                Console.WriteLine($"     Transaction ID: {result.TransactionId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  ❌ {processor.GetType().Name}: {ex.Message}");
            }
            Console.WriteLine();
        }
    }

    private static void DemonstrateDataSourceAdapters()
    {
        Console.WriteLine("📊 Data Source Adapter Example:");
        
        // Different data sources with incompatible interfaces
        var xmlAdapter = new XmlDataAdapter(new XmlDataSource());
        var jsonAdapter = new JsonDataAdapter(new JsonDataSource());
        var csvAdapter = new CsvDataAdapter(new CsvDataSource());
        
        var dataSources = new List<IDataSource> { xmlAdapter, jsonAdapter, csvAdapter };
        
        foreach (var source in dataSources)
        {
            var data = source.GetData("users");
            Console.WriteLine($"  📁 {source.GetType().Name}: Retrieved {data.Count} records");
            foreach (var record in data.Take(2))
            {
                Console.WriteLine($"     • {record}");
            }
            Console.WriteLine();
        }
    }
}

// Target interface that our application expects
/// <summary>
/// The common interface that our application expects for payment processing.
/// </summary>
public interface IPaymentProcessor
{
    PaymentResult ProcessPayment(PaymentRequest request);
    bool ValidatePayment(PaymentRequest request);
}

/// <summary>
/// Common data source interface for the adapter pattern demonstration.
/// </summary>
public interface IDataSource
{
    List<string> GetData(string query);
}

// Application's common models
public class PaymentRequest
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public string CustomerEmail { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class PaymentResult
{
    public bool IsSuccess { get; set; }
    public string TransactionId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
}

// Third-party service classes (adaptees) - these have incompatible interfaces

/// <summary>
/// Stripe's third-party payment gateway with its own interface.
/// </summary>
public class StripePaymentGateway
{
    public StripeChargeResult CreateCharge(StripeChargeRequest request)
    {
        // Simulate Stripe's API call
        return new StripeChargeResult
        {
            Id = $"ch_{Guid.NewGuid():N}"[..24],
            Status = "succeeded",
            Amount = request.AmountInCents
        };
    }
}

public class StripeChargeRequest
{
    public long AmountInCents { get; set; }
    public string Currency { get; set; } = "usd";
    public string CustomerEmail { get; set; } = string.Empty;
}

public class StripeChargeResult
{
    public string Id { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public long Amount { get; set; }
}

/// <summary>
/// PayPal's API with a different interface structure.
/// </summary>
public class PayPalAPI
{
    public PayPalTransaction ExecutePayment(PayPalPaymentInfo payment)
    {
        // Simulate PayPal's API call
        return new PayPalTransaction
        {
            TransactionId = $"PAY-{Random.Shared.Next(10000000, 99999999)}",
            State = "approved",
            Total = payment.TotalAmount.ToString("F2")
        };
    }
}

public class PayPalPaymentInfo
{
    public decimal TotalAmount { get; set; }
    public string CurrencyCode { get; set; } = "USD";
    public string PayerEmail { get; set; } = string.Empty;
}

public class PayPalTransaction
{
    public string TransactionId { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Total { get; set; } = string.Empty;
}

/// <summary>
/// Square's payment processor with yet another interface.
/// </summary>
public class SquareProcessor
{
    public SquarePaymentResponse ProcessSquarePayment(SquarePaymentDetails details)
    {
        // Simulate Square's API call
        return new SquarePaymentResponse
        {
            PaymentId = Guid.NewGuid().ToString(),
            Status = "COMPLETED",
            AmountMoney = details.AmountMoney
        };
    }
}

public class SquarePaymentDetails
{
    public SquareAmountMoney AmountMoney { get; set; } = new();
    public string BuyerEmailAddress { get; set; } = string.Empty;
}

public class SquareAmountMoney
{
    public long Amount { get; set; }
    public string Currency { get; set; } = "USD";
}

public class SquarePaymentResponse
{
    public string PaymentId { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public SquareAmountMoney AmountMoney { get; set; } = new();
}

// Adapter classes that make third-party services compatible with our interface

/// <summary>
/// Adapter that makes Stripe's payment gateway compatible with our IPaymentProcessor interface.
/// </summary>
public class StripePaymentAdapter : IPaymentProcessor
{
    private readonly StripePaymentGateway _stripeGateway;

    public StripePaymentAdapter(StripePaymentGateway stripeGateway)
    {
        _stripeGateway = stripeGateway;
    }

    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        // Adapt our request to Stripe's format
        var stripeRequest = new StripeChargeRequest
        {
            AmountInCents = (long)(request.Amount * 100),
            Currency = request.Currency.ToLowerInvariant(),
            CustomerEmail = request.CustomerEmail
        };

        // Call Stripe's API
        var stripeResult = _stripeGateway.CreateCharge(stripeRequest);

        // Adapt Stripe's response to our format
        return new PaymentResult
        {
            IsSuccess = stripeResult.Status == "succeeded",
            TransactionId = stripeResult.Id,
            Message = $"Stripe payment {stripeResult.Status}"
        };
    }

    public bool ValidatePayment(PaymentRequest request)
    {
        return request.Amount > 0 && !string.IsNullOrEmpty(request.CustomerEmail);
    }
}

/// <summary>
/// Adapter that makes PayPal's API compatible with our IPaymentProcessor interface.
/// </summary>
public class PayPalPaymentAdapter : IPaymentProcessor
{
    private readonly PayPalAPI _paypalApi;

    public PayPalPaymentAdapter(PayPalAPI paypalApi)
    {
        _paypalApi = paypalApi;
    }

    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        // Adapt our request to PayPal's format
        var paypalPayment = new PayPalPaymentInfo
        {
            TotalAmount = request.Amount,
            CurrencyCode = request.Currency,
            PayerEmail = request.CustomerEmail
        };

        // Call PayPal's API
        var paypalResult = _paypalApi.ExecutePayment(paypalPayment);

        // Adapt PayPal's response to our format
        return new PaymentResult
        {
            IsSuccess = paypalResult.State == "approved",
            TransactionId = paypalResult.TransactionId,
            Message = $"PayPal payment {paypalResult.State} for ${paypalResult.Total}"
        };
    }

    public bool ValidatePayment(PaymentRequest request)
    {
        return request.Amount > 0 && request.CustomerEmail.Contains('@');
    }
}

/// <summary>
/// Adapter that makes Square's processor compatible with our IPaymentProcessor interface.
/// </summary>
public class SquarePaymentAdapter : IPaymentProcessor
{
    private readonly SquareProcessor _squareProcessor;

    public SquarePaymentAdapter(SquareProcessor squareProcessor)
    {
        _squareProcessor = squareProcessor;
    }

    public PaymentResult ProcessPayment(PaymentRequest request)
    {
        // Adapt our request to Square's format
        var squareDetails = new SquarePaymentDetails
        {
            AmountMoney = new SquareAmountMoney
            {
                Amount = (long)(request.Amount * 100),
                Currency = request.Currency
            },
            BuyerEmailAddress = request.CustomerEmail
        };

        // Call Square's API
        var squareResult = _squareProcessor.ProcessSquarePayment(squareDetails);

        // Adapt Square's response to our format
        return new PaymentResult
        {
            IsSuccess = squareResult.Status == "COMPLETED",
            TransactionId = squareResult.PaymentId,
            Message = $"Square payment {squareResult.Status}"
        };
    }

    public bool ValidatePayment(PaymentRequest request)
    {
        return request.Amount >= 1; // Square has minimum amount requirements
    }
}

// Data source examples for the second demonstration

/// <summary>
/// Legacy XML data source with its own interface.
/// </summary>
public class XmlDataSource
{
    public string GetXmlData(string entityName)
    {
        return $"<{entityName}><item>John Doe</item><item>Jane Smith</item></{entityName}>";
    }
}

/// <summary>
/// JSON data service with a different interface.
/// </summary>
public class JsonDataSource
{
    public string RetrieveJsonData(string table)
    {
        return $"{{\"data\": [{{\"name\": \"Alice Johnson\"}}, {{\"name\": \"Bob Wilson\"}}]}}";
    }
}

/// <summary>
/// CSV data provider with yet another interface.
/// </summary>
public class CsvDataSource
{
    public string GetCsvContent(string dataSet)
    {
        return "name\nCharlie Brown\nDiana Prince";
    }
}

// Data source adapters

public class XmlDataAdapter : IDataSource
{
    private readonly XmlDataSource _xmlSource;

    public XmlDataAdapter(XmlDataSource xmlSource)
    {
        _xmlSource = xmlSource;
    }

    public List<string> GetData(string query)
    {
        var xmlData = _xmlSource.GetXmlData(query);
        // Simple XML parsing simulation
        return ["John Doe (from XML)", "Jane Smith (from XML)"];
    }
}

public class JsonDataAdapter : IDataSource
{
    private readonly JsonDataSource _jsonSource;

    public JsonDataAdapter(JsonDataSource jsonSource)
    {
        _jsonSource = jsonSource;
    }

    public List<string> GetData(string query)
    {
        var jsonData = _jsonSource.RetrieveJsonData(query);
        // Simple JSON parsing simulation
        return ["Alice Johnson (from JSON)", "Bob Wilson (from JSON)"];
    }
}

public class CsvDataAdapter : IDataSource
{
    private readonly CsvDataSource _csvSource;

    public CsvDataAdapter(CsvDataSource csvSource)
    {
        _csvSource = csvSource;
    }

    public List<string> GetData(string query)
    {
        var csvData = _csvSource.GetCsvContent(query);
        // Simple CSV parsing simulation
        return ["Charlie Brown (from CSV)", "Diana Prince (from CSV)"];
    }
}