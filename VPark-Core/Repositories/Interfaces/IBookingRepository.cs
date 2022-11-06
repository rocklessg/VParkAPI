using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Dtos;
using VPark_Models.Dtos.BookingDtos;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task<Response<IEnumerable<BookingResponseDto>>> GetBookingAsync(BookingRequestDto bookingRequestDto, string customerId);
        Task<Response<BookingResponseDto>> GetBookingByReferenceAsync(string Bookingeference);
        Task<Response<BookingResponseDto>> AddBookingAsync(BookingRequestDto bookingRequestDto, string parkingSpaceId);
        Task<Response<string>> DeleteBookingAsync(string parkingSpaceId);
        Task<Response<BookingResponseDto>> EditBookingAsync(BookingRequestDto bookingRequestDto);
    }
}
