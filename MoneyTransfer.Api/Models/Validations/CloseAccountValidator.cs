using FluentValidation;

namespace MoneyTransfer.Api.Models.Validations
{
    public class CloseAccountValidator
        : AbstractValidator<CloseAccount>
    {
        public CloseAccountValidator()
        {
            RuleFor(p => p.Iban)
                .NotEmpty().WithMessage("You should fill out 'Iban'.")
                .Length(20).WithMessage("'Iban' should have a length of 25 characters.");
        }
    }
}
