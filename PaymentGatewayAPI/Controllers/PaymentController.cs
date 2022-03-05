using Common.Models;
using Microsoft.AspNetCore.Mvc;
using PaymentGatewayAPI.Services;
using PaymentGatewayAPI.Validations;

namespace PaymentGatewayAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> logger;
        private readonly IPaymentValidator paymentValidator;
        private readonly IPaymentService paymentService;
        public PaymentController(ILogger<PaymentController> logger, IPaymentValidator paymentValidator,IPaymentService paymentService)
        {
            this.logger = logger;
            this.paymentValidator = paymentValidator; 
            this.paymentService = paymentService;
        }

        [HttpPost]
        public virtual async Task<IActionResult> Payment([FromBody]PaymentRequest payment)
        {
            logger.LogDebug("Payment endpoint called");
            if (payment == null)
                return BadRequest("Request object is malformed or empty");

            var validationResult = paymentValidator.Validate(payment);
            if (!validationResult.IsValid)
            {
                //This can be modeled into class and standarised error responses but skipped as need to done quickly
                return new BadRequestObjectResult(new {
                    Errors = validationResult.Errors.Select( err => new { filedName = err.PropertyName, Message = err.ErrorMessage })
                });
            }
            var result = await paymentService.DoPaymentAsync(payment);
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
    public IActionResult GetPayment([FromRoute] Guid id)
        {
            logger.LogDebug("get Payment endpoint called");

            //Simliar to post method validation can be done using validator but for now skipping that
            
            //404 not found should be done if perchant not present
            var payments = paymentService.GetPaymentListByMerchant(id);
            return Ok(new { MerchantId = id, Payments = payments });
        }
    }
}