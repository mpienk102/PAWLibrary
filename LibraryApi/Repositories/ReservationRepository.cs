using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly LibraryDbContext _context;

        public ReservationRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reservation>> GetAllReservations() => await _context.Reservations.ToListAsync();

        public async Task<Reservation?> GetReservationById(int reservationId)
        {
            return await _context.Reservations.FindAsync(reservationId);
        }

        public async Task<IEnumerable<Reservation>> GetReservationsByUserId(int userId)
        {
            var result = await _context.Reservations
            .Where(reservation => reservation.UserId.Equals(userId))
            .ToListAsync();
            return result;
        }

        public async Task<IEnumerable<Book>> GetBooksByIds(IEnumerable<int> bookIds)
        {
            return await _context.Books
                .Where(book => bookIds.Contains(book.Id))
                .ToListAsync();
        }

        public async Task Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();
        }

        public async Task Update(int reservationId, Reservation updatedReservation)
        {
            var existingReservation = _context.Reservations.Find(reservationId);
            if (existingReservation is not null)
            {
                existingReservation.ReservedBookId = updatedReservation.ReservedBookId;
                existingReservation.UserId = updatedReservation.UserId;
            }
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int reservationId)
        {
            var existingReservation = _context.Reservations.Find(reservationId);
            if (existingReservation is not null)
            {
                _context.Reservations.Remove(existingReservation);
                await _context.SaveChangesAsync();
            }
        }
    }
}