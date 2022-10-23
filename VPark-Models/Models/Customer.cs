using VPark_Models.Models;

namespace VPark_Models
{
    public class Customer
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}