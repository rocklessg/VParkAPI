using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Helper
{
    public class PayStackRequestCardChargeDto
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("card")]
        public Card Card { get; set; }

        [JsonProperty("pin")]
        public string Pin { get; set; }
    }

    public class Card
    {
        [JsonProperty("cvv")]
        public string Cvv { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("expiry_month")]
        public string ExpiryMonth { get; set; }

        [JsonProperty("expiry_year")]
        public string ExpiryYear { get; set; }
    }

    public class PayStackResponseCardChargeDto
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public PayStackResponseCardChargeDataDto Data { get; set; }
    }

    public class PayStackResponseCardChargeDataDto
    {
        [JsonProperty(PropertyName = "authorization_url")]
        public string AuthorizationUrl { get; set; }
        [JsonProperty(PropertyName = "access_code")]
        public string AccessCode { get; set; }
        public string PaymentRef { get; set; }
    }
}
