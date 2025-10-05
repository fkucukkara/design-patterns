using DesignPatterns.Infrastructure;

namespace DesignPatterns.Patterns.Behavioral.Iterator;

public class IteratorPatternDemo : IPatternDemo
{
    public string PatternName => "Iterator";
    public string Description => "Provides a way to access elements of a collection sequentially.";

    public void Demonstrate()
    {
        Console.WriteLine("📚 Book Collection Iterator Example");
        
        var library = new BookCollection();
        library.AddBook(new Book("Design Patterns", "GoF"));
        library.AddBook(new Book("Clean Code", "Robert Martin"));
        library.AddBook(new Book("Refactoring", "Martin Fowler"));
        
        Console.WriteLine("📖 Iterating through books:");
        var iterator = library.CreateIterator();
        
        while (iterator.HasNext())
        {
            var book = iterator.Next();
            Console.WriteLine($"  📘 {book.Title} by {book.Author}");
        }
        
        Console.WriteLine("\n🔄 Using foreach (built-in iterator):");
        foreach (var book in library)
        {
            Console.WriteLine($"  📗 {book.Title} by {book.Author}");
        }
    }
}

// Element
public class Book
{
    public string Title { get; }
    public string Author { get; }
    
    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }
}

// Iterator interface
public interface IIterator<T>
{
    bool HasNext();
    T Next();
}

// Concrete Iterator
public class BookIterator : IIterator<Book>
{
    private readonly List<Book> books;
    private int position = 0;
    
    public BookIterator(List<Book> books)
    {
        this.books = books;
    }
    
    public bool HasNext() => position < books.Count;
    
    public Book Next()
    {
        if (!HasNext())
            throw new InvalidOperationException("No more elements");
        return books[position++];
    }
}

// Aggregate
public class BookCollection : IEnumerable<Book>
{
    private readonly List<Book> books = new();
    
    public void AddBook(Book book) => books.Add(book);
    
    public IIterator<Book> CreateIterator() => new BookIterator(books);
    
    // Implementing IEnumerable for built-in foreach support
    public IEnumerator<Book> GetEnumerator() => books.GetEnumerator();
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
}