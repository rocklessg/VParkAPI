using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Helper
{
    public class PayStackTransactionDto
    {
        public string Status { get; set; }
        public PayStackTransactionDataDto Data { get; set; }
        public string Message { get; set; }
    }

    public class PayStackTransactionDataDto
    {
        [JsonProperty(PropertyName = "authorization_url")]
        public string AuthorizationUrl { get; set; }
        [JsonProperty(PropertyName = "access_code")]
        public string AccessCode { get; set; }
        public string Reference { get; set; }
    }
}
