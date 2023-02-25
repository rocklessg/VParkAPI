using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPark_Models.Models
{
    public class CardDetails :BaseEntity
    {      
        [Required]
        [StringLength(100)]              
        public string CardOwnerName { get; set; }
        [Required]
        [StringLength(19, MinimumLength = 16)]
        public string CardNumber { get; set; }
        [Required]
        [StringLength(5)]  
        public string ExpirationDate { get; set; }
        public CardType CardType { get; set; }
        [Required]
        [StringLength(3)]  
        public string CVV { get; set; }
        [Required]
        [StringLength(3)]
        public string Pin { get; set; }

        //Nav properties
        public string appUserId { get; set; }
        [ForeignKey(nameof(appUserId))]
        public AppUser AppUser { get; set; }

    }
}

namespace VPark_Models
{
    public enum CardType
    {
        VisaCard = 1,
        MasterCard = 2
    }
}