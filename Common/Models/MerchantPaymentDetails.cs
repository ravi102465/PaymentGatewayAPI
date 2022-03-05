using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Models
{
    public class MerchantPaymentDetails
    {
        [JsonIgnore]
        public Guid MerchantPaymentId { get; set; }

        public Payment Payment { get; set; }

        public PaymentStatus Status { get; set; }
    }
}
