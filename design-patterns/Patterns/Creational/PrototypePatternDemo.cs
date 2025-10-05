using DesignPatterns.Infrastructure;
using System.Text.Json;

namespace DesignPatterns.Patterns.Creational.Prototype;

/// <summary>
/// Demonstrates the Prototype pattern with a document template system.
/// The Prototype pattern creates objects by cloning existing instances rather than
/// creating new ones from scratch, which is useful for expensive object creation.
/// </summary>
public class PrototypePatternDemo : IPatternDemo
{
    public string PatternName => "Prototype";

    public string Description => "Creates objects by cloning existing instances. " +
                               "Useful when object creation is expensive or when you need " +
                               "to create objects with similar state to existing ones.";

    public void Demonstrate()
    {
        Console.WriteLine("📄 Document Template Prototype Example");
        Console.WriteLine();

        // Create original templates
        var originalInvoice = CreateInvoiceTemplate();
        var originalReport = CreateReportTemplate();

        Console.WriteLine("🎨 Original Templates Created:");
        Console.WriteLine($"Invoice: {originalInvoice.Title}");
        Console.WriteLine($"Report: {originalReport.Title}");
        Console.WriteLine();

        // Clone and customize templates
        DemonstrateInvoiceCloning(originalInvoice);
        Console.WriteLine();
        
        DemonstrateReportCloning(originalReport);
        Console.WriteLine();
        
        DemonstrateDeepCloning();
    }

    private static DocumentTemplate CreateInvoiceTemplate()
    {
        return new InvoiceTemplate
        {
            Title = "Standard Invoice Template",
            CreatedDate = DateTime.Now,
            Metadata = new DocumentMetadata
            {
                Author = "Finance Department",
                Version = "1.0",
                Tags = ["invoice", "billing", "finance"]
            },
            CompanyInfo = new CompanyInfo
            {
                Name = "Acme Corporation",
                Address = "123 Business St",
                TaxId = "TAX123456"
            }
        };
    }

    private static DocumentTemplate CreateReportTemplate()
    {
        return new ReportTemplate
        {
            Title = "Monthly Report Template",
            CreatedDate = DateTime.Now,
            Metadata = new DocumentMetadata
            {
                Author = "Analytics Team",
                Version = "2.1",
                Tags = ["report", "analytics", "monthly"]
            },
            ReportType = "Financial Summary",
            ChartSettings = new ChartSettings
            {
                ChartType = "Bar",
                ShowLegend = true,
                Colors = ["#FF6B6B", "#4ECDC4", "#45B7D1"]
            }
        };
    }

    private static void DemonstrateInvoiceCloning(DocumentTemplate originalInvoice)
    {
        Console.WriteLine("💰 Creating Custom Invoices from Template:");
        
        // Clone for different customers
        var customerAInvoice = originalInvoice.Clone();
        customerAInvoice.Title = "Invoice for Customer A";
        customerAInvoice.Metadata.Tags.Add("customer-a");
        
        var customerBInvoice = originalInvoice.Clone();
        customerBInvoice.Title = "Invoice for Customer B";
        customerBInvoice.Metadata.Tags.Add("customer-b");
        
        Console.WriteLine($"  📋 {customerAInvoice.Title} - Tags: [{string.Join(", ", customerAInvoice.Metadata.Tags)}]");
        Console.WriteLine($"  📋 {customerBInvoice.Title} - Tags: [{string.Join(", ", customerBInvoice.Metadata.Tags)}]");
        
        // Show that original is unchanged
        Console.WriteLine($"  ✅ Original template unchanged: {originalInvoice.Title}");
    }

    private static void DemonstrateReportCloning(DocumentTemplate originalReport)
    {
        Console.WriteLine("📊 Creating Custom Reports from Template:");
        
        var quarterlyReport = originalReport.Clone();
        quarterlyReport.Title = "Q1 Financial Report";
        quarterlyReport.Metadata.Version = "2.2";
        
        if (quarterlyReport is ReportTemplate report)
        {
            report.ReportType = "Quarterly Summary";
            report.ChartSettings.ChartType = "Line";
        }
        
        var annualReport = originalReport.Clone();
        annualReport.Title = "Annual Performance Report";
        
        Console.WriteLine($"  📈 {quarterlyReport.Title} - Version: {quarterlyReport.Metadata.Version}");
        Console.WriteLine($"  📈 {annualReport.Title} - Version: {annualReport.Metadata.Version}");
    }

    private static void DemonstrateDeepCloning()
    {
        Console.WriteLine("🔍 Deep vs Shallow Cloning Demonstration:");
        
        var original = new InvoiceTemplate
        {
            Title = "Original Invoice",
            Metadata = new DocumentMetadata { Author = "John Doe" },
            CompanyInfo = new CompanyInfo { Name = "Original Company" }
        };
        
        var deepClone = (InvoiceTemplate)original.Clone();
        deepClone.Title = "Deep Clone Invoice";
        deepClone.Metadata.Author = "Jane Smith";
        deepClone.CompanyInfo.Name = "Clone Company";
        
        Console.WriteLine($"  🎯 Original: {original.Title} | Author: {original.Metadata.Author} | Company: {original.CompanyInfo.Name}");
        Console.WriteLine($"  🎯 Clone: {deepClone.Title} | Author: {deepClone.Metadata.Author} | Company: {deepClone.CompanyInfo.Name}");
        Console.WriteLine("  ✅ Deep cloning preserved original object integrity!");
    }
}

/// <summary>
/// The prototype interface that all cloneable objects implement.
/// </summary>
public interface IDocumentPrototype<T>
{
    T Clone();
}

/// <summary>
/// Abstract base class for all document templates.
/// Implements the prototype pattern with deep cloning capability.
/// </summary>
public abstract class DocumentTemplate : IDocumentPrototype<DocumentTemplate>
{
    public string Title { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DocumentMetadata Metadata { get; set; } = new();

    /// <summary>
    /// Creates a deep clone of the document template using JSON serialization.
    /// This ensures all nested objects are properly cloned.
    /// </summary>
    public virtual DocumentTemplate Clone()
    {
        // Use JSON serialization for deep cloning
        var json = JsonSerializer.Serialize(this, GetType());
        return (DocumentTemplate)JsonSerializer.Deserialize(json, GetType())!;
    }

    /// <summary>
    /// Alternative manual cloning method for demonstration purposes.
    /// In production, consider using specialized libraries like AutoMapper.
    /// </summary>
    protected virtual DocumentTemplate ManualClone()
    {
        var clone = (DocumentTemplate)MemberwiseClone();
        clone.Metadata = new DocumentMetadata
        {
            Author = Metadata.Author,
            Version = Metadata.Version,
            Tags = new List<string>(Metadata.Tags)
        };
        return clone;
    }
}

/// <summary>
/// Concrete prototype for invoice documents.
/// </summary>
public class InvoiceTemplate : DocumentTemplate
{
    public CompanyInfo CompanyInfo { get; set; } = new();
    public decimal TaxRate { get; set; } = 0.1m;

    protected override DocumentTemplate ManualClone()
    {
        var clone = (InvoiceTemplate)base.ManualClone();
        clone.CompanyInfo = new CompanyInfo
        {
            Name = CompanyInfo.Name,
            Address = CompanyInfo.Address,
            TaxId = CompanyInfo.TaxId
        };
        return clone;
    }
}

/// <summary>
/// Concrete prototype for report documents.
/// </summary>
public class ReportTemplate : DocumentTemplate
{
    public string ReportType { get; set; } = string.Empty;
    public ChartSettings ChartSettings { get; set; } = new();

    protected override DocumentTemplate ManualClone()
    {
        var clone = (ReportTemplate)base.ManualClone();
        clone.ChartSettings = new ChartSettings
        {
            ChartType = ChartSettings.ChartType,
            ShowLegend = ChartSettings.ShowLegend,
            Colors = new List<string>(ChartSettings.Colors)
        };
        return clone;
    }
}

/// <summary>
/// Supporting class representing document metadata.
/// </summary>
public class DocumentMetadata
{
    public string Author { get; set; } = string.Empty;
    public string Version { get; set; } = "1.0";
    public List<string> Tags { get; set; } = [];
}

/// <summary>
/// Supporting class representing company information.
/// </summary>
public class CompanyInfo
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string TaxId { get; set; } = string.Empty;
}

/// <summary>
/// Supporting class representing chart configuration.
/// </summary>
public class ChartSettings
{
    public string ChartType { get; set; } = "Bar";
    public bool ShowLegend { get; set; } = true;
    public List<string> Colors { get; set; } = [];
}