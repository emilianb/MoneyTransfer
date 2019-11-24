using Akka.Actor;
using Akka.Event;

using MoneyTransfer.Entities;
using MoneyTransfer.Messages;

namespace MoneyTransfer.Actors
{
    public class MaterializeAccountsViewActor
        : ReceiveActor
    {
        public MaterializeAccountsViewActor()
        {
            var connectionStringPath = "akka.persistence.journal.mongodb.connection-string";
            var connectionString = Context.System.Settings.Config
                .GetString(connectionStringPath)
                .Replace("Write", "Read");

            AccountService = new AccountService(connectionString);

            #region Register event handlers

            Receive<AccountEvents.Opened>(HandleOpened, e => true);
            Receive<AccountEvents.Debited>(HandleDebited, e => true);
            Receive<AccountEvents.Credited>(HandleCredited, e => true);
            Receive<AccountEvents.Closed>(HandleClosed, e => true);

            #endregion Register event handlers
        }

        private ILoggingAdapter Log
            => Context.GetLogger();

        private IAccountService AccountService { get; }

        protected override void Unhandled(object message)
            => Log.Warning(
                "[Actor: {0}][Method: {1}][Message: {2}] Unhandled message.",
                nameof(MaterializeAccountsViewActor),
                nameof(Unhandled),
                message.GetType().FullName);

        #region Event handlers

        private void HandleOpened(AccountEvents.Opened @event)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Processing event for {3}.",
                nameof(MaterializeAccountsViewActor),
                nameof(HandleOpened),
                @event.GetType().FullName,
                @event.Iban);

            var account = new Account
            {
                FirstName = @event.FirstName,
                LastName = @event.LastName,
                Iban = @event.Iban,
                Amount = @event.Amount,
                CurrencyCode = @event.CurrencyCode,
            };

            AccountService.Add(account);
        }

        private void HandleDebited(AccountEvents.Debited @event)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Processing event for {3}.",
                nameof(MaterializeAccountsViewActor),
                nameof(HandleDebited),
                @event.GetType().FullName,
                @event.Iban);

            AccountService.UpdateAmount(@event.Iban, -@event.Amount);
        }

        private void HandleCredited(AccountEvents.Credited @event)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Processing event for {3}.",
                nameof(MaterializeAccountsViewActor),
                nameof(HandleCredited),
                @event.GetType().FullName,
                @event.Iban);

            AccountService.UpdateAmount(@event.Iban, @event.Amount);
        }

        private void HandleClosed(AccountEvents.Closed @event)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Processing event for {3}.",
                nameof(MaterializeAccountsViewActor),
                nameof(HandleClosed),
                @event.GetType().FullName,
                @event.Iban);

            AccountService.Remove(@event.Iban);
        }

        #endregion Event handlers
    }
}
