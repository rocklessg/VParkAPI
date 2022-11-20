using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPark_Core.Repositories.Interfaces;
using VPark_Models.Dtos.BookingDtos;
using VPark_Models.Dtos.CustomerDtos;
using VPark_Models.Models;

namespace VPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingRepository _bookingRepo;
       
        public BookingController(IBookingRepository bookingRepo)
        {
            _bookingRepo = bookingRepo;           
        }

        [HttpPost("Add-Booking")]
        public async Task<IActionResult> AddBookingAsync(BookingRequestDto bookingRequestDto,string parkingSpaceId,string email)
        {
            var response = await _bookingRepo.AddBookingAsync(bookingRequestDto, parkingSpaceId, email);
            return StatusCode(response.StatusCode, response);
        }
    }
}
