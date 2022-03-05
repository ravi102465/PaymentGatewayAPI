using Common.Models;

namespace PaymentGatewayAPI.Repositories
{
    public class MemoryPaymentRepository : IPaymentRepository
    {
        private static ICollection<MerchantPaymentDetails> MerchantPaymentDetails { get; set; } = new List<MerchantPaymentDetails>();

        public MemoryPaymentRepository()
        {
        }
        public void AddPayment(MerchantPaymentDetails payment)
        {
            MerchantPaymentDetails.Add(payment);
        }

        public IEnumerable<MerchantPaymentDetails> GetPaymentsById(Guid Id)
        {
            var details = MerchantPaymentDetails.Where(p => p.MerchantPaymentId == Id).ToList();
            return details;
        }
    }
}
