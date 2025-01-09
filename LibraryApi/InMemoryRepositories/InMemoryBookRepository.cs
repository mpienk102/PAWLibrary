using System.ComponentModel.DataAnnotations;
using LibraryApi.Models;

public class InMemoryBookRepository : IBookRepository{
    private readonly List<Book> _books = new(){
        new Book(1,"Horse","JP2GMD","Books of Horses",BookState.Unavailable,"Drama"),
        new Book(2,"Cat","JD2","Book of Cats",BookState.Available,"History")
    };

    public IEnumerable<Book> GetAll() => _books;

    public Book? GetById(int id) =>_books.FirstOrDefault(i => i.Id == id);

    public Book? GetBookByTitleAndAuthor(string title, string author)
    {
        return _books.FirstOrDefault(b => b.Title == title && b.Author == author);
    }
    public IEnumerable<Book> GetByCategory([Required]string category) => 
    _books.Where(book => book.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
    
    public IEnumerable<Book> GetByAuthor([Required]string author) =>
    _books.Where(book => book.Author.Equals(author, StringComparison.OrdinalIgnoreCase));

    public void Add(Book book)
    {
        var newId = _books.Any() ? _books.Max(i => i.Id) +1 : 1;
        _books.Add(new Book
        {
            Id = newId,
            Title = book.Title,
            Description = book.Description,
            State = book.State,
            Category = book.Category,
            Author = book.Author
        });
    }

    public void Update(int id, Book updatedBook)
    {
        var index = _books.FindIndex(i => i.Id == id);
        if (index != -1)
        {
            var book = _books[index];
            book.Title = updatedBook.Title;
            book.Description = updatedBook.Description;
            book.State = updatedBook.State;
            book.Category = updatedBook.Category; 
            book.Author = updatedBook.Author; 
        }
    }
    public void Delete(int id)
    {
        var item = GetById(id);
        if (item != null)
        {
            _books.Remove(item);
        }
    }
}