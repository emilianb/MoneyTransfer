using MoneyTransfer.Service.Commands;

namespace MoneyTransfer.Service.Events
{
    public class AccountOpenedEvent
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Iban { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }

        public static AccountOpenedEvent Create(OpenAccountCommand command)
            => new AccountOpenedEvent
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                Iban = command.Iban,
                Amount = command.Amount,
                CurrencyCode = command.CurrencyCode
            };
    }
}
