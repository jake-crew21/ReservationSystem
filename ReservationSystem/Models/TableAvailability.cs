using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Models
{
    public class TableAvailability
    {
        [Required]
        public int TableId { get; set; }
        public Table Table { get; set; }
        [Required]
        public DateOnly ResDate { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        public String LastName { get; set; }
        public Reservation Reservation { get; set; }
        public enum StatusEnum
        {
            Available,
            Unavailable
        }
    }
}
