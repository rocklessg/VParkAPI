using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Models;

namespace VPark_Models.Dtos
{
    public class PaymentDto
    {
        public string PaymentReference { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string BookingId { get; set; }
    }
}
