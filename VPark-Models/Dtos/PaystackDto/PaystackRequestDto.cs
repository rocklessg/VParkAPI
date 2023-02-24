using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Dtos.PaystackDto
{
    public class PaystackRequestDto
    {
        [JsonProperty(PropertyName = "email")]
        public string email { get; set; }
        [JsonProperty(PropertyName = "amount")]
        public string amount { get; set; }
        
    }
    public class PaystackResponseDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonProperty("authorization_url")]
        public string AuthorizationUrl { get; set; }
        [JsonProperty("access_code")]
        public string AccessCode { get; set; }
        public string Reference { get; set; }

    }
}
