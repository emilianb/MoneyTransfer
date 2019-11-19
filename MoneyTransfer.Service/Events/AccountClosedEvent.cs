using MoneyTransfer.Service.Commands;

namespace MoneyTransfer.Service.Events
{
    public class AccountClosedEvent
    {
        public string Iban { get; set; }

        public static AccountClosedEvent Create(CloseAccountCommand command)
            => new AccountClosedEvent { Iban = command.Iban };
    }
}
