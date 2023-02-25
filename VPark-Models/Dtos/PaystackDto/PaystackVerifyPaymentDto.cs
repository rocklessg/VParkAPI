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
        public Authorization authorization { get; set; }    
        public Customer Customer { get; set; }    

    }

    public class Authorization
    {
        public string authorization_code { get; set; }
        public bool reusable { get; set; }
    }

    public class Customer
    {
        public string Email { get; set; }
    }
}
