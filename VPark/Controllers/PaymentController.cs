using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VPark_Core.Repositories.Interfaces;
using VPark_Models.Dtos;

namespace VPark.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentRepository _paymentRepository;
       
        public PaymentController(IPaymentRepository paymentRepository, ILogger<PaymentController> logger)
        {
            _paymentRepository = paymentRepository;
            
        }

        [HttpPost]
        public async Task<IActionResult> AddPayment(PaymentDto paymentDto,string bookingId )
        {
            var response = await _paymentRepository.AddPayment(paymentDto, bookingId);
            return Ok(response);    

        }
    }
}
