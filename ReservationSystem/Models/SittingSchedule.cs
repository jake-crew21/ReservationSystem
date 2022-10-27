using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Models
{
    public class SittingSchedule
    {
        [Required]
        public DateOnly StartDate { get; set; }
        [Required]
        public DateOnly EndDate { get; set; }
        [Required]
        public TimeOnly StartTime { get; set; }
        [Required]
        public TimeOnly EndTime { get; set; }
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
        public DayOfWeek DayOfWeek { get; set; }
    }
}
