using LibraryApi.Models;
public interface IBookRepository{

    Task<IEnumerable<Book>> GetAll();
    Task<Book?> GetById(int id);
    Task<Book?> GetBookByTitleAndAuthor(string title, string author);
    Task<IEnumerable<Book>> GetByCategory(BookCategory category);
    Task<IEnumerable<Book>> GetByAuthor(string author);
    Task Add(Book book);
    Task Update(int id, Book updatedBook);
    Task Delete(int id);
}