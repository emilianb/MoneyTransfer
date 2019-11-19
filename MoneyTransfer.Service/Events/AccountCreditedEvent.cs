using MoneyTransfer.Service.Commands;

namespace MoneyTransfer.Service.Events
{
    public class AccountCreditedEvent
    {
        public string Iban { get; private set; }

        public decimal Amount { get; private set; }

        public string CurrencyCode { get; private set; }

        public static AccountCreditedEvent Create(CreditAccountCommand command)
            => new AccountCreditedEvent
            {
                Iban = command.Iban,
                Amount = command.Amount,
                CurrencyCode = command.CurrencyCode
            };
    }
}
