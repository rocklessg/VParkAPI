using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPark_Core.Repositories.Interfaces;
using VPark_Models.Dtos.BookingDtos;
using VPark_Models.Dtos.CustomerDtos;

namespace VPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly ILogger<BookingController> _logger;
        public BookingController(IBookingRepository bookingRepo, ILogger<BookingController> logger)
        {
            _bookingRepo = bookingRepo;
            _logger = logger;
        }

        [HttpPost("Add-Booking")]
        public async Task<IActionResult> AddBookingAsync([FromQuery] BookingRequestDto bookingRequestDto, string ParkingSpaceId, CustomerDto customerDto)
        {
            var response = await _bookingRepo.AddBookingAsync(bookingRequestDto, ParkingSpaceId, customerDto);
            return StatusCode(response.StatusCode, response);
        }
    }
}
