using System.ComponentModel.DataAnnotations;

namespace LibraryApi.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationId {get; set;}
        [Required]
        public int ReservedBookId {get;set;}
        [Required]
        public int UserId {get; set;}

        public Reservation(int reservationId, int reservedBookId, int userId)
        {
            ReservationId = reservationId;
            ReservedBookId = reservedBookId;
            UserId = userId;
        }
        public Reservation()
        {
            
        }
    }
}