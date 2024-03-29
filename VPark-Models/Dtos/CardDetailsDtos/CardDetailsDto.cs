﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Dtos.CardDetailsDtos
{
    public class AuthorizeCardDto
    {
        [Required]
        [JsonProperty("Email")]
        [StringLength(100)]
        public string Email { get; set; }
        [Required]
        [JsonProperty("Amount")]
        [StringLength(100)]
        public string Amount { get; set; }

        [Required]
        [JsonProperty("Card Owner Name")]
        [StringLength(100)]
        public string CardOwnerName { get; set; }
        [Required]
        [JsonProperty("Card Number")]
        [StringLength(19, MinimumLength = 16)]
        public string CardNumber { get; set; }
        [Required]
        [JsonProperty("Card Expiry Month")]
        [StringLength(5)]
        public string CardExpiryMonth { get; set; }
        [Required]
        [JsonProperty("Card Expiry Year")]
        [StringLength(5)]
        public string CardExpiryYear { get; set; }
        [Required]
        [JsonProperty("Cvv")]
        [StringLength(3)]
        public string Cvv { get; set; }

        [Required]
        [JsonProperty("Pin")]
        [StringLength(4)]
        public string Pin { get; set; }

    }

    public class CardAuthorizeResponseDto
    {
        public string PaymenReference { get; set; }
    }
}
