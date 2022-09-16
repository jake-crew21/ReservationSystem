using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ReservationSystem.Models
{
    public class AccountType
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [DisplayName(displayName: "MemberShip Type")]
        public string Name { get; set; }
    }
}
