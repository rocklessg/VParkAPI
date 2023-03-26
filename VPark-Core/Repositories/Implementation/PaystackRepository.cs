using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Core.Repositories.Interfaces;
using VPark_Data;
using VPark_Helper.Request;
using VPark_Helper;
using VPark_Models;
using VPark_Models.Dtos.PaystackDto;
using VPark_Models.Models;
using PayStack.Net;
using Microsoft.Extensions.Configuration;

namespace VPark_Core.Repositories.Implementation
{
    public class PaystackRepository : IPaystackRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaystackRepository> _logger;
        private readonly IHttpServices _httpService;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IConfiguration _config;
        private PayStackApi PayStack { get; set; }
        private readonly string token;

        public PaystackRepository(AppDbContext context, ILogger<PaystackRepository> logger, IHttpServices httpService, IPaymentRepository paymentRepository, IConfiguration config)
        {
            _context = context;
            _logger = logger;
            _httpService = httpService;
            _paymentRepository = paymentRepository;
            token = _config["Paystack: SecretKey"];
            PayStack = new PayStackApi(token);
            _config = config;
        }

        public async Task<PaystackResponseDto> InitializePaystackTransaction(PaystackRequestDto paystackReqDto, string bookingId)
        {
            var paymentMethodAcronym = HelperCodeGenerator.GenerateTransactionReference(PaymentMethodNameAcronym.CP.ToString());
            PaystackResponseDto payRes = new();
            payRes.PaymentReference = paymentMethodAcronym;
            var request = new JsonContentPostRequest<PaystackRequestDto>();
            paystackReqDto.amount = string.Format(CultureInfo.GetCultureInfo("ig-NG"), "{0:c}", paystackReqDto.amount).Replace(" ", "");
            request.Data = paystackReqDto;
            request.Url = "https://api.paystack.co/transaction/initialize";
            request.AccessToken = _config["Paystack:SecretKey"];

            var response = await _httpService.SendPostRequest<PaystackResponseDto, PaystackRequestDto>(request);
            if (response.Status == "true")
            {
                await _paymentRepository.CreatePaymentAsync(response.Data.Reference, payRes.PaymentReference, paystackReqDto.amount, bookingId);
            }
            if (response.Data.Access_code == null)
            {
                return new PaystackResponseDto { Status = "false", Message = "Access code not found", Data = null };
            }
            return response;
        }

        public async Task<PaystackVerifyPaymentDto> VerifyPaymentReference(string paystackRef, string parkingSpaceId)
        {
            var paymentVerification = await _paymentRepository.GetPaymentByReferenceAsync(paystackRef);
            if (paymentVerification == null)
            {
                return new PaystackVerifyPaymentDto { Status = "false", Message = "payment reference not found!, please try again" };
            };

            if (paymentVerification.Status == Status.Success)
            {
                return new PaystackVerifyPaymentDto { Status = "true", Message = "payment completed for this operation" };
            };
            var request = new GetRequest();
            request.Url = $"https://api.paystack.co/transaction/verify/{paystackRef}";
            request.AccessToken = _config["Paystack:SecretKey"];
            var response = await _httpService.SendGetRequest<PaystackVerifyPaymentDto>(request);
            if (response.Status == "true")
            {
                if (response.Data.authorization.reusable == true)
                {
                    // Save the authorization_code alonside to the database
                    var paystackEmail = response.Data.Customer.Email;
                    var authorizationCode = response.Data.authorization.authorization_code;
                    var authCodeplusEmailToDb = new CardAuthorization
                    {
                        AuthorizationCode = authorizationCode,
                        Email = paystackEmail,
                        CreatedAt = DateTime.UtcNow,
                        ModifiedAt = DateTime.UtcNow
                    };
                    _context.CardAuthorizations.Add(authCodeplusEmailToDb);
                    await _context.SaveChangesAsync();

                    if (response.Data.Status == "success")
                    {
                        paymentVerification.Status = Status.Success;
                        paymentVerification.ModifiedAt = DateTime.UtcNow;
                        //Update the isBooked to true
                        var parkingSpaceToBook = await _context.ParkingSpaces.Where(x => x.Id == parkingSpaceId).FirstOrDefaultAsync();
                        parkingSpaceToBook.IsBooked = true;
                        parkingSpaceToBook.ModifiedAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();
                        return new PaystackVerifyPaymentDto { Status = "true", Message = "payment status and Booking have been updated to TRUE in the DB and " };
                    }
                    else
                    {
                        return new PaystackVerifyPaymentDto { Status = "false", Message = "Something went wrong, Failed to update the payment status and Booking status to TRUE in DB" };
                    }
                };
            };
            return new PaystackVerifyPaymentDto { Status = "false", Message = $"Paystack could not verify Payment with ref: {paystackRef}" };
        }

        public async Task<PaystackResponseDto> ChargeSavedCard(PaystackRequestDto chargeCardReq, string bookingId)
        {
            if (string.IsNullOrEmpty(chargeCardReq.email) || string.IsNullOrEmpty(Convert.ToString(chargeCardReq.amount)))
            {
                return new PaystackResponseDto { Status = "false", Message = "email and amount field cannot be empty" };
            }
            var authorizationCode = await _context.CardAuthorizations
                                                            .Where(a => a.Email == chargeCardReq.email)
                                                            .Select(a => a.AuthorizationCode)
                                                            .FirstOrDefaultAsync();
            if (string.IsNullOrEmpty(authorizationCode))
            {
                return new PaystackResponseDto { Status = "false", Message = "authorization code not found" };
            }

            var paymentMethodAcronym = HelperCodeGenerator.GenerateTransactionReference(PaymentMethodNameAcronym.CP.ToString());
            PaystackResponseDto payRes = new();
            payRes.PaymentReference = paymentMethodAcronym;
            var request = new JsonContentPostRequest<PaystackRequestDto>();
            chargeCardReq.amount = string.Format(CultureInfo.GetCultureInfo("ig-NG"), "{0:c}", chargeCardReq.amount).Replace(" ", "");
            chargeCardReq.authorization_code = authorizationCode;
            request.Data = chargeCardReq;
            request.Url = "https://api.paystack.co/transaction/charge_authorization";
            request.AccessToken = _config["Paystack:SecretKey"];
            var response = await _httpService.SendPostRequest<PaystackResponseDto, PaystackRequestDto>(request);
            if (response.Status == "true")
            {
                await _paymentRepository.CreatePaymentAsync(response.Data.Reference, payRes.PaymentReference, chargeCardReq.amount, bookingId);

            }

            return response;

        }




    }
}
