using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IPaystackRepository
    {
        Task CreatePaymentAsync(string PaystackRef, string paymentReference, string amount, string bookingId);
        Task<Payment> GetPaymentByReferenceAsync(string paymentRef);
    }
}
