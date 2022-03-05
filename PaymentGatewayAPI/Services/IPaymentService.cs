
using Common.Models;

namespace PaymentGatewayAPI.Services
{
    public interface IPaymentService
    {
        public Task<AccuringBankResponse> DoPaymentAsync(PaymentRequest payment);

        IEnumerable<MerchantPaymentDetails> GetPaymentListByMerchant(Guid MerchantId);
    }
}
