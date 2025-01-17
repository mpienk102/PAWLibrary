using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;

/// <summary>
/// Handles book-related operations.
/// </summary>
 // [Authorize]
[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookRepository _repository;
    private readonly IUserService _userService;

    public BookController(IBookRepository bookRepository, IUserService userService)
    {
        _repository = bookRepository;
        _userService = userService;
    }

    /// <summary>
    /// Return a list of all books.
    /// </summary>
    /// <returns>A list of books.</returns>
    [HttpGet("Browse")]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _repository.GetAll();
        return Ok(books);
    }

    /// <summary>
    /// Return a book by its ID.
    /// </summary>
    /// <param name="id">The unique ID of the book.</param>
    /// <returns>The book with the given ID, or a 404 if not found.</returns>
    [HttpGet("SearchById/{id}")]
    public async Task<IActionResult> GetBookById(int id)
    {
        var book = await _repository.GetById(id);
        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    /// <summary>
    /// Create a book.
    /// </summary>
    /// <param name="newBookDTO">DTO read from body.</param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDTO newBookDTO)
    {
        var currentUser = await _userService.GetMe(User);

        if (currentUser == null || currentUser.Role != UserRole.SuperUser)
        {
            return Forbid("You are not allowed to perform this action.");
        }

        if (newBookDTO is null)
        {
            return BadRequest("Book data is required.");
        }

        var existingBook = await _repository.GetBookByTitleAndAuthor(newBookDTO.Title, newBookDTO.Author);
        if (existingBook is not null)
        {
            return BadRequest($"Book with Title: '{newBookDTO.Title}' and Author: '{newBookDTO.Author}' already exists.");
        }

        var newBook = new Book
        {
            Title = newBookDTO.Title,
            Author = newBookDTO.Author,
            Description = newBookDTO.Description,
            State = newBookDTO.State,
            Category = newBookDTO.Category,
        };

        await _repository.Add(newBook);
        return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
    }

    /// <summary>
    /// Update Book Data
    /// </summary>
    /// <param name="updatedBook"></param>
    /// <returns>Code 200 or Forbid on bad permission.</returns>
    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] Book updatedBook)
    {
        var currentUser = await _userService.GetMe(User);

        if (currentUser == null || currentUser.Role != UserRole.SuperUser)
        {
            return Forbid("You are not allowed to perform this action.");
        }

        var existingBook = await _repository.GetById(updatedBook.Id);
        if (existingBook is null)
        {
            return NotFound("Book with given id was not found.");
        }

        await _repository.Update(updatedBook.Id, updatedBook);
        return NoContent();
    }

    /// <summary>
    /// Delete Book by it`s Id
    /// </summary>
    /// <param name="id">Book id</param>
    /// <returns>Code 200 or Forbid on bad permission.</returns>
    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var currentUser = await _userService.GetMe(User);

        if (currentUser == null || currentUser.Role != UserRole.SuperUser)
        {
            return Forbid("You are not allowed to perform this action.");
        }

        var existingBook = await _repository.GetById(id);
        if (existingBook is null)
        {
            return NotFound("Book with given Id was not found.");
        }

        await _repository.Delete(id);
        return NoContent();
    }

    /// <summary>
    /// Searches books by a category.
    /// </summary>
    /// <param name="category">The category to search for.</param>
    /// <remarks>This method will be deprecated in the next release. Use `/Books/AdvancedSearch` instead.</remarks>
    /// <returns>A list of books in the given category.</returns>
    [HttpGet("SearchByCategory/{category}")]
    public async Task<IActionResult> GetBooksByCategory(BookCategory category)
    {
        var listOfBooks = await _repository.GetByCategory(category);

        int countOfBooks = listOfBooks.Count();

        return Ok(new { ListOfBooks = listOfBooks, Count = countOfBooks });
    }

    /// <summary>
    /// Retrieves books by an author.
    /// </summary>
    /// <param name="author">The author's name.</param>
    /// <returns>A list of books written by the specified author.</returns>
    [HttpGet("SearchByAuthor/{author}")]
    public async Task<IActionResult> GetBooksByAuthor(string author)
    {
        var listOfBooks = await _repository.GetByAuthor(author);
        return Ok(listOfBooks);
    }
}
