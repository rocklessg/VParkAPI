using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models;

namespace VPark_Helper
{
    public static class HelperCodeGenerator
    {
        public static readonly Random CodeGenerator = new();

        public static string GenerateSixDigitToken()
        {
            string token = CodeGenerator.Next(0, 1000000).ToString("D6");
            return token;
        }

        public static string GenerateTransactionReference(string paymentMethodNameAcronym)
        {
            string generatedNumbers = GenerateSixDigitToken();
            string transactionReference = $"{paymentMethodNameAcronym}_{DateTime.UtcNow.TimeOfDay.Ticks}{generatedNumbers}";
            return transactionReference;
        }

        public static string GenerateBookingReference(string prefix)
        {
            string generatedNumbers = GenerateSixDigitToken();
            string bookingReference = $"{prefix}_{DateTime.UtcNow.TimeOfDay.Ticks}{generatedNumbers}";
            return bookingReference;
        }

       

    }
}
