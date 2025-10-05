using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.ChainOfResponsibility;

public class ChainOfResponsibilityPatternDemo : IPatternDemo
{
    public string PatternName => "Chain of Responsibility";
    public string Description => "Passes requests along a chain of handlers until one handles it.";

    public void Demonstrate()
    {
        Console.WriteLine("⛓️ Support Ticket Chain Example");
        
        var chain = new Level1Support();
        chain.SetNext(new Level2Support())
             .SetNext(new Level3Support());
        
        var tickets = new[]
        {
            new SupportTicket("Password reset", 1),
            new SupportTicket("Server crash", 3),
            new SupportTicket("App bug", 2),
            new SupportTicket("Security breach", 3)
        };
        
        foreach (var ticket in tickets)
        {
            Console.WriteLine($"\n🎫 Processing: {ticket.Issue} (Level {ticket.Priority})");
            chain.Handle(ticket);
        }
    }
}

// Request
public class SupportTicket
{
    public string Issue { get; }
    public int Priority { get; }
    
    public SupportTicket(string issue, int priority)
    {
        Issue = issue;
        Priority = priority;
    }
}

// Handler interface
public abstract class SupportHandler
{
    private SupportHandler? nextHandler;
    
    public SupportHandler SetNext(SupportHandler handler)
    {
        nextHandler = handler;
        return handler;
    }
    
    public virtual void Handle(SupportTicket ticket)
    {
        if (nextHandler != null)
            nextHandler.Handle(ticket);
        else
            Console.WriteLine("❌ No handler available for this ticket");
    }
}

// Concrete Handlers
public class Level1Support : SupportHandler
{
    public override void Handle(SupportTicket ticket)
    {
        if (ticket.Priority <= 1)
            Console.WriteLine("👨‍💻 Level 1 Support: Handling basic issue");
        else
            base.Handle(ticket);
    }
}

public class Level2Support : SupportHandler
{
    public override void Handle(SupportTicket ticket)
    {
        if (ticket.Priority == 2)
            Console.WriteLine("👨‍🔧 Level 2 Support: Handling technical issue");
        else
            base.Handle(ticket);
    }
}

public class Level3Support : SupportHandler
{
    public override void Handle(SupportTicket ticket)
    {
        if (ticket.Priority >= 3)
            Console.WriteLine("👨‍🚀 Level 3 Support: Handling critical issue");
        else
            base.Handle(ticket);
    }
}