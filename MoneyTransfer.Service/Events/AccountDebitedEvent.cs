using MoneyTransfer.Service.Commands;

namespace MoneyTransfer.Service.Events
{
    public class AccountDebitedEvent
    {
        public string Iban { get; private set; }

        public decimal Amount { get; private set; }

        public string CurrencyCode { get; private set; }

        public static AccountDebitedEvent Create(DebitAccountCommand command)
            => new AccountDebitedEvent
            {
                Iban = command.Iban,
                Amount = command.Amount,
                CurrencyCode = command.CurrencyCode
            };
    }
}
