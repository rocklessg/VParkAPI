using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Dtos.PaystackDto
{
    public class PaystackVerifyPaymentDto
    {
        public string Status { get; set; }
        public PayStackVerifyPaymentDataDto Data { get; set; }
        public string Message { get; set; }
    }

    public class PayStackVerifyPaymentDataDto
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public string Reference { get; set; }
        public int Amount { get; set; }
    }
}
