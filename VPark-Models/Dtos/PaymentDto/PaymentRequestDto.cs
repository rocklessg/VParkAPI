using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Dtos.PaymentDto
{
    public class PaymentRequestDto
    {
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
