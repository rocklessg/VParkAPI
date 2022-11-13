using System.ComponentModel.DataAnnotations;
using VPark_Models.Models;

namespace VPark_Models
{
    public class Customer
    {
        [Key]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}