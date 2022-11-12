using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using VPark_Core.Repositories.Interfaces;
using VPark_Models.Dtos;
using VPark_Models.Models;

namespace VPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkingSpaceController : ControllerBase
    {
        private readonly IParkingSpaceRepository _parkingSpaceRepository;
        private readonly ILogger<ParkingSpaceController> _logger;

        public ParkingSpaceController(IParkingSpaceRepository parkingSpaceRepository, ILogger<ParkingSpaceController> logger)
        {
            _parkingSpaceRepository = parkingSpaceRepository;
            _logger = logger;
        }

        [SwaggerOperation(Summary = "Description: This endpoint gets all the parking space registered on the app")]
        //[Authorize]
        [HttpGet("All-ParkingSpace")]
        public async Task<IActionResult> GetAllParkingSpacesAsync()
        {
            var response = await _parkingSpaceRepository.GetAllParkingSpacesAsync();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}", Name = "GetParkingSpaceByIdAsync")]
        public async Task<IActionResult> GetParkingSpaceByIdAsync(string id)
        {
            var response = await _parkingSpaceRepository.GetParkingSpaceByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("Add-ParkingSpace")]
        public async Task<IActionResult> AddParkingSpace([FromBody] ParkingSpaceDto payload)
        {
            var response = await _parkingSpaceRepository.AddParkingSpace(payload);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditParkingSpaceAsync([FromBody] ParkingSpaceDto payload)
        {
            var response = await _parkingSpaceRepository.EditParkingSpaceAsync(payload);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete()]
        public async Task<IActionResult> DeleteParkingSpace(string parkingSpaceId)
        {
            var response = await _parkingSpaceRepository.DeleteParkingSpace(parkingSpaceId);
            return StatusCode(response.StatusCode, response);
        }

    }
}
