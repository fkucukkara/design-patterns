using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.Pipeline;

/// <summary>
/// Demonstrates the Pipeline pattern with order processing, text transformation,
/// and HTTP middleware simulation scenarios.
/// The Pipeline pattern passes data through a sequence of processing steps where
/// each step transforms the input and passes the result to the next stage.
/// </summary>
public class PipelinePatternDemo : IPatternDemo
{
    public string PatternName => "Pipeline";

    public string Description => "Passes data through a sequence of processing steps (stages), where each step " +
                                 "transforms the input and forwards the result to the next. " +
                                 "Useful for data transformation workflows, middleware chains, and ETL processes " +
                                 "where concerns must remain cleanly separated and composable.";

    public void Demonstrate()
    {
        Console.WriteLine("🔁 Pipeline Pattern Examples");
        Console.WriteLine();

        DemonstrateOrderProcessingPipeline();
        Console.WriteLine();

        DemonstrateTextProcessingPipeline();
        Console.WriteLine();

        DemonstrateHttpMiddlewarePipeline();
    }

    private static void DemonstrateOrderProcessingPipeline()
    {
        Console.WriteLine("🛒 Order Processing Pipeline:");

        var pipeline = new Pipeline<OrderContext>()
            .AddStep(new ParseOrderStep())
            .AddStep(new ValidateOrderStep())
            .AddStep(new ApplyDiscountStep())
            .AddStep(new CalculateTaxStep())
            .AddStep(new FormatReceiptStep());

        var context = new OrderContext
        {
            RawOrderData = "ITEM:Laptop,QTY:2,PRICE:999.99,CUSTOMER:Gold",
        };

        Console.WriteLine($"  📥 Input  : \"{context.RawOrderData}\"");
        pipeline.Execute(context);
        Console.WriteLine($"  📄 Receipt:");
        Console.WriteLine($"     {context.Receipt}");
    }

    private static void DemonstrateTextProcessingPipeline()
    {
        Console.WriteLine("📝 Text Processing Pipeline:");

        var pipeline = new Pipeline<TextContext>()
            .AddStep(new TrimStep())
            .AddStep(new NormalizeWhitespaceStep())
            .AddStep(new SanitizeStep())
            .AddStep(new TruncateStep(maxLength: 60))
            .AddStep(new CapitalizeStep());

        var samples = new[]
        {
            "   hello   WORLD   from   the   pipeline!   ",
            "  <script>alert('xss')</script>  clean  text  here  ",
            "  this sentence will be trimmed because it exceeds the configured maximum length allowed  "
        };

        foreach (var sample in samples)
        {
            var context = new TextContext { Text = sample };
            pipeline.Execute(context);
            Console.WriteLine($"  ✏️  In  : \"{sample.Trim()[..Math.Min(45, sample.Trim().Length)]}...\"");
            Console.WriteLine($"     Out : \"{context.Text}\"");
            Console.WriteLine();
        }
    }

    private static void DemonstrateHttpMiddlewarePipeline()
    {
        Console.WriteLine("🌐 HTTP Middleware Pipeline:");

        var pipeline = new Pipeline<HttpRequestContext>()
            .AddStep(new LoggingMiddlewareStep())
            .AddStep(new AuthenticationMiddlewareStep())
            .AddStep(new RateLimitingMiddlewareStep())
            .AddStep(new RequestHandlerStep());

        var requests = new[]
        {
            new HttpRequestContext { Path = "/api/orders", Token = "Bearer valid-token-abc", ClientIp = "192.168.1.10" },
            new HttpRequestContext { Path = "/api/products", Token = "",                     ClientIp = "10.0.0.5" },
            new HttpRequestContext { Path = "/api/health",  Token = "Bearer valid-token-xyz", ClientIp = "192.168.1.10" },
        };

        foreach (var request in requests)
        {
            Console.WriteLine($"  🔗 {request.Path}");
            pipeline.Execute(request);
            Console.WriteLine($"     Status : {request.StatusCode} — {request.ResponseMessage}");
            Console.WriteLine();
        }
    }
}

// ─── Core Pipeline Infrastructure ────────────────────────────────────────────

/// <summary>
/// Represents a single processing step within a pipeline.
/// </summary>
/// <typeparam name="T">The context object flowing through the pipeline.</typeparam>
public interface IPipelineStep<T>
{
    /// <summary>Processes the context and advances the pipeline state.</summary>
    void Process(T context);
}

/// <summary>
/// Builds and executes an ordered sequence of <see cref="IPipelineStep{T}"/> instances.
/// Steps are executed in the order they are added.
/// </summary>
/// <typeparam name="T">The context object flowing through the pipeline.</typeparam>
public sealed class Pipeline<T>
{
    private readonly List<IPipelineStep<T>> _steps = [];

    /// <summary>Adds a step to the end of the pipeline and returns the pipeline for chaining.</summary>
    public Pipeline<T> AddStep(IPipelineStep<T> step)
    {
        _steps.Add(step);
        return this;
    }

    /// <summary>Executes all steps in order against the provided context.</summary>
    public void Execute(T context)
    {
        foreach (var step in _steps)
            step.Process(context);
    }
}

// ─── Demo 1: Order Processing ─────────────────────────────────────────────────

/// <summary>Mutable context object that carries order state through the pipeline.</summary>
public sealed class OrderContext
{
    public string RawOrderData { get; set; } = string.Empty;
    public string ItemName      { get; set; } = string.Empty;
    public int    Quantity      { get; set; }
    public decimal UnitPrice    { get; set; }
    public string CustomerTier  { get; set; } = string.Empty;
    public decimal Subtotal     { get; set; }
    public decimal DiscountRate { get; set; }
    public decimal TaxAmount    { get; set; }
    public decimal Total        { get; set; }
    public string Receipt       { get; set; } = string.Empty;
    public bool IsValid         { get; set; }
}

/// <summary>Parses the raw order string into typed fields.</summary>
public sealed class ParseOrderStep : IPipelineStep<OrderContext>
{
    public void Process(OrderContext context)
    {
        var parts = context.RawOrderData.Split(',');
        foreach (var part in parts)
        {
            var kv = part.Split(':');
            if (kv.Length is not 2) continue;
            switch (kv[0].Trim().ToUpperInvariant())
            {
                case "ITEM":     context.ItemName     = kv[1].Trim(); break;
                case "QTY":      context.Quantity     = int.Parse(kv[1].Trim()); break;
                case "PRICE":    context.UnitPrice    = decimal.Parse(kv[1].Trim()); break;
                case "CUSTOMER": context.CustomerTier = kv[1].Trim(); break;
            }
        }
        context.Subtotal = context.Quantity * context.UnitPrice;
    }
}

/// <summary>Validates that required order fields are present and well-formed.</summary>
public sealed class ValidateOrderStep : IPipelineStep<OrderContext>
{
    public void Process(OrderContext context)
    {
        context.IsValid = !string.IsNullOrWhiteSpace(context.ItemName)
                          && context.Quantity > 0
                          && context.UnitPrice > 0;
    }
}

/// <summary>Applies a loyalty discount based on customer tier.</summary>
public sealed class ApplyDiscountStep : IPipelineStep<OrderContext>
{
    public void Process(OrderContext context)
    {
        if (!context.IsValid) return;

        context.DiscountRate = context.CustomerTier.ToUpperInvariant() switch
        {
            "GOLD"     => 0.15m,
            "SILVER"   => 0.10m,
            "BRONZE"   => 0.05m,
            _          => 0.00m
        };

        context.Subtotal -= context.Subtotal * context.DiscountRate;
    }
}

/// <summary>Calculates tax on the discounted subtotal.</summary>
public sealed class CalculateTaxStep : IPipelineStep<OrderContext>
{
    private const decimal TaxRate = 0.08m;

    public void Process(OrderContext context)
    {
        if (!context.IsValid) return;
        context.TaxAmount = context.Subtotal * TaxRate;
        context.Total     = context.Subtotal + context.TaxAmount;
    }
}

/// <summary>Formats the final receipt string from the processed context.</summary>
public sealed class FormatReceiptStep : IPipelineStep<OrderContext>
{
    public void Process(OrderContext context)
    {
        if (!context.IsValid)
        {
            context.Receipt = "⚠️  Invalid order — receipt not generated.";
            return;
        }

        context.Receipt =
            $"{context.ItemName} x{context.Quantity} @ ${context.UnitPrice:F2} " +
            $"| Discount: {context.DiscountRate:P0} " +
            $"| Tax: ${context.TaxAmount:F2} " +
            $"| Total: ${context.Total:F2}";
    }
}

// ─── Demo 2: Text Processing ──────────────────────────────────────────────────

/// <summary>Mutable context carrying text state through transformation steps.</summary>
public sealed class TextContext
{
    public string Text { get; set; } = string.Empty;
}

/// <summary>Removes leading and trailing whitespace.</summary>
public sealed class TrimStep : IPipelineStep<TextContext>
{
    public void Process(TextContext context) => context.Text = context.Text.Trim();
}

/// <summary>Collapses multiple consecutive whitespace characters into a single space.</summary>
public sealed class NormalizeWhitespaceStep : IPipelineStep<TextContext>
{
    public void Process(TextContext context) =>
        context.Text = string.Join(' ', context.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries));
}

/// <summary>Strips HTML/script tags to prevent injection.</summary>
public sealed class SanitizeStep : IPipelineStep<TextContext>
{
    public void Process(TextContext context)
    {
        // Remove anything that looks like an HTML tag
        var result = System.Text.RegularExpressions.Regex.Replace(context.Text, "<[^>]*>", string.Empty);
        context.Text = result.Trim();
    }
}

/// <summary>Truncates text to a maximum number of characters, appending an ellipsis if needed.</summary>
public sealed class TruncateStep(int maxLength) : IPipelineStep<TextContext>
{
    public void Process(TextContext context)
    {
        if (context.Text.Length > maxLength)
            context.Text = string.Concat(context.Text.AsSpan(0, maxLength - 1), "…");
    }
}

/// <summary>Capitalizes the first letter and lower-cases the remainder.</summary>
public sealed class CapitalizeStep : IPipelineStep<TextContext>
{
    public void Process(TextContext context)
    {
        if (string.IsNullOrEmpty(context.Text)) return;
        context.Text = char.ToUpperInvariant(context.Text[0]) + context.Text[1..].ToLowerInvariant();
    }
}

// ─── Demo 3: HTTP Middleware ───────────────────────────────────────────────────

/// <summary>Represents an incoming HTTP request flowing through middleware steps.</summary>
public sealed class HttpRequestContext
{
    public string Path            { get; set; } = string.Empty;
    public string Token           { get; set; } = string.Empty;
    public string ClientIp        { get; set; } = string.Empty;
    public int    StatusCode      { get; set; } = 200;
    public string ResponseMessage { get; set; } = string.Empty;
    public bool   IsAborted       { get; set; }
}

/// <summary>Logs the incoming request path (always allows the request to continue).</summary>
public sealed class LoggingMiddlewareStep : IPipelineStep<HttpRequestContext>
{
    public void Process(HttpRequestContext context)
    {
        if (context.IsAborted) return;
        Console.WriteLine($"     📋 [LOG] {context.ClientIp} → {context.Path}");
    }
}

/// <summary>Validates the Bearer token; aborts the pipeline with 401 if missing.</summary>
public sealed class AuthenticationMiddlewareStep : IPipelineStep<HttpRequestContext>
{
    public void Process(HttpRequestContext context)
    {
        if (context.IsAborted) return;

        if (string.IsNullOrWhiteSpace(context.Token) || !context.Token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            context.StatusCode      = 401;
            context.ResponseMessage = "Unauthorized — missing or invalid token.";
            context.IsAborted       = true;
        }
    }
}

/// <summary>
/// Simulates per-IP rate limiting; aborts the pipeline with 429 if the limit is exceeded.
/// </summary>
public sealed class RateLimitingMiddlewareStep : IPipelineStep<HttpRequestContext>
{
    private static readonly Dictionary<string, int> _hitCounts = [];
    private const int MaxRequestsPerIp = 2;

    public void Process(HttpRequestContext context)
    {
        if (context.IsAborted) return;

        _hitCounts.TryGetValue(context.ClientIp, out var count);
        _hitCounts[context.ClientIp] = count + 1;

        if (count >= MaxRequestsPerIp)
        {
            context.StatusCode      = 429;
            context.ResponseMessage = "Too Many Requests — rate limit exceeded.";
            context.IsAborted       = true;
        }
    }
}

/// <summary>Terminal step — produces a 200 OK response for requests that passed all middleware.</summary>
public sealed class RequestHandlerStep : IPipelineStep<HttpRequestContext>
{
    public void Process(HttpRequestContext context)
    {
        if (context.IsAborted) return;
        context.StatusCode      = 200;
        context.ResponseMessage = $"OK — handled request for {context.Path}.";
    }
}
