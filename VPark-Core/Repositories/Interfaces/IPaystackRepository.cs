using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models;
using VPark_Models.Dtos.PaystackDto;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IPaystackRepository
    {
        Task<PaystackResponseDto> InitializePaystackTransaction(PaystackRequestDto paystackReqDto, string bookingId);
        Task<PaystackVerifyPaymentDto> VerifyPaymentReference(string paystackRef, string parkingSpaceId);
        Task<PaystackResponseDto> ChargeSavedCard(PaystackRequestDto chargeCardReq, string bookingId);

    }
}
