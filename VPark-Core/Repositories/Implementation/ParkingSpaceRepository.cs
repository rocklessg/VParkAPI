using AutoMapper;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Core.Repositories.Interfaces;
using VPark_Data;
using VPark_Models.Dtos;
using VPark_Models.Dtos.ParkingSpaceDto;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Implementation
{
    public class ParkingSpaceRepository : IParkingSpaceRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ParkingSpaceRepository> _logger;
        private readonly IMapper _mapper;

        public ParkingSpaceRepository(AppDbContext context, ILogger<ParkingSpaceRepository> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Response<ParkingSpaceResponseDto>> AddParkingSpace(ParkingSpaceRequestDto newParkingSpace)
        {
            ParkingSpace parkingSpace = _mapper.Map<ParkingSpace>(newParkingSpace);
            //var parkingLot = new ParkingSpace
            //{
            //    Name = newParkingSpace.Name,
            //    IsBooked = newParkingSpace.Isbooked,
            //    CreatedAt = DateTime.UtcNow,
            //    ModifiedAt = DateTime.UtcNow
            //};
            await _context.AddAsync(parkingSpace);
            await _context.SaveChangesAsync();

            return new Response<ParkingSpaceResponseDto> { Succeeded = true, Message = "Parking Space Added Successfully" };
        }

        public async Task<Response<string>> DeleteParkingSpace(string parkingSpaceId)
        {
            var parkingLot = await _context.ParkingSpaces.Where(x => x.Id == parkingSpaceId).FirstOrDefaultAsync(); ;
            if (parkingLot != null)
            {
                _context.ParkingSpaces.Remove(parkingLot);
                await _context.SaveChangesAsync();

                return new Response<string> { Succeeded = true, Message = "Parking Space Deleted Successfully"};
            }
            _logger.LogError(message: "Error in Deleting parking space", parkingSpaceId);
            return new Response<string>() { Succeeded = false, Message = "Parking space not found, please check the Id and try again" };
        }

        public async Task<Response<ParkingSpaceResponseDto>> EditParkingSpaceAsync(ParkingSpaceUpdateDto updateParkingSpace)
        {
            var parkingLot = await _context.ParkingSpaces.Where(x => x.Id == updateParkingSpace.Id).FirstOrDefaultAsync();
            if (parkingLot != null)
            {
                parkingLot.Name = updateParkingSpace.Name;
                parkingLot.IsBooked = updateParkingSpace.Isbooked;
                parkingLot.ModifiedAt = DateTime.UtcNow;

                _context.Update(parkingLot);
                await _context.SaveChangesAsync();

                return new Response<ParkingSpaceResponseDto> { Succeeded = true, Message = "Parking space updated successfully" };
            }
            _logger.LogError(message: "Error in updating parking space", parkingLot);
            return new Response<ParkingSpaceResponseDto> { Succeeded = false, Message = "Unexpected error. Please try again later" };
        }

        public async Task<Response<IEnumerable<ParkingSpace>>> GetAllParkingSpacesAsync()
        {
            var parkingLots = await _context.ParkingSpaces.ToListAsync();
            var response = new Response<IEnumerable<ParkingSpace>>(StatusCodes.Status200OK, true, "List of all Parking spaces", parkingLots);
            return response;
        }

        public async Task<Response<ParkingSpace>> GetParkingSpaceByIdAsync(string id)
        {
            var parkingLot = await _context.ParkingSpaces.Where(x => x.Id == id).FirstOrDefaultAsync();
            if (parkingLot != null)
            {
                return new Response<ParkingSpace> { Succeeded = true, Message = "Get Parking Space Successful", Data = parkingLot, StatusCode = StatusCodes.Status200OK };
            }
            _logger.LogError(message: "Error in Getting parking space by Id", parkingLot);
            return new Response<ParkingSpace>() { Succeeded = false, Message = "Parking space not found, please check the Id and try again" };
        }

        public async Task<Response<IEnumerable<ParkingSpace>>> GetBookedParkingSpacesAsync()
        {
            var parkingLots = await _context.ParkingSpaces.ToListAsync();
            var bookedParkingSpaces = parkingLots.Where(x => x.IsBooked);
            var response = new Response<IEnumerable<ParkingSpace>>(StatusCodes.Status200OK, true, "List of all Booked Parking spaces", bookedParkingSpaces);
            return response;
        }
    }
}
