using ReservationSystem.Models;

namespace ReservationSystem.ViewModels
{
    public class SittingViewModel
    {
        public Reservation Reservation { get; set; }
        public IEnumerable<Table>[] Table { get; set; }
    }
}
