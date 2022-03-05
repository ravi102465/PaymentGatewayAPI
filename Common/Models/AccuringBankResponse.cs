using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class AccuringBankResponse
    {
        public PaymentStatus Status { get; set; }
        public string? Message { get; set; }
    }
}
