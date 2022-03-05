
using Common.Models;
using PaymentGatewayAPI.Repositories;
using PaymentGatewayAPI.Helpers;

namespace PaymentGatewayAPI.Services
{
    public class PaymentService : IPaymentService
    {
        public readonly IPaymentRepository Repositroy;
        public readonly IBankSimulatorClient BankSimulatorClient;
        public PaymentService(IPaymentRepository Repositroy, IBankSimulatorClient bankSimulatorClient)
        {
            this.Repositroy = Repositroy;
            this.BankSimulatorClient = bankSimulatorClient;
        }
        public async Task<AccuringBankResponse> DoPaymentAsync(PaymentRequest paymentRequest)
        {
            //call bank mimic
            var result = await BankSimulatorClient.TakePayment(paymentRequest.payment);

            var merchantPaymentDetails = new MerchantPaymentDetails
            {
                MerchantPaymentId = (Guid)paymentRequest.MerchantId,
                Payment = paymentRequest.payment,
                Status = result.Status
            };
            Repositroy.AddPayment(merchantPaymentDetails);

            return result;
        }

        public IEnumerable<MerchantPaymentDetails> GetPaymentListByMerchant(Guid MerchantId)
        {
            var paymentByMerchant = Repositroy.GetPaymentsById(MerchantId);

            //Busines logic arounf masking and all

            return paymentByMerchant.Select(x => new MerchantPaymentDetails
            {
                Payment = new Payment
                {
                    Amount = x.Payment.Amount,
                    CardNumber = $"XXXX-XXXX-XXXX-{x.Payment.CardNumber?.Split("-").ToList().Last()}",
                    CVV = 999,
                    Currency = x.Payment.Currency,
                    NameOnCard = x.Payment.NameOnCard,
                    ExpiryMonthYear = x.Payment.ExpiryMonthYear
                },
                Status = x.Status
            });
        }
    }
}
