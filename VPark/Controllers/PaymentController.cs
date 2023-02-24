using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPark_Core.Repositories.Interfaces;
using VPark_Helper;
using VPark_Models.Dtos;
using VPark_Models.Dtos.CardDetailsDtos;
using VPark_Models.Dtos.PaystackDto;
using VPark_Models.Models;

namespace VPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IServiceFee _serviceFee;
        private readonly IConfiguration _config;

        public PaymentController(IPaymentRepository paymentRepository, ILogger<PaymentController> logger,
            IServiceFee serviceFee, IConfiguration config)
        {
            _paymentRepository = paymentRepository;
            _serviceFee = serviceFee;
            _config = config;
        }

        [HttpPost("Add-Payment")]
        public async Task<IActionResult> AddPayment(PaymentDto paymentDto, string bookingId)
        {
            var response = await _paymentRepository.AddPayment(paymentDto, bookingId);
            return Ok(response);

        }

        [HttpPost("Add-Card")]
        public async Task<IActionResult> AddCard(AuthorizeCardDto cards, string appUserId)
        {
            var response = await _paymentRepository.AddCard(cards, appUserId);  
            return Ok(response);
           
        }

        [HttpGet("Get-AllCards")]
        public async Task<IActionResult> GetAllCards()
        {
            var response = await _paymentRepository.GetAllCardsAsync();
            return Ok(response);   
        }

        [HttpGet("Get-CardById")]
        public async Task<IActionResult> GetCardById(string cardId)
        {
            var response = await _paymentRepository.GetCardByUserId(cardId);
            return Ok(response);            
        }

        [HttpDelete("Delete-Card")]
        public async Task<IActionResult> RemoveCard(string cardId)
        {
            var response = await _paymentRepository.RemoveCard(cardId);
            return Ok(response);    
        }

       

        //[HttpPost("Charge-Savedcard")]
        //public async Task<IActionResult> ChargeSavedCard(string email, string amount)
        //{
        //    var response = await _paymentRepository.ChargeSavedCard(email, amount);
        //    return Ok(response);
        //}
       

        [HttpPost("Paystack-initializepayment")]
        public async Task<IActionResult> InitializePaystackPayment(PaystackRequestDto paystackReqDto, string bookingId)
        {
            var response = await _paymentRepository.InitializePaystackTransaction(paystackReqDto, bookingId);
            return Ok(response);
        }

        [HttpGet("Verify-payment")]
        public async Task<IActionResult> VerifyPaystackPayment(string reference)
        {
            var response = await _paymentRepository.VerifyPaymentReference(reference);
            return Ok(response);
        }


    }
}
