using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LibraryApi.Models;
using LibraryApi.DTOs;
using System.Linq;
using System.Threading.Tasks;

//[Authorize]
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

    [HttpGet("Browse")]
    public IActionResult GetBooks()
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet("SearchById/{id}")]
    public IActionResult GetBookById(int id)
    {
        var book = _repository.GetById(id);
        if (book == null) return NotFound();
        return Ok(book);
    }

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

        var existingBook = _repository.GetBookByTitleAndAuthor(newBookDTO.Title, newBookDTO.Author);
        if (existingBook is not null)
        {
            return BadRequest("Book with the same Title and Author already exists.");
        }

        var newBook = new Book
        {
            Title = newBookDTO.Title,
            Author = newBookDTO.Author,
            Description = newBookDTO.Description,
            State = newBookDTO.State,
            Category = newBookDTO.Category
        };

        _repository.Add(newBook);
        return CreatedAtAction(nameof(GetBookById), new { id = newBook.Id }, newBook);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateBook([FromBody] Book updatedBook)
    {
        var currentUser = await _userService.GetMe(User); // Await added here

        if (currentUser == null || currentUser.Role != UserRole.SuperUser)
        {
            return Forbid("You are not allowed to perform this action.");
        }

        var existingBook = _repository.GetById(updatedBook.Id);
        if (existingBook is null) return NotFound();

        _repository.Update(updatedBook.Id, updatedBook);
        return NoContent();
    }

    [HttpDelete("Delete/{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var currentUser = await _userService.GetMe(User); // Await added here

        if (currentUser == null || currentUser.Role != UserRole.SuperUser)
        {
            return Forbid("You are not allowed to perform this action.");
        }

        var existingBook = _repository.GetById(id);
        if (existingBook is null) return NotFound();

        _repository.Delete(id);
        return NoContent();
    }

    [HttpGet("SearchByCategory/{category}")]
    public IActionResult GetBooksByCategory(string category)
    {
        var listOfBooks = _repository.GetByCategory(category);

        int countOfBooks = listOfBooks.Count();

        return Ok(new { ListOfBooks = listOfBooks, Count = countOfBooks });
    }

    [HttpGet("SearchByAuthor/{author}")]
    public IActionResult GetBooksByAuthor(string author)
    {
        var listOfBooks = _repository.GetByAuthor(author);
        return Ok(listOfBooks);
    }
}
