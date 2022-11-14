using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models;
using VPark_Models.Dtos;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IPaymentRepository
    {
        Task<Response<PaymentDto>> AddPayment(PaymentDto payment, string bookingId);
    }
}
