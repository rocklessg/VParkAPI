using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Core.Repositories.Interfaces;
using VPark_Data;
using VPark_Models;

namespace VPark_Core.Repositories.Implementation
{
    public class PaystackRepository : IPaystackRepository
    {
        private readonly AppDbContext _context;
        private readonly ILogger<PaystackRepository> _logger;
        public PaystackRepository(AppDbContext context, ILogger<PaystackRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CreatePaymentAsync(string paystackRef, string paymentReference, string amount, string bookingId)
        {
            var payment = new Payment();
            payment.BookingId = bookingId;
            payment.Amount = Convert.ToDecimal(amount);
            payment.PaystackRef = paystackRef;    
            payment.PaymentReference = paymentReference;
            payment.Status = Status.Pending;
            payment.PaymentMethod = PaymentMethod.Card;
            payment.CreatedAt = DateTime.UtcNow;    
            payment.UpdatedAt = DateTime.UtcNow;    
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
        }

        public async Task<Payment> GetPaymentByReferenceAsync(string paymentRef)
        {
            var result = await _context.Payments.FirstOrDefaultAsync(x => x.PaystackRef == paymentRef);
            return result;
        } 
            
            
            
            
           

    }
}
