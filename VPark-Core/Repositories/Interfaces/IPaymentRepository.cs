using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models;
using VPark_Models.Dtos;
using VPark_Models.Dtos.PaymentDto;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Response<PaymentResponseDto>> AddPayment(PaymentResponseDto payment, string bookingId);
    }
}
