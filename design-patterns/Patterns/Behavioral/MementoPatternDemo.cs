using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.Memento;

public class MementoPatternDemo : IPatternDemo
{
    public string PatternName => "Memento";
    public string Description => "Captures and restores an object's internal state without violating encapsulation.";

    public void Demonstrate()
    {
        Console.WriteLine("💾 Text Editor Memento Example");
        
        var editor = new TextEditor();
        var history = new EditorHistory();
        
        editor.Write("Hello");
        history.Save(editor);
        Console.WriteLine($"📝 Current: '{editor.Content}'");
        
        editor.Write(" World");
        history.Save(editor);
        Console.WriteLine($"📝 Current: '{editor.Content}'");
        
        editor.Write("!!!");
        Console.WriteLine($"📝 Current: '{editor.Content}'");
        
        Console.WriteLine("\n⏪ Undoing last change:");
        history.Undo(editor);
        Console.WriteLine($"📝 Current: '{editor.Content}'");
        
        Console.WriteLine("\n⏪ Undoing again:");
        history.Undo(editor);
        Console.WriteLine($"📝 Current: '{editor.Content}'");
    }
}

// Memento
public class EditorMemento
{
    public string Content { get; }
    public DateTime Timestamp { get; }
    
    public EditorMemento(string content)
    {
        Content = content;
        Timestamp = DateTime.Now;
    }
}

// Originator
public class TextEditor
{
    public string Content { get; private set; } = string.Empty;
    
    public void Write(string text)
    {
        Content += text;
    }
    
    public EditorMemento Save()
    {
        return new EditorMemento(Content);
    }
    
    public void Restore(EditorMemento memento)
    {
        Content = memento.Content;
    }
}

// Caretaker
public class EditorHistory
{
    private readonly Stack<EditorMemento> history = new();
    
    public void Save(TextEditor editor)
    {
        history.Push(editor.Save());
    }
    
    public void Undo(TextEditor editor)
    {
        if (history.Count > 0)
        {
            var memento = history.Pop();
            editor.Restore(memento);
        }
        else
        {
            Console.WriteLine("❌ No more states to restore");
        }
    }
}