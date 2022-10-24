using System.ComponentModel.DataAnnotations;
using VPark_Models.Models;

namespace VPark_Models
{
    public class Payment
    {       
        public string PaymentReference { get; set; }
        public decimal Amount { get; set; }
        public Status Status { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        //Nav properties
        [Key]
        public string BookingId { get; set; }
        public Booking Booking { get; set; }
    }

    public enum PaymentMethod
    {
        Card = 1, BankTransfer = 2
    }

    public enum Status
    {
        pending = 1, failed = 2, suceeded = 3
    }
}