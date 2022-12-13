using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Dtos.ParkingSpaceDto
{
    public class ParkingSpaceUpdateDto : ParkingSpaceRequestDto
    {
        public string Id { get; set; }
    }
}
