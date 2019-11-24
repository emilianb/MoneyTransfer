using MoneyTransfer.Messages;

namespace MoneyTransfer.Actors.Snapshots
{
    public class AccountSnapshot
    {
        public AccountSnapshot(string iban)
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

        #region Apply events

        public void Apply(AccountEvents.Opened @event)
        {
            FirstName = @event.FirstName;
            LastName = @event.LastName;
            Amount = @event.Amount;
            CurrencyCode = @event.CurrencyCode;
            Status = AccountStatus.Opened;
        }

        public void Apply(AccountEvents.Debited @event)
        {
            Amount = Amount - @event.Amount;
        }

        public void Apply(AccountEvents.Credited @event)
        {
            Amount = Amount + @event.Amount;
        }

        public void Apply(AccountEvents.Closed @event)
        {
            Status = AccountStatus.Closed;
        }

        #endregion Apply events
    }
}
