using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem.Models
{
    public class Reservation
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Id { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        public string Contact { get; set; }
        [Required]
        public int NoOfPpl { get; set; }
        public int NoOfTable { get; set; }
        [Key, Column(Order=2)]
        [Required]
        public DateOnly ResDate { get; set; }
        [Key,Column(Order=3)]
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
        [Required]
        public int Duration { get; set; }
        public string Notes { get; set; }
        public string Source { get; set; }
        [Required]
        public StatusEnum BookingStatus { get; set; }
        public enum StatusEnum
        {
            Pending,
            Confirmed,
            Cancelled,
            Rejected,
            Seated,
            Completed,
            Requested
        }
        [Required]
        public SessionEnum SessionType { get; set; }
        public enum SessionEnum
        {
            Breakfast,
            Lunch,
            Dinner,
            SpecialEvent
        }
        [Required]
        public  AreaEnum Area { get; set; }
        public enum AreaEnum
        {
            Main,
            Outside,
            balcony
        }
    }
}
