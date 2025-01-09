using System.ComponentModel.DataAnnotations;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;
namespace LibraryApi.Repositories
{
public class BookRepository : IBookRepository
{
    private readonly LibraryDbContext _context;

    public BookRepository(LibraryDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Book> GetAll() => _context.Books.ToList();

    public Book? GetById(int id) => _context.Books.Find(id);

     public Book? GetBookByTitleAndAuthor(string title, string author)
    {
        return _context.Books.FirstOrDefault(b => b.Title == title && b.Author == author);
    }
    public IEnumerable<Book> GetByCategory([Required]string category) 
    {
        return  _context.Books
        .Where(book => book.Category.ToLower() == category.ToLower())
        .ToList();
    }

    public IEnumerable<Book> GetByAuthor([Required]string author)
    {
        return  _context.Books
        .Where(book => book.Author.ToLower() == author.ToLower())
        .ToList();
    }

    public void Add (Book book)
    {
        _context.Books.Add(book);
        _context.SaveChangesAsync();
    }
    public void Update(int id, Book updatedBook)
    {
        var existingBook = _context.Books.Find(id);
        if (existingBook is not null)
        {
            existingBook.Title = updatedBook.Title;
            existingBook.Description = updatedBook.Description;
            existingBook.State = updatedBook.State;
            existingBook.Category = updatedBook.Category;
            existingBook.Author = updatedBook.Author;
        }
        _context.SaveChangesAsync();
    }
    public void Delete(int id)
    {
        var book = _context.Books.Find(id);
        if (book is not null)
        {
            _context.Books.Remove(book);
            _context.SaveChangesAsync();
        }
    }
}
}