using MoneyTransfer.Service.Events;

namespace MoneyTransfer.Service.States
{
    public class AccountState
    {
        public AccountState(string iban)
        {
            Iban = iban;
            Status = AccountStatus.Initialized;
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Iban { get; private set; }

        public decimal Amount { get; private set; }

        public string CurrencyCode { get; private set; }

        public AccountStatus Status { get; private set; }

        public void Apply(AccountOpenedEvent @event)
        {
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            Amount = @event.Amount;
            CurrencyCode = @event.CurrencyCode;
            Status = AccountStatus.Opened;
        }

        public void Apply(AccountDebitedEvent @event)
        {
            Amount = Amount - @event.Amount;
        }

        public void Apply(AccountCreditedEvent @event)
        {
            Amount = Amount + @event.Amount;
        }

        public void Apply(AccountClosedEvent @event)
        {
            Status = AccountStatus.Closed;
        }
    }
}
