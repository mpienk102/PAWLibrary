using LibraryApi.Models;

public interface IReservationRepository
{
    IEnumerable<Reservation> GetAllReservations();
    Reservation? GetReservationById(int reservationId);
    IEnumerable<Reservation> GetReservationsByUserId(int userId);
    void Add (Reservation reservation);
    void Update(int reservationId, Reservation updatedReservation);
    void Delete(int reservationId);
}