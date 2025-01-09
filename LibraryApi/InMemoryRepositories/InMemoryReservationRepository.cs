using LibraryApi.Models;

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly List<Reservation> _reservations = new(){
        new Reservation(1,1,1),
        new Reservation(2,2,1)
    };
    public IEnumerable<Reservation> GetAllReservations() => _reservations;
    public Reservation? GetReservationById(int reservationId)
    {
        return _reservations.FirstOrDefault(r => r.ReservationId == reservationId);
    }
    public IEnumerable<Reservation> GetReservationsByUserId(int userId)
    {
        var result = _reservations.Where(reservation => reservation.UserId.Equals(userId));
        return result;
    }

    public void Add(Reservation reservation)
    {
        var newId = _reservations.Any() ? _reservations.Max(i => i.ReservationId) +1 : 1;
        _reservations.Add(new Reservation
        {
            ReservationId = newId,
            ReservedBookId = reservation.ReservedBookId,
            UserId = reservation.UserId
        });
    }
    public void Update(int reservationId, Reservation updatedReservation)
    {
        var reservationIndex = _reservations.FindIndex(i => i.ReservationId == reservationId);
        if (reservationIndex != -1)
        {
            var reservation = _reservations[reservationIndex];
            reservation.ReservationId = updatedReservation.ReservationId;
            reservation.ReservedBookId = updatedReservation.ReservedBookId;
            reservation.UserId = updatedReservation.UserId;
        }
    }
    public void Delete(int reservationId)
    {
        var reservation = GetReservationById(reservationId);
        if (reservation is not null)
        {
            _reservations.Remove(reservation);
        }
    }
}