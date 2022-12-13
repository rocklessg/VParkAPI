using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Dtos.CardDetailsDtos
{
    public class CardDetailsDto
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Display(Name = "Card Owner Name")]
        [StringLength(100)]
        public string CardOwnerName { get; set; }
        [Required]
        [Display(Name = "Card Number")]
        [StringLength(19, MinimumLength = 16)]
        public string CardNumber { get; set; }
        [Required]
        [Display(Name = "Expiration Date")]
        [StringLength(5)]
        public string ExpirationDate { get; set; }
        [Required]
        [Display(Name = "CVV")]
        [StringLength(3)]
        public string CVV { get; set; }
        public CardType CardType { get; set; }
    }
}
