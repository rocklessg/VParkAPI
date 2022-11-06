using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Models;

namespace VPark_Models.Dtos.BookingDtos
{
    public class BookingResponseDto
    {
        public ServiceType ServiceType { get; set; }
        public string Reference { get; set; }
        public DateTime Date { get; set; }
        public DateTime Duration { get; set; }
        public bool PaymentStatus { get; set; }
        public ParkingSpace ParkingSpace { get; set; }
    }
}
