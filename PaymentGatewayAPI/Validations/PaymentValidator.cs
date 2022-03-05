using Common.Models;
using FluentValidation;
using FluentValidation.Results;
using System.Text.RegularExpressions;


//Centalised location for all the validation minimum validation are done
// Repository can be in injected and check around Merchant is preent or not etc
namespace PaymentGatewayAPI.Validations
{
    public interface IPaymentValidator
    {
        ValidationResult Validate(PaymentRequest payment);
    }

    public class PaymentValidator : AbstractValidator<PaymentRequest>, IPaymentValidator
    {
        public PaymentValidator()
        {
            RuleFor(p => p.MerchantId).NotNull().WithMessage("Merchant Id not present or not in guid format");
            RuleFor(p => p.payment)
                .NotNull()
                .WithMessage("Malformed payment details")
                .DependentRules(() =>
                {
                    RuleFor(p => p.payment.Amount).GreaterThan(0).When(p => p.payment != null).WithMessage("amount cannot be zero or negative");
                    RuleFor(p => p.payment.CVV)
                        .InclusiveBetween(100, 999).When(p => p.payment != null).WithMessage("CVV should be in between 100 and 999");
                    RuleFor(p => p.payment.NameOnCard)
                        .NotNull()
                        .When(p => p.payment != null).WithMessage("name attribute not present")
                        .DependentRules(() => RuleFor(p => p.payment.NameOnCard)
                        .NotEmpty()
                        .When(p => p.payment != null).WithMessage("name not present"));
                    RuleFor(p => p.payment.Currency).IsInEnum().When(p => p.payment != null).WithMessage("Not a valid currency");
                    RuleFor(p => p.payment.ExpiryMonthYear).Must(expiryMonthYear => IsValidExpiryMonthAndYear(expiryMonthYear)).When(p => p.payment != null && p.payment.ExpiryMonthYear != null).WithMessage("Card is either Expire or not in valid format");
                    RuleFor(p => p.payment.CardNumber).Must(cardNumber => IsValidCardNumber(cardNumber)).When(p => p.payment != null && p.payment.CardNumber != null).WithMessage("Not a valid card number");
                });

        }

        private bool IsValidCardNumber(string cardNumber)
        {
            const string Pattern = @"\d{4}-\d{4}-\d{4}-\d{4}";
            //this function can be used to check customize vard validation currenlty only regex is applied on 16 digit with hypen
            var regex = new Regex(Pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(cardNumber);
        }

        private bool IsValidExpiryMonthAndYear(string? expiryMonthDate)
        {
            if (expiryMonthDate.Length < 6)
                return false;
            if (int.TryParse(expiryMonthDate.Substring(0, 2), out int month))
            {
                if (month < 1 || month > 12)
                    return false;
            }
            else
            {
                return false;
            }
            if (int.TryParse(expiryMonthDate.Substring(2, expiryMonthDate.Length - 2), out int year))
            {
                if (year < DateTime.Now.Year || month > DateTime.MaxValue.Year)
                    return false;
            }
            else
            {
                return false;
            }

            if (year == DateTime.Now.Year && month < DateTime.Now.Month)
            {
                return false;
            }
            return true;
        }

        ValidationResult IPaymentValidator.Validate(PaymentRequest payment)
        {
            return Validate(payment);
        }
    }
}
