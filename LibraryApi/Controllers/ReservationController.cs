using System.Security.Claims;
using LibraryApi.DTOs;
using LibraryApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//[Authorize]
[ApiController]
[Route("api/Reservations")] 
public class ReservationController : ControllerBase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IBookRepository _bookRepository;
    private readonly IUserService _userService;

    
    public ReservationController(IReservationRepository reservationRepository, IBookRepository bookRepository, IUserService userService)
    {
        _reservationRepository = reservationRepository;
        _bookRepository = bookRepository;
        _userService = userService;
    }

    [HttpGet("Browse")] 
    public IActionResult GetAllReservations()
    {
        return Ok(_reservationRepository.GetAllReservations());
    }

    [HttpGet("SearchById/{reservationId:int}")]
    public IActionResult GetReservationById(int reservationId)
    {
        var reservation = _reservationRepository.GetReservationById(reservationId);
        if (reservation is null) 
        {
            return NotFound();
        }
        return Ok(reservation);
    }
    //[Authorize]
    [HttpPost("ReserveBook/{bookId:int}")]
    public IActionResult ReserveBookById(int bookId)
    {

        var book = _bookRepository.GetById(bookId);
        var currentUser = _userService.GetMe(User);
        if (currentUser is null)
        {
            return BadRequest("You`re not log in");
        }
        if(book is null)
        {
            return NotFound(new{message = "Book not found."});
        }
        if(book.State == BookState.Reserved || book.State == BookState.Unavailable)
        {
            return NotFound(new{message = "This book is currently unavailable"});
        }
        //var userId = 111;
        var reservation = new Reservation
        {
            ReservedBookId = bookId,
            UserId = currentUser.Id
        };
        book.State = BookState.Reserved;
        _bookRepository.Update(bookId, book);
        _reservationRepository.Add(reservation);

        return CreatedAtAction(nameof(GetReservationById), new {reservationId = reservation.ReservationId}, reservation);
    }
    [HttpDelete("ReturnBook/{reservationId:int}")]
    public IActionResult ReturnBookById(int reservationId)
    {
        var reservation = _reservationRepository.GetReservationById(reservationId);
        if(reservation is null)
        {
            return NotFound(new {message = "Reservation not found."});
        }

        var book = _bookRepository.GetById(reservation.ReservedBookId);
        if(book is null)
        {
            return NotFound(new{message = "Book not found."});
        }
        book.State = BookState.Available;
        _reservationRepository.Delete(reservationId);
        return Ok(new{message = "Book returned succesfully."});
    }
    [HttpGet("GetUserReservation/{userId}")]
    public IActionResult GetUserReservations(int userId)
    {
        var listOfReservations = _reservationRepository.GetReservationsByUserId(userId);
        
        var reservationsWithBookDetails = listOfReservations
            .Select(reservation =>
            {
                var book = _bookRepository.GetById(reservation.ReservedBookId);
                if (book != null)
                {
                    return new ReservationDetailDTO
                    {
                        ReservationId = reservation.ReservationId,
                        Title = book.Title,
                        Author = book.Author
                    };
                }
                return null;
            })
            .ToList();
        return Ok(reservationsWithBookDetails);
    }


}
