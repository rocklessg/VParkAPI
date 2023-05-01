using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Models;

namespace VPark_Models.Dtos.BookingDtos
{
    public class BookingRequestDto
    {
        public ServiceType ServiceType { get; set; }
        public int DurationOfStay { get; set; }
    }
}
