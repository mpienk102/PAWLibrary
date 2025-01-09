using LibraryApi.Models;
public interface IBookRepository{

    IEnumerable<Book> GetAll();
    Book? GetById(int id);
    Book? GetBookByTitleAndAuthor(string title, string author);
    IEnumerable<Book> GetByCategory(string category);
    IEnumerable<Book> GetByAuthor(string author);
    void Add(Book book);
    void Update(int id, Book updatedBook);
    void Delete(int id);
}