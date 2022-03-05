using Common.Enums;
using Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankSimulator.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        [HttpPost]
        public IActionResult Payment([FromBody] Payment payment)
        {
            if (payment.Amount > 500)
                return Ok(new AccuringBankResponse {
                    Status = PaymentStatus.Rejected, Message = "Insufficent Balance" });

            if (payment.CardNumber.StartsWith("99"))
                return Ok(new AccuringBankResponse
                {
                    Status = PaymentStatus.Rejected,
                    Message = "Card is blocked"
                });

            return Ok(new AccuringBankResponse { Status = PaymentStatus.Approved, Message = "Ok" });
        }
    }
}
