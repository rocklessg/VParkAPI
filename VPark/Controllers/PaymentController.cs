using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPark_Core.Repositories.Interfaces;
using VPark_Helper;
using VPark_Models.Dtos;
using VPark_Models.Dtos.CardDetailsDtos;
using VPark_Models.Models;

namespace VPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IServiceFee _serviceFee;

        public PaymentController(IPaymentRepository paymentRepository, ILogger<PaymentController> logger, 
            IServiceFee serviceFee)
        {
            _paymentRepository = paymentRepository;
            _serviceFee = serviceFee;
        }

        [HttpPost("Add-Payment")]
        public async Task<IActionResult> AddPayment(PaymentDto paymentDto, string bookingId)
        {
            var response = await _paymentRepository.AddPayment(paymentDto, bookingId);
            return Ok(response);

        }

        [HttpPost("Add-Card")]
        public async Task<IActionResult> AddCard(CardDetailsDto cards, string appUserId)
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
    }
}
