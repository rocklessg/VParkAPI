using System;
using System.Collections.Generic;
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
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        //Nav properties
        public bool PaymentStatus { get; set; }
        public Payment Payment { get; set; }       
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string ParkingSpaceId { get; set; }
        public ParkingSpace ParkingSpace { get; set; }

    }
}

namespace VPark_Models
{
    public enum ServiceType
    {
        hour = 1, Day = 2
    }
}