using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReservationSystem.Models
{
    public class Table
    {
        //[Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public int Seats { get; set; }
        [Required]
        public AreaEnum Area { get; set; }
        public enum AreaEnum
        {
            Main,
            Outside,
            Balcony
        }
        [Required]
        public string TableNum { get; set; }
        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; set; }
    }
}
