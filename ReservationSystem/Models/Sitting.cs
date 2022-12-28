using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem.Models
{
    public class Sitting
    {
        [Key, Column(Order = 1)]
        [Required]
        public int TableId { get; set; }
        public Table Table { get; set; }
        [Key, Column(Order = 2)]
        [Required]
        public int BookingId { get; set; }
        public Reservation Reservation { get; set; }
    }
}
