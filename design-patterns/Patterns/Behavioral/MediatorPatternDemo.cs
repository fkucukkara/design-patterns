using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.Mediator;

public class MediatorPatternDemo : IPatternDemo
{
    public string PatternName => "Mediator";
    public string Description => "Defines how objects interact with each other through a mediator.";

    public void Demonstrate()
    {
        Console.WriteLine("💬 Chat Room Mediator Example");
        
        var chatRoom = new ChatRoom();
        
        var alice = new User("Alice", chatRoom);
        var bob = new User("Bob", chatRoom);
        var charlie = new User("Charlie", chatRoom);
        
        alice.Send("Hello everyone!");
        bob.Send("Hi Alice!");
        charlie.Send("Hey there!");
    }
}

// Mediator interface
public interface IChatMediator
{
    void SendMessage(string message, User user);
    void AddUser(User user);
}

// Concrete Mediator
public class ChatRoom : IChatMediator
{
    private readonly List<User> users = new();
    
    public void AddUser(User user)
    {
        users.Add(user);
        Console.WriteLine($"👋 {user.Name} joined the chat");
    }
    
    public void SendMessage(string message, User sender)
    {
        foreach (var user in users)
        {
            if (user != sender)
                user.Receive(message, sender.Name);
        }
    }
}

// Colleague
public class User
{
    public string Name { get; }
    private readonly IChatMediator mediator;
    
    public User(string name, IChatMediator mediator)
    {
        Name = name;
        this.mediator = mediator;
        mediator.AddUser(this);
    }
    
    public void Send(string message)
    {
        Console.WriteLine($"📤 {Name}: {message}");
        mediator.SendMessage(message, this);
    }
    
    public void Receive(string message, string from)
    {
        Console.WriteLine($"📥 {Name} received from {from}: {message}");
    }
}