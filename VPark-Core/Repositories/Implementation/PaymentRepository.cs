using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayStack.Net;
using VPark_Core.Repositories.Interfaces;
using VPark_Data;
using VPark_Helper;
using VPark_Helper.Request;
using VPark_Models;
using VPark_Models.Dtos;
using VPark_Models.Dtos.CardDetailsDtos;
using VPark_Models.Dtos.PaystackDto;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaymentRepository> _logger;
        private readonly IServiceFee _serviceFee;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private readonly string token;
        private readonly IHttpServices _httpService;
        private readonly IPaystackRepository _paystackRepository;


        private PayStackApi PayStack { get; set; }


        public PaymentRepository(AppDbContext context,
            ILogger<PaymentRepository> logger, IServiceFee serviceFee, UserManager<IdentityUser> userManager, IMapper mapper, IConfiguration config, IHttpServices httpService, IPaystackRepository paystackRepository)
        {
            _context = context;
            _logger = logger;
            _serviceFee = serviceFee;
            _userManager = userManager;
            _mapper = mapper;
            _config = config;
            token = _config["Paystack: SecretKey"];
            PayStack = new PayStackApi(token);
            _httpService = httpService;
            _paystackRepository = paystackRepository;
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
                    return new Response<PaymentDto> { Succeeded = false, Message = "Invalid ParkingSpaceId" };
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
        public async Task<Response<CardAuthorizeResponseDto>> AddCard(AuthorizeCardDto cards, string appUserId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == appUserId);
            if (user == null)
            {
                _logger.LogError($"{nameof(AddCard)} USER WITH id: {appUserId} NOT FOUND IN THE DATABASE AT: {DateTime.Now}");
                return new Response<CardAuthorizeResponseDto> { Succeeded = false, Message = "Invalid User" };
            }
            else
            {
                _logger.LogInformation($"{nameof(AddCard)} ATTEMPTING TO MAP USER DTO TO THE MODELS AT: {DateTime.Now}");
                CardDetails cardDetails = _mapper.Map<CardDetails>(cards);
                await _context.AddAsync(cardDetails);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"{nameof(AddCard)} USER CARD DETAILS SUCCESSFULLY SAVED TO DATABASE AT: {DateTime.Now}");
                return new Response<CardAuthorizeResponseDto> { Succeeded = true, Message = "Card details successfully Added" };
            }
        }
        public async Task<Response<IEnumerable<CardDetails>>> GetAllCardsAsync()
        {
            _logger.LogInformation($"{nameof(GetAllCardsAsync)} ATTEMPTING TO GET ALL CARDS FROM THE DATABASE AT: {DateTime.Now}");
            var getAllcards = await _context.CardDetails.ToListAsync();
            _logger.LogInformation($"{nameof(GetAllCardsAsync)} ALL USERS CARDS SUCCESSFULLY RETRIEVED FROM THE DATABASE AT: {DateTime.Now}");
            return new Response<IEnumerable<CardDetails>>
            {
                Succeeded = true,
                Message = "List of all Cards",
                Data = getAllcards,

            };
        }
        public async Task<Response<CardDetails>> GetCardByUserId(string cardId)
        {
            _logger.LogInformation($"{nameof(GetCardByUserId)} ATTEMPTING TO GET USER BY id:{cardId}AT : {DateTime.Now}");
            var cardDetails = await _context.CardDetails.Where(x => x.Id == cardId).FirstOrDefaultAsync();
            if (cardDetails != null)
            {
                _logger.LogInformation($"{nameof(GetCardByUserId)} RETURNING CARD FOR USER WITH id:{cardId} FROM THE DATABASE AT: {DateTime.Now}");
                return new Response<CardDetails> { Succeeded = true, Message = "Card details " };
            }
            else
            {
                _logger.LogError($"{nameof(GetCardByUserId)} FAILED TURN CARD FROM THE DATABASE AT : {DateTime.Now}");
                return new Response<CardDetails> { Succeeded = false, Message = $"User with id:{cardId} has no existing card details" };
            }
        }
        public async Task<Response<string>> RemoveCard(string cardId)
        {
            _logger.LogInformation($"{nameof(RemoveCard)} ATTEMPTING TO SEARCH DB FOR CARD TO BE DELETED AT: {DateTime.Now}");
            var recordToDelete = await _context.CardDetails.FirstOrDefaultAsync(x => x.Id == cardId);
            if (recordToDelete != null)
            {
                _logger.LogInformation($"{nameof(RemoveCard)} CARD TO BE DELETED SUCCESSFULLY RETRIEVED CARD FROM THE DATABASE AT: {DateTime.Now}");
                _context.CardDetails.Remove(recordToDelete);
                _logger.LogInformation($"{nameof(RemoveCard)} SUCCESSFULLY REMOVED CARD FROM THE DATABASE AT: {DateTime.Now}");
                var result = await _context.SaveChangesAsync();

                if (result > 0)
                {
                    _logger.LogInformation($"{nameof(RemoveCard)} SUCCESSFULLY UPDATED THE DATABASE OF THE CARD REMOVED AT: {DateTime.Now}");
                    return new Response<string>
                    {
                        Message = $"Successfully removed card",
                        Succeeded = true,

                    };
                }
                else
                {
                    _logger.LogError($"{nameof(RemoveCard)} FAILED TO REMOVE CARD, NO CHANGES WAS MADE TO THE DATABASE AT: {DateTime.Now}");
                    return new Response<string>
                    {
                        Message = $"Card removal Failed",
                        Succeeded = false,
                    };
                }
            }
            else
            {
                _logger.LogError($"{nameof(RemoveCard)} CARD DOES NOT EXIST IN THE DATABASE AT: {DateTime.Now}");
                return new Response<string>
                {
                    Message = $"Record Not found",
                    Succeeded = false,

                };
            }
        }

        public async Task<PaystackResponseDto> InitializePaystackTransaction(PaystackRequestDto paystackReqDto, string bookingId)
        {
            var paymentMethodAcronym = HelperCodeGenerator.GenerateTransactionReference(PaymentMethodNameAcronym.CP.ToString());
            PaystackResponseDto payRes = new();
            payRes.PaymentReference = paymentMethodAcronym;
            var request = new JsonContentPostRequest<PaystackRequestDto>();
            paystackReqDto.amount = paystackReqDto.amount + ".00";
            request.Data = paystackReqDto;
            request.Url = "https://api.paystack.co/transaction/initialize";
            request.AccessToken = _config["Paystack:SecretKey"];

            var response = await _httpService.SendPostRequest<PaystackResponseDto, PaystackRequestDto>(request);
            if (response.Status == "true")
            {
                await _paystackRepository.CreatePaymentAsync(response.Data.Reference, payRes.PaymentReference, paystackReqDto.amount, bookingId);
            }
            if (response.Data.AccessCode == null)
            {
                return new PaystackResponseDto { Status = "false", Message = "Access code not found", Data = null };
            }

            var authorizationCode = response.Data.AccessCode;
            var authorizationUrl = response.Data.AuthorizationUrl;
            // Save the authorization code to the database
            var authCodeToDb = new CardAuthorization { AuthorizationCode = authorizationCode, Email = paystackReqDto.email, AuthorizationUrl = authorizationUrl, CreatedAt = DateTime.UtcNow, ModifiedAt = DateTime.UtcNow };
            _context.CardAuthorizations.Add(authCodeToDb);
            await _context.SaveChangesAsync();

            return response;
        }

        public async Task<Response<string>> VerifyPaymentReference(string paymentReference)
        {
            var paymentVerification = await _paystackRepository.GetPaymentByReferenceAsync(paymentReference);
            if (paymentVerification == null)
            {
                return new Response<string> { Succeeded = false, Message = "payment reference not found!, please try again" };
            };

            if (paymentVerification.Status == Status.Success)
            {
                return new Response<string> { Succeeded = false, Message = "payment is not successful at the moment, you may check back later" };
            };
            var request = new GetRequest();
            request.Url = $"https://api.paystack.co/transaction/verify/{paymentReference}";
            request.AccessToken = _config["Paystack:SecretKey"];
            var response = await _httpService.SendGetRequest<PaystackVerifyPaymentDto>(request);
            if (response.Status == "true")
            {
                if (response.Data.Status == "success")
                {
                    paymentVerification.Status = Status.Success;
                    paymentVerification.ModifiedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return new Response<string> { Succeeded = true, Message = "payment status have been updated to SUCCESS in the DB" };
                }
                else
                {
                    return new Response<string> { Succeeded = false, Message = "Something went wrong, Failed to update the payment status to SUCCESS in DB" };
                }
            }
            return new Response<string> { Succeeded = false, Message = $"Paystack could not verify Payment with ref: {paymentReference}" };
        }

        //public async Task<ChargeResponseDto> ChargeSavedCard(ChargeCardRequestDto chargeCardReq, string bookingId)
        //{
        //    if (string.IsNullOrEmpty(chargeCardReq.Email) || string.IsNullOrEmpty(Convert.ToString(chargeCardReq.Amount)))
        //    {
        //        return new ChargeResponseDto { Status = "false", Message = "email and amount field cannot be empty" };
        //    }
        //    var authorizationCode = await _context.CardAuthorizations
        //                                                    .Where(a => a.Email == chargeCardReq.Email)
        //                                                    .Select(a => a.AuthorizationCode)
        //                                                    .FirstOrDefaultAsync();
        //    if (string.IsNullOrEmpty(authorizationCode))
        //    {
        //        return new ChargeResponseDto { Status = "false", Message = "authorization code not found" };
        //    }

        //    var paymentMethodAcronym = HelperCodeGenerator.GenerateTransactionReference(PaymentMethodNameAcronym.CP.ToString());
        //    var request = new JsonContentPostRequest<ChargeCardRequestDto>();
        //    chargeCardReq.Amount = chargeCardReq.Amount + ".00";
        //    request.Data = chargeCardReq;
        //    request.Url = "https://api.paystack.co/transaction/charge_authorization";
        //    request.AccessToken = _config["Paystack:SecretKey"];
        //    var response = await _httpService.SendPostRequest<ChargeResponseDto, ChargeCardRequestDto>(request);
        //    if (response.Status == "true")
        //    {
        //        await _paystackRepository.CreatePaymentAsync(response.Data.Reference, chargeCardReq.PaymentReference, chargeCardReq.Amount, bookingId);
        //    }




        //    //var request = new AuthorizationCodeChargeRequest
        //    //{
        //    //    Email = email,
        //    //    Amount = amount,
        //    //    AuthorizationCode = authorizationCode,
        //    //    Reference = paymentMethodAcronym,
        //    //};

        //    //ChargeResponse response = PayStack.Charge.ChargeAuthorizationCode(request);
        //    //if (!response.Status)
        //    //{
        //    //    return new Response<PaymentResponse> { Succeeded = false, Message = "Charging saved card failed", Data = null };
        //    //}
        //    //var authorization = response.Data.Authorization;
        //    //if (authorization != null || !authorization.Reusable)
        //    //{
        //    //    return new Response<PaymentResponse> { Succeeded = false, Message = "authorization not found or not reusable, Payment Failed", Data = null };
        //    //}

        //    //var payment = new Payment()
        //    //{
        //    //    Amount = Convert.ToDecimal(amount),
        //    //    PaymentReference = request.Reference,
        //    //};
        //    //await _context.Payments.AddAsync(payment);
        //    //await _context.SaveChangesAsync();
        //    //return new Response<PaymentResponse> { Succeeded = true, Message = "Payment successful", Data = null };
        //}
    }
}