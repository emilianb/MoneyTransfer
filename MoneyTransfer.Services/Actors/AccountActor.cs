using Akka.Actor;
using Akka.Persistence;
using System;

using MoneyTransfer.Actors.Snapshots;
using MoneyTransfer.Messages;

namespace MoneyTransfer.Actors
{
    public class AccountActor
        : ReceivePersistentActor
    {
        public AccountActor(string iban)
        {
            SetReceiveTimeout(TimeSpan.FromSeconds(15));

            Account = new AccountSnapshot(iban);

            #region Register recover from event handlers

            Recover<SnapshotOffer>(offer =>
            {
                if (offer.Snapshot is AccountSnapshot account)
                {
                    Account = account;
                }
            });

            Recover<AccountEvents.Opened>(Account.Apply);
            Recover<AccountEvents.Debited>(Account.Apply);
            Recover<AccountEvents.Credited>(Account.Apply);
            Recover<AccountEvents.Closed>(Account.Apply);

            Recover<RecoveryCompleted>(@event =>
            {
                if (Account.Status == AccountStatus.Opened)
                {
                    Become(Opened);
                }
                else if(Account.Status == AccountStatus.Closed)
                {
                    Become(Closed);
                }
            });

            #endregion Register recover from event handlers

            Become(Initialized);
        }

        // TODO: Read the value from config.
        private long SnapshotInterval
            => 5;

        public override string PersistenceId
            => $"Account-{Account.Iban}";

        private AccountSnapshot Account { get; set; }

        private void TrySaveSnapshot()
        {
            if (LastSequenceNr % SnapshotInterval == 0)
            {
                SaveSnapshot(Account);
            }
        }

        #region Behaviours

        private void Initialized()
        {
            Command<AccountCommands.Open>(HandleOpen, c => true);
            Command<ReceiveTimeout>(HandleReceiveTimeout, c => true);
        }

        private void Opened()
        {
            Command<AccountCommands.Debit>(HandleDebit, c => true);
            Command<AccountCommands.Credit>(HandleCredit, c => true);
            Command<AccountCommands.Close>(HandleClose, c => true);
            Command<ReceiveTimeout>(HandleReceiveTimeout, c => true);
        }

        private void Closed()
            => Command<ReceiveTimeout>(HandleReceiveTimeout, c => true);

        #endregion Behaviours

        protected override void Unhandled(object message)
            => Log.Warning(
                "[Actor: {0}][Method: {1}][Message: {2}] Unhandled message.",
                nameof(AccountActor),
                nameof(Unhandled),
                message.GetType().FullName);

        #region Command handlers

        private void HandleOpen(AccountCommands.Open command)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Opening account {3}.",
                nameof(AccountActor),
                nameof(HandleOpen),
                command.GetType().FullName,
                Account.Iban);

            var opened = command.ToAccountOpenedEvent();

            Persist(opened, @event =>
            {
                Account.Apply(@event);

                TrySaveSnapshot();

                Become(Opened);

                Context.ActorSelection("/user/MaterializeAccountsView*").Tell(@event);

                Sender.Tell(new Status.Success(@event));
            });
        }

        private void HandleDebit(AccountCommands.Debit command)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Debiting account {3}.",
                nameof(AccountActor),
                nameof(HandleDebit),
                command.GetType().FullName,
                Account.Iban);

            var debited = command.ToAccountDebitedEvent();

            if (Account.CurrencyCode != command.CurrencyCode)
            {
                Sender.Tell(new Status.Failure(new Exception("Currency code mismatch.")));

                return;
            }

            if (Account.Amount < command.Amount)
            {
                Sender.Tell(new Status.Failure(new Exception("Insufficient funds.")));

                return;
            }

            Persist(debited, @event =>
            {
                Account.Apply(@event);

                TrySaveSnapshot();

                Context.ActorSelection("/user/MaterializeAccountsView*").Tell(@event);

                Sender.Tell(new Status.Success(@event));
            });
        }

        private void HandleCredit(AccountCommands.Credit command)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Debiting account {3}.",
                nameof(AccountActor),
                nameof(HandleCredit),
                command.GetType().FullName,
                Account.Iban);

            var credited = command.ToAccountCreditedEvent();

            if (Account.CurrencyCode != command.CurrencyCode)
            {
                Sender.Tell(new Status.Failure(new Exception("Currency code mismatch.")));

                return;
            }

            Persist(credited, @event =>
            {
                Account.Apply(@event);

                TrySaveSnapshot();

                Context.ActorSelection("/user/MaterializeAccountsView*").Tell(@event);

                Sender.Tell(new Status.Success(@event));
            });
        }

        private void HandleClose(AccountCommands.Close command)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Closing account {3}.",
                nameof(AccountActor),
                nameof(HandleClose),
                command.GetType().FullName,
                Account.Iban);

            var closed = command.ToAccountClosedEvent();

            Persist(closed, @event =>
            {
                Account.Apply(@event);

                TrySaveSnapshot();

                Become(Closed);

                Context.ActorSelection("/user/MaterializeAccountsView*").Tell(@event);

                Sender.Tell(new Status.Success(@event));
            });
        }

        private void HandleReceiveTimeout(ReceiveTimeout command)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Shutting down {3}.",
                nameof(AccountActor),
                nameof(HandleReceiveTimeout),
                command.GetType().FullName,
                Account.Iban);

            // Shutdown actor.

            /**
             * Option 1
             *
             * Process current message than discard the others.
             */
            Context.Stop(Context.Self);

            /**
             * Option 2
             *
             * Send PoisonPill.Instance message to Self.
             * All messages that are ahead of it in the queue will be processed first.
             *
             * Self.Tell(PoisonPill.Instance);
             */
        }

        #endregion Command handlers
    }
}
