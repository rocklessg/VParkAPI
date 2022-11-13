using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Core.Repositories.Interfaces;
using VPark_Models.Dtos;

namespace VPark_Core.Repositories.Implementation
{
    public class PaymentRepository : IPaymentRepository
    {
        public Task<Response<PaymentDto>> AddPayment(PaymentDto payment)
        {
            throw new NotImplementedException();
        }
    }
}
