using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Dtos;
using VPark_Models.Dtos.BookingDtos;
using VPark_Models.Dtos.CustomerDtos;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Response<BookingResponseDto>> AddBookingAsync(BookingRequestDto bookingRequestDto, string parkingSpaceId, string email);
        Task<Response<IEnumerable<Booking>>> GetAllBookings();
        Task<Response<Booking>> GetBookingsById(string bookingId);

        //Task<Response<BookingResponseDto>> GetBookingByReferenceAsync(string bookingReference);
        //Task<Response<string>> DeleteBookingAsync(string parkingSpaceId);
        //Task<Response<BookingResponseDto>> EditBookingAsync(BookingRequestDto bookingRequestDto);
    }
}
