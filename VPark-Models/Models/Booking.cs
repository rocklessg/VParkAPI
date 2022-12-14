using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Models
{
    public class Booking : BaseEntity
    {
        public ServiceType ServiceType { get; set; }
        public string Reference { get; set; } 
        public DateTime Date { get; set; }
        public int DurationOfStay { get; set; }
        [NotMapped]
        public int Duration { get; set; }

        //Nav properties
        public bool PaymentStatus { get; set; } = false;
        public Payment Payment { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string ParkingSpaceId { get; set; }
        public ParkingSpace ParkingSpace { get; set; }

    }

    public enum ServiceType
    {
        Hour = 1, Day = 2
    }
}
