
using LibraryApi.Models;

namespace LibraryApi.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly LibraryDbContext _context;

        public ReservationRepository(LibraryDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Reservation> GetAllReservations() => _context.Reservations.ToList();

        public Reservation? GetReservationById(int reservationId)
        {
            return _context.Reservations.Find(reservationId);
        }
        public IEnumerable<Reservation> GetReservationsByUserId(int userId)
        {
            var result = _context.Reservations
            .Where(reservation => reservation.UserId.Equals(userId))
            .ToList();
            return result;
        }
        public void Add(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChangesAsync();
        }
        public void Update(int reservationId, Reservation updatedReservation)
        {
            var existingReservation = _context.Reservations.Find(reservationId);
            if (existingReservation is not null)
            {
                existingReservation.ReservedBookId = updatedReservation.ReservedBookId;
                existingReservation.UserId = updatedReservation.UserId;
            }
        }
        public void Delete(int reservationId)
        {
            var existingReservation = _context.Reservations.Find(reservationId);
            if (existingReservation is not null)
            {
                _context.Reservations.Remove(existingReservation);
                _context.SaveChangesAsync();
            }
        }
    }   

}