
using Common.Enums;
using Common.Models;

namespace PaymentGatewayAPI.Repositories
{
    public interface IPaymentRepository
    {
        IEnumerable<MerchantPaymentDetails> GetPaymentsById(Guid Id);
        void AddPayment(MerchantPaymentDetails payment);
    }
}
