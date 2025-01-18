using LibraryApi.Controllers;
using LibraryApi.DTOs;
using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryApi.Tests
{
    public class BookControllerTests
    {
        private readonly Mock<IBookRepository> _mockBookRepository;
        private readonly Mock<IUserRepository> _mockUserRepository;
        private readonly BookController _controller;

        public BookControllerTests()
        {
            // Mocking the repository dependencies
            _mockBookRepository = new Mock<IBookRepository>();
            _mockUserRepository = new Mock<IUserRepository>();

            // Creating the controller with mocked dependencies
            _controller = new BookController(_mockBookRepository.Object, _mockUserRepository.Object);
        }

        [Fact]
        public async Task GetBookById_BookExists_ReturnsOkResult()
        {
            // Arrange
            var bookId = 1;
            var book = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                Description = "Test Description",
                State = BookState.Available,
                Category = BookCategory.Fantasy,
            };
            _mockBookRepository.Setup(repo => repo.GetById(bookId)).ReturnsAsync(book);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBook = Assert.IsType<Book>(okResult.Value);
            Assert.Equal(bookId, returnedBook.Id);
        }

        [Fact]
        public async Task GetBookById_BookDoesNotExist_ReturnsNotFoundResult()
        {
            // Arrange
            var bookId = 11199;
            _mockBookRepository.Setup(repo => repo.GetById(bookId)).ReturnsAsync((Book)null);

            // Act
            var result = await _controller.GetBookById(bookId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
