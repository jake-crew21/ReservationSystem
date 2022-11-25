using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem.Models
{
    public class SittingSchedule
    {
        [Required, DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Required, DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [Required, DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }
        [Required, DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
        [Required]
        public int Capacity { get; set; }
        [Required]
        public SessionEnum SessionType { get; set; }
        public enum SessionEnum
        {
            Breakfast,
            Lunch,
            Dinner,
            SpecialEvent
        }
    }
}
