using Common.Models;

namespace PaymentGatewayAPI.Helpers
{
    public interface IBankSimulatorClient
    {
        public Task<AccuringBankResponse> TakePayment(Payment payment);
    }
}
