using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Core.Repositories.Interfaces;
using VPark_Data;
using VPark_Helper;
using VPark_Models;
using VPark_Models.Dtos;
using VPark_Models.Dtos.BookingDtos;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;
        private readonly IServiceFee _serviceFee;

        public PaymentRepository(AppDbContext context, ILogger<PaymentRepository> logger, IServiceFee serviceFee)
        {
            _context = context;
            _logger = logger;
            _serviceFee = serviceFee;
        }

        public async Task<Response<PaymentDto>> AddPayment(PaymentDto paymentDto, string bookingId)
        {
            if (bookingId == null)
            {
                _logger.LogError("Invalid bookingId", nameof(AddPayment));
                return new Response<PaymentDto> { Succeeded = false, Message = "Booking not found" };
            }

            var getBooking = _context.Bookings.FirstOrDefault(x => x.Id == bookingId);
            if (getBooking == null) { return new Response<PaymentDto> { Succeeded = false, Message = "booking not found" }; }

            string paymentMethodAcronym = string.Empty;

            if (paymentDto.PaymentMethod == (PaymentMethod)PaymentMethodNameAcronym.CP)
            {
                paymentMethodAcronym = HelperCodeGenerator.GenerateTransactionReference(PaymentMethodNameAcronym.CP.ToString());
            }
            else
            {
                paymentMethodAcronym = HelperCodeGenerator.GenerateTransactionReference(PaymentMethodNameAcronym.BT.ToString());
            }

            var servicePaymentAmount = string.Empty;

            if (getBooking.ServiceType.Equals(ServiceType.Hour))
            {
                servicePaymentAmount = _serviceFee.GetParkingSpaceFee(getBooking.ServiceType, getBooking.DurationOfStay);
                //servicePaymentAmount = ServiceFee.GetParkingSpaceFee(getBooking.ServiceType, getBooking.DurationOfStay);
            }
            else
            {
                servicePaymentAmount = _serviceFee.GetParkingSpaceFee(ServiceType.Day, getBooking.DurationOfStay);
                //servicePaymentAmount = ServiceFee.GetParkingSpaceFee(ServiceType.Day, getBooking.DurationOfStay);
            }

            var payment = new Payment
            {
                PaymentReference = paymentMethodAcronym,
                Amount = decimal.Parse(servicePaymentAmount),
                Status = Status.Pending,
                PaymentMethod = paymentDto.PaymentMethod,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                BookingId = getBooking.Id
            };
            await _context.AddAsync(payment);
            await _context.SaveChangesAsync();

            /*   Implement card payment logic here && Bank transfer payment logic
                 from here, we check the payment status from NIP or MiFOS (Or any payment get way we used)
                 if the response status is successful, we will update the
                 payment status from pending to success,
                 we will update the PaymentStatus in booking table to true,
                 and we will update the parkingSpace IsBooked to true
            */


            // Card payment simulation for Api testing

            if (payment.PaymentMethod == PaymentMethod.Card)
            {
                var parkingSpaceToBook = _context.ParkingSpaces.FirstOrDefault(x => x.Id == getBooking.ParkingSpaceId);
                if (parkingSpaceToBook == null)
                {
                    _logger.LogInformation("Parking Space not found", nameof(parkingSpaceToBook));
                    return new Response<PaymentDto> { Succeeded = false, Message = "Invalid ParkingSpaceId", StatusCode = StatusCodes.Status404NotFound };
                }

                payment.Status = Status.Success;
                payment.UpdatedAt = DateTime.UtcNow;
                _context.Update(payment);

                parkingSpaceToBook.IsBooked = true;
                parkingSpaceToBook.ModifiedAt = DateTime.UtcNow;
                _context.Update(parkingSpaceToBook);

                getBooking.PaymentStatus = true;
                getBooking.ModifiedAt = DateTime.UtcNow;
                _context.Update(getBooking);

                await _context.SaveChangesAsync();

                return new Response<PaymentDto> { Succeeded = true, Message = "Payment Successful", Data = paymentDto, StatusCode = 00 };
            }

            return new Response<PaymentDto> { Succeeded = false, Message = "Please try Card payment", Data = paymentDto, StatusCode = 99 };


        }
    }
}
