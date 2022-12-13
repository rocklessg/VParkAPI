using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
using VPark_Models.Dtos.CardDetailsDtos;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;
        private readonly IServiceFee _serviceFee;
        private readonly UserManager<IdentityUser> _userManager;

        public PaymentRepository(AppDbContext context,
            ILogger<PaymentRepository> logger, IServiceFee serviceFee, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _serviceFee = serviceFee;
            _userManager = userManager;
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

        public async Task<Response<CardDetailsDto>> AddCard(CardDetailsDto cards, string appUserId)
        {


            if (appUserId == null)
            {
                _logger.LogError($"Please provide the appUserId");
                return new Response<CardDetailsDto> { Succeeded = false, Message = "user not found" };
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == appUserId);
            if (user == null)
            {
                _logger.LogError($"User with id: {appUserId} is not found in the database");
                return new Response<CardDetailsDto> { Succeeded = false, Message = "Invalid User", StatusCode = StatusCodes.Status400BadRequest };
            }
            else
            {
                var cardDetails = new CardDetails
                {
                    CardNumber = cards.CardNumber,
                    CardType = cards.CardType,
                    CardOwnerName = cards.CardOwnerName,
                    ExpirationDate = cards.ExpirationDate,
                    CVV = cards.CVV,
                    CreatedAt = DateTime.UtcNow,
                    ModifiedAt = DateTime.UtcNow,

                };
                await _context.AddAsync(cardDetails);
                await _context.SaveChangesAsync();
                return new Response<CardDetailsDto> { Succeeded = true, Message = "Card details successfully Added" };

            }

            //try
            //{               
            //    var user = ServicesWrapper.UserManager.Users.FirstOrDefault(x => x.Id == Request.UserId);
            //    if (user == null)
            //    {
            //        response.StatusCode = VestedStatusCode.TransactionFailed;
            //        response.Message = "Invalid UserId";
            //    }
            //    else
            //    {
            //        CardDetailsDto CardDetails = null;
            //        CardDetails = await ServicesWrapper.PaymentGateWay.GetCardDetails(ServicesWrapper, Request);
            //        if (CardDetails != null)
            //        {
            //            var checkUser = ServicesWrapper.AppDbContext.Cards.FirstOrDefault(c => c.AccountName == CardDetails.AccountName && c.LastFourDigits == CardDetails.LastFourDigits && c.UserId == Request.UserId);
            //            if (checkUser == null)
            //            {
            //                var record = new Card
            //                {
            //                    AccountName = CardDetails.AccountName,
            //                    CardType = CardDetails.CardType,
            //                    LastFourDigits = CardDetails.LastFourDigits,
            //                    ExpiryMonth = CardDetails.ExpiryMonth,
            //                    ExpiryYear = CardDetails.ExpiryYear,
            //                    RawResponse = CardDetails.RawResponse,
            //                    AuthorizationCode = CardDetails.AuthorizationCode,
            //                    Reusable = CardDetails.Reusable,
            //                    CreatedAt = DateTime.UtcNow,
            //                    ModifiedAt = DateTime.UtcNow,
            //                    UserId = Request.UserId,
            //                    Reference = Request.Reference
            //                };
            //                ServicesWrapper.AppDbContext.Add(record);
            //                var result = await ServicesWrapper.AppDbContext.SaveChangesAsync();
            //                if (result > 0)
            //                {
            //                    response.StatusCode = VestedStatusCode.Successful;
            //                    response.Message = "Card Added Succesfully";
            //                }
            //                else
            //                {
            //                    response.StatusCode = VestedStatusCode.TransactionFailed;
            //                    response.Message = "No record found";
            //                }
            //            }
            //            else
            //            {
            //                response.StatusCode = VestedStatusCode.TransactionFailed;
            //                response.Message = "Card already added for this User";
            //            }
            //        }
            //        else
            //        {
            //            response.StatusCode = VestedStatusCode.TransactionFailed;
            //            response.Message = "Card Verification failed";
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.ELogError(ex, "Add card");
            //    response.Message = ex.Message;
            //}
            //return response;
        }
        public async Task<Response<IEnumerable<CardDetails>>> GetAllCardsAsync()
        {
            var getAllcards = await _context.CardDetails.ToListAsync();
            return new Response<IEnumerable<CardDetails>>
            {
                Succeeded = true,
                Message = "List of all Cards",
                Data = getAllcards,
                StatusCode = StatusCodes.Status200OK
            };

            //var parkingLots = await _context.ParkingSpaces.ToListAsync();
            //var response = new Response<IEnumerable<ParkingSpace>>(StatusCodes.Status200OK, true, "List of all Parking spaces", parkingLots);
            //return response;
        }

        public async Task<Response<CardDetailsDto>> GetCardByUserId(string cardId)
        {
            var cardsDetails = new CardDetailsDto();
            try
            {
                var result = await _context.CardDetails.Where(x => x.Equals(cardId)).Select(x => new CardDetailsDto
                {
                    Id = x.Id,
                    CardNumber = x.CardNumber,
                    CardOwnerName = x.CardOwnerName,
                    ExpirationDate = x.ExpirationDate,
                    CVV = x.CVV,
                    CardType = x.CardType,

                }).ToListAsync();
                if (result.Count != 0)
                {
                    return new Response<CardDetailsDto> { Succeeded = true, Message = "Card details " };
                }
                else
                {
                    _logger.LogError(message: $"user with userId: {cardId} has no existing card");
                    return new Response<CardDetailsDto> { Succeeded = false, Message = $"User with id:{cardId} has no existing card details" };
                }
            }
            catch (Exception)
            {
                _logger.LogError(message: $"Error Getting card details with userId: {cardId}");
                return new Response<CardDetailsDto> { Message = "user not found", Succeeded = false, StatusCode = StatusCodes.Status404NotFound };
            }
        }

        public async Task<Response<string>> RemoveCard(string cardId)
        {
            var recordToDelete = await _context.CardDetails.FirstOrDefaultAsync(x => x.Equals(cardId));
            if (recordToDelete != null)
            {
                _context.CardDetails.Remove(recordToDelete);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new Response<string>
                    {
                        Message = $"Successfully removed card",
                        Succeeded = true,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new Response<string>
                    {
                        Message = $"Card removal Failed",
                        Succeeded = false,
                    };
                }
            }
            else
            {
                return new Response<string>
                {
                    Message = $"Record Not found",
                    Succeeded = false,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
        }

       

        
    }
}
