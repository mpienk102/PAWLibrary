using LibraryApi.DTOs;
using LibraryApi.Interfaces;
using LibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/Reservations")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IUserRepository _userService;

        public ReservationController(IReservationRepository reservationRepository, IBookRepository bookRepository, IUserRepository userService)
        {
            _reservationRepository = reservationRepository;
            _bookRepository = bookRepository;
            _userService = userService;
        }

        /// <summary>
        /// Return a list of all reservations.
        /// </summary>
        /// <returns>List of reservations.</returns>
        [HttpGet("Browse")]
        public async Task<IActionResult> GetAllReservations()
        {
            var reservations = await _reservationRepository.GetAllReservations();
            return Ok(reservations);
        }

        /// <summary>
        /// Return a reservation by its Id.
        /// </summary>
        /// <param name="reservationId"> ReservationID</param>
        /// <returns>Reservation with given reservationId.</returns>
        [HttpGet("SearchById/{reservationId:int}")]
        public async Task<IActionResult> GetReservationById(int reservationId)
        {
            var reservation = await _reservationRepository.GetReservationById(reservationId);
            if (reservation is null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        /// <summary>
        /// Reserve a book with given id.
        /// </summary>
        /// <param name="bookId">Specific bookId.</param>
        /// <returns>New reservation or Code 400 on Exception.</returns>
        [Authorize]
        [HttpPost("ReserveBook/{bookId:int}")]
        public async Task<IActionResult> ReserveBookById(int bookId)
        {
            var book = await _bookRepository.GetById(bookId);
            var currentUser = await _userService.GetMe(User);
            if (currentUser is null)
            {
                return BadRequest("You`re not log in");
            }

            if (book is null)
            {
                return NotFound(new { message = "Book not found." });
            }

            if (book.State == BookState.Reserved || book.State == BookState.Unavailable)
            {
                return NotFound(new { message = "This book is currently unavailable" });
            }

            var reservation = new Reservation
            {
                ReservedBookId = bookId,
                UserId = currentUser.Id,
            };

            book.State = BookState.Reserved;
            await _bookRepository.Update(bookId, book);
            await _reservationRepository.Add(reservation);

            return CreatedAtAction(nameof(GetReservationById), new { reservationId = reservation.ReservationId }, reservation);
        }

        /// <summary>
        /// Return a book (delete reservation)
        /// </summary>
        /// <param name="reservationId">Id of the reservation.</param>
        /// <returns>Code 200 and success message.</returns>
        [HttpDelete("ReturnBook/{reservationId:int}")]
        public async Task<IActionResult> ReturnBookById(int reservationId)
        {
            var reservation = await _reservationRepository.GetReservationById(reservationId);
            if (reservation is null)
            {
                return NotFound(new { message = "Reservation not found." });
            }

            var book = await _bookRepository.GetById(reservation.ReservedBookId);
            if (book is null)
            {
                return NotFound(new { message = "Book not found." });
            }

            book.State = BookState.Available;
            await _reservationRepository.Delete(reservationId);
            return Ok(new { message = "Book returned succesfully." });
        }

        /// <summary>
        /// Search for reservations assigned to specific user.
        /// </summary>
        /// <param name="userId">Unique user ID.</param>
        /// <returns>List of reservations assigned to specific user.</returns>
        [HttpGet("GetUserReservation/{userId}")]
        public async Task<IActionResult> GetUserReservations(int userId)
        {
            var listOfReservations = await _reservationRepository.GetReservationsByUserId(userId);

            var bookIds = listOfReservations
                .Select(reservation => reservation.ReservedBookId)
                .Distinct()
                .ToList();

            var books = await _reservationRepository.GetBooksByIds(bookIds);

            var bookLookup = books.ToDictionary(book => book.Id);

            var reservationsWithBookDetails = listOfReservations
                .Select(reservation =>
                {
                    if (bookLookup.TryGetValue(reservation.ReservedBookId, out var book))
                    {
                        return new ReservationDetailDTO
                        {
                            ReservationId = reservation.ReservationId,
                            Title = book.Title,
                            Author = book.Author,
                        };
                    }

                    return null;
                })
                .Where(dto => dto != null)
                .ToList();

            return Ok(reservationsWithBookDetails);
        }
    }
}