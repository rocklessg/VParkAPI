using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VPark_Models;
using VPark_Models.Dtos;
using VPark_Models.Dtos.CardDetailsDtos;
using VPark_Models.Models;

namespace VPark_Core.Repositories.Interfaces
{
    public interface IPaymentRepository
    {

        Task<Response<CardDetailsDto>> AddCard(CardDetailsDto cards, string appUserId);
        Task<Response<IEnumerable<CardDetails>>> GetAllCardsAsync();
        Task<Response<CardDetails>> GetCardByUserId(string cardId);
        Task<Response<PaymentDto>> AddPayment(PaymentDto payment, string bookingId);
        Task<Response<string>> RemoveCard(string cardId);
    }
}
