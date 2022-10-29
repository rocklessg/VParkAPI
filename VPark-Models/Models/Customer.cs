using System.ComponentModel.DataAnnotations;
using VPark_Models.Models;

namespace VPark_Models
{
    public class Customer : BaseEntity
    {
        [Key]
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }
}