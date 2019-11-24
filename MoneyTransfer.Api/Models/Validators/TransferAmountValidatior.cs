using FluentValidation;

namespace MoneyTransfer.Api.Models.Validators
{
    public class TransferValidator
        : AbstractValidator<Transfer>
    {
        public TransferValidator()
        {
            RuleFor(p => p.DebitorIban)
                .NotEmpty().WithMessage("You should fill out 'DebitorIban'.")
                .Length(20).WithMessage("'DebitorIban' should have a length of 25 characters.");

            RuleFor(p => p.CreditorIban)
                .NotEmpty().WithMessage("You should fill out 'CreditorIban'.")
                .Length(20).WithMessage("'CreditorIban' should have a length of 25 characters.");

            RuleFor(p => p.Amount)
                .GreaterThan(0m).WithMessage("'Amount' should be a positive number greather than zero.");

            RuleFor(p => p.CurrencyCode)
                .NotEmpty().WithMessage("You should fill out 'CurrencyCode'.")
                .Length(3).WithMessage("'CurrencyCode' should have a length of 3 characters.");
        }
    }
}
