using Common.Enums;

namespace Common.Models
{
    public class PaymentRequest
    {
        public Guid? MerchantId { get; set; }
        public Payment payment { get; set; }
    }
    public class Payment
    {
        public string? NameOnCard { get; set; }
        public string? CardNumber { get; set; }
        public int? CVV { get; set; }
        public decimal? Amount { get; set; }
        public Currency Currency { get; set; }
        public string? ExpiryMonthYear { get; set; }
    }
}