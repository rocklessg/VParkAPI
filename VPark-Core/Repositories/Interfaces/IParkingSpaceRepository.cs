using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Dtos;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IParkingSpaceRepository
    {
        Task<Response<IEnumerable<ParkingSpace>>> GetAllParkingSpacesAsync();
        Task<Response<ParkingSpace>> GetParkingSpaceByIdAsync(string id);
        Task<Response<ParkingSpaceDto>> AddParkingSpace(ParkingSpaceDto newParkingSpace);
        Task<Response<string>> DeleteParkingSpace(string parkingSpaceId);
        Task<Response<ParkingSpaceDto>> EditParkingSpaceAsync(ParkingSpaceDto updateParkingSpace);
    }

}
