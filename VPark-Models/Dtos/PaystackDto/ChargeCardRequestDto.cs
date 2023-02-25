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

        public string authorization_code { get; set; }
        public string PaymentReference { get; set; } = string.Empty;
    }

    public class ChargeResponseDto
    {
        public bool status { get; set; }
        public string message { get; set; }
        public SavedCardData Data { get; set; }
    }

    public class SavedCardData
    {

        [JsonProperty("reference")]
        public string Reference { get; set; }
        public string authorization_url { get; set; }
        public string access_code { get; set; }
        public bool paused { get; set; }
    }

}
