using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPark_Core.Repositories.Interfaces;
using VPark_Helper;
using VPark_Models.Dtos;
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

        [HttpGet("Get-ParkingSpaceFee")]
        public IActionResult GetParkingSpaceFee(ServiceType serviceType, int parkingDuration)
        {
            var response = _serviceFee.GetParkingSpaceFee(serviceType, parkingDuration);
            return Ok(response);    
        }
    }
}
