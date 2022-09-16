using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem.Models
{
    public class Table
    {
        [Key, Column(Order = 2)]
        [Required]
        public int Id { get; set; }
        [Required]
        public int Seats { get; set; }
        [Key, Column(Order = 1)]
        [Required]
        public AreaEnum Area { get; set; }
        public enum AreaEnum
        {
            Main,
            Outside,
            Balcony
        }
    }
}
