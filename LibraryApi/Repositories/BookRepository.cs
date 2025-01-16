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

        public async Task<IEnumerable<Book>> GetAll() => await _context.Books.ToListAsync();

        public async Task<Book?> GetById(int id) => await _context.Books.FindAsync(id);

        public async Task<Book?> GetBookByTitleAndAuthor(string title, string author)
        {
            try
            {
                var book = await _context.Books
                    .FirstOrDefaultAsync(b =>
                        EF.Functions.Like(b.Title.ToLower(), title.ToLower()) &&
                        EF.Functions.Like(b.Author.ToLower(), author.ToLower()));
                return book;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBookByTitleAndAuthor: {ex.Message}");
                throw;
            }
        }

        // Updated to filter by BookCategory (enum), not a string comparison.
        public async Task<IEnumerable<Book>> GetByCategory([Required] BookCategory category)
        {
            return await _context.Books
                .Where(book => book.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Book>> GetByAuthor([Required] string author)
        {
            return await _context.Books
                .Where(book => book.Author.ToLower() == author.ToLower())
                .ToListAsync();
        }

        public async Task Add(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int id, Book updatedBook)
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
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var book = _context.Books.Find(id);
            if (book is not null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
