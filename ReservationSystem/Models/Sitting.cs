using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem.Models
{
    public class Sitting
    {
        [Key, Column(Order=1)]
        [Required]
        public int TableId { get; set; }
        public Table Table { get; set; }
        [Required]
        public TStatusEnum Status { get; set; }
        public enum TStatusEnum
        {
            Open,
            Reserved,
            Closed
        }
        [Key, Column(Order=2)]
        [Required]
        public string Contact { get; set; }
        [Key, Column(Order=3)]
        [Required]
        public DateOnly ResDate { get; set; }
        [Key,Column(Order=4)]
        [Required]
        public TimeOnly StartTime { get; set; }
        public Reservation Reservation { get; set; }
    }
}
