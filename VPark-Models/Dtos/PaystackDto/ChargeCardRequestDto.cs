using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Dtos.PaystackDto
{
    public class ChargeCardRequestDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
    }

    public class ChargeResponseDto
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public ChargeCardResponseData Data { get; set; }
    }

    public class ChargeCardResponseData
    {

        [JsonProperty("reference")]
        public string Reference { get; set; }
    }

}
