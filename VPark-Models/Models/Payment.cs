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
        public string BookingId { get; set; }
        public Booking Booking { get; set; }
    }

    public enum PaymentMethod
    {
        Card = 0, BankTransfer = 1
    }

    public enum Status
    {
        pending = 0, failed = 1, suceeded = 3
    }
}