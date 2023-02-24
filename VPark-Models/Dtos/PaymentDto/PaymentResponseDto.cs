using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models.Models;

namespace VPark_Models.Dtos.PaymentDto
{
    public class PaymentResponseDto : PaymentRequestDto
    {
        public string PaymentReference { get; set; }
        public Status Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string BookingId { get; set; }
    }
}
