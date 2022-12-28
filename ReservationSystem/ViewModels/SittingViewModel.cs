using ReservationSystem.Models;

namespace ReservationSystem.ViewModels
{
    public class SittingViewModel
    {
        public Sitting Sitting { get; set; }
        public IEnumerable<Reservation> Reservation { get; set; }
        public IEnumerable<Table> Table { get; set; }
    }
}
