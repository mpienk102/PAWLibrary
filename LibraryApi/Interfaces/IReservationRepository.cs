using LibraryApi.Models;

namespace LibraryApi.Interfaces
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllReservations();

        Task<Reservation?> GetReservationById(int reservationId);

        Task<IEnumerable<Reservation>> GetReservationsByUserId(int userId);

        Task<IEnumerable<Book>> GetBooksByIds(IEnumerable<int> bookIds);

        Task Add(Reservation reservation);

        Task Update(int reservationId, Reservation updatedReservation);

        Task Delete(int reservationId);
    }
}
