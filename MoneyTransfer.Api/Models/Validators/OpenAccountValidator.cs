using FluentValidation;

namespace MoneyTransfer.Api.Models.Validators
{
    public class OpenAccountValidator
        : AbstractValidator<OpenAccount>
    {
        public OpenAccountValidator()
        {
            RuleFor(p => p.FirstName)
                .NotEmpty().WithMessage("You should fill out 'FirstName'.");

            RuleFor(p => p.LastName)
                .NotEmpty().WithMessage("You should fill out 'LastName'.");

            RuleFor(p => p.Iban)
                .NotEmpty().WithMessage("You should fill out 'Iban'.")
                .Length(20).WithMessage("'Iban' should have a length of 25 characters.");

            RuleFor(p => p.Amount)
                .GreaterThanOrEqualTo(0m).WithMessage("'Amount' should be a positive number.");

            RuleFor(p => p.CurrencyCode)
                .NotEmpty().WithMessage("You should fill out 'CurrencyCode'.")
                .Length(3).WithMessage("'CurrencyCode' should have a length of 3 characters.");
        }
    }
}
