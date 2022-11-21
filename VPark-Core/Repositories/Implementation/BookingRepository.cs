﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Core.Repositories.Interfaces;
using VPark_Data;
using VPark_Helper;
using VPark_Models;
using VPark_Models.Dtos;
using VPark_Models.Dtos.BookingDtos;
using VPark_Models.Dtos.CustomerDtos;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Implementation
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<BookingRepository> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public BookingRepository(AppDbContext context, ILogger<BookingRepository> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<Response<BookingResponseDto>> AddBookingAsync(BookingRequestDto bookingRequestDto, string parkingSpaceId, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var parkingSpaceToBook = _context.ParkingSpaces.FirstOrDefault(x => x.Id == parkingSpaceId);
            if (parkingSpaceToBook == null || parkingSpaceToBook.IsBooked == true)
            {
                _logger.LogInformation("Parking Space booked or not found", nameof(parkingSpaceToBook));
                return new Response<BookingResponseDto> { Succeeded = false, Message = "Parking Space Not available", StatusCode = StatusCodes.Status404NotFound };
            }

            var generatedBookingReference = HelperCodeGenerator.GenerateBookingReference("BKN");

            var booking = new Booking
            {
                ServiceType = bookingRequestDto.ServiceType,
                Date = DateTime.UtcNow.Date,
                //DurationOfStay = bookingRequestDto.,
                PaymentStatus = false,
                Reference = generatedBookingReference,
                ParkingSpaceId = parkingSpaceId,
                CustomerId = user.Id,
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow
            };


            await _context.AddAsync(booking);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Booking parking space in progress", nameof(booking));
            return new Response<BookingResponseDto> { Succeeded = false, Message = "Proceed to make payment" };
        }

        public Task<Response<IEnumerable<BookingResponseDto>>> GetBookingAsync(string customerId)
        {
            throw new NotImplementedException();
        }

    }
}