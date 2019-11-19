using Akka.Actor;
using Akka.Event;
using Akka.Persistence;
using System;

using MoneyTransfer.Service.Commands;
using MoneyTransfer.Service.Events;
using MoneyTransfer.Service.Messages;
using MoneyTransfer.Service.States;

namespace MoneyTransfer.Service.Actors
{
    public class AccountActor
        : ReceivePersistentActor, IWithUnboundedStash
    {
        public AccountActor(string iban)
        {
            SetReceiveTimeout(TimeSpan.FromSeconds(15));

            State = new AccountState(iban);

            // Recover events

            Recover<AccountOpenedEvent>(@event =>
            {
                State.Apply(@event);

                EventCount++;
            });

            Recover<AccountDebitedEvent>(@event =>
            {
                State.Apply(@event);

                EventCount++;
            });

            Recover<AccountCreditedEvent>(@event =>
            {
                State.Apply(@event);

                EventCount++;
            });

            Recover<AccountClosedEvent>(@event =>
            {
                State.Apply(@event);

                EventCount++;
            });

            Recover<SnapshotOffer>(offer => State = (AccountState)offer.Snapshot);

            Recover<RecoveryCompleted>(m =>
            {
                if (State.Status == AccountStatus.Initialized)
                {
                    Become(Initialized);
                }
                else if (State.Status == AccountStatus.Opened)
                {
                    Become(Opened);
                }
                else
                {
                    Become(Closed);
                }

                Stash.UnstashAll();
            });

            // Commands

            Command<OpenAccountCommand>(HandleOpenAccount, m => true);
            Command<DebitAccountCommand>(HandleDebitAccount, m => true);
            Command<CreditAccountCommand>(HandleCreditAccount, m => true);
            Command<CloseAccountCommand>(HandleCloseAccount, m => true);
            Command<ReceiveTimeout>(HandleReceiveTimeout, m => true);
        }

        private void Uninitialized()
            => CommandAny(_ => Stash.Stash());

        private void Initialized()
            => Command<OpenAccountCommand>(HandleOpenAccount, m => true);

        private void Opened()
        {
            Command<DebitAccountCommand>(HandleDebitAccount, m => true);
            Command<CreditAccountCommand>(HandleCreditAccount, m => true);
            Command<CloseAccountCommand>(HandleCloseAccount, m => true);
        }

        private void Closed()
        { }

        private ILoggingAdapter Log =>
            Context.GetLogger();

        public IStash Stash { get; set; }

        public override string PersistenceId
            => $"Account-{State.Iban}";

        private AccountState State { get; set; }

        private int EventCount { get; set; }

        private void TrySaveSnapshot()
        {
            if (++EventCount % 5 == 0)
            {
                SaveSnapshot(State);

                EventCount = 0;
            }
        }

        protected override void Unhandled(object command)
            => Log.Warning(
                "Unhandled command '{0}' in '{1}'.",
                command.GetType().FullName,
                nameof(AccountManagerActor));

        private void HandleOpenAccount(OpenAccountCommand command)
        {
            Log.Info(
                "Open account command received in {0}({1}).",
                nameof(AccountActor),
                PersistenceId,
                command.Amount,
                command.CurrencyCode);

            var accountCreatedEvent = AccountOpenedEvent.Create(command);

            Persist(accountCreatedEvent, @event =>
            {
                State.Apply(@event);

                TrySaveSnapshot();

                Become(Opened);
            });

            Context
                .ActorSelection("/user/MaterializeView*")
                .Tell(accountCreatedEvent);

            if (Sender != ActorRefs.Nobody)
            {
                Sender.Tell(new Ack());
            }
        }

        private void HandleDebitAccount(DebitAccountCommand command)
        {
            Log.Info(
                "Debit account command received in {0}({1}) -{2} {3}.",
                nameof(AccountActor),
                PersistenceId,
                command.Amount,
                command.CurrencyCode);

            var accountDebitedEvent = AccountDebitedEvent.Create(command);

            if (State.CurrencyCode != command.CurrencyCode && Sender != ActorRefs.Nobody)
            {
                Sender.Tell(new Nack("Currency code mismatch."));

                return;
            }

            if (State.Amount < command.Amount && Sender != ActorRefs.Nobody)
            {
                Sender.Tell(new Nack("Insufficient funds."));

                return;
            }

            Persist(accountDebitedEvent, @event =>
            {
                State.Apply(@event);

                TrySaveSnapshot();
            });

            Context
                .ActorSelection("/user/MaterializeView*")
                .Tell(accountDebitedEvent);

            if (Sender != ActorRefs.Nobody)
            {
                Sender.Tell(new Ack());
            }
        }

        private void HandleCreditAccount(CreditAccountCommand command)
        {
            Log.Info(
                "Credit account command received in {0}({1}) +{2} {3}.",
                nameof(AccountActor),
                PersistenceId,
                command.Amount,
                command.CurrencyCode);

            var accountCreditedEvent = AccountCreditedEvent.Create(command);

            if (State.CurrencyCode != command.CurrencyCode && Sender != ActorRefs.Nobody)
            {
                Sender.Tell(new Nack("Currency code mismatch."));

                return;
            }

            Persist(accountCreditedEvent, @event =>
            {
                State.Apply(@event);

                TrySaveSnapshot();
            });

            Context
                .ActorSelection("/user/MaterializeView*")
                .Tell(accountCreditedEvent);

            if (Sender != ActorRefs.Nobody)
            {
                Sender.Tell(new Ack());
            }
        }

        private void HandleCloseAccount(CloseAccountCommand command)
        {
            Log.Info(
                "Close account command received in {0}({1}).",
                nameof(AccountActor),
                PersistenceId);

            var accountClosedEvent = AccountClosedEvent.Create(command);

            Persist(accountClosedEvent, @event =>
            {
                State.Apply(@event);

                TrySaveSnapshot();

                Become(Closed);
            });

            Context
                .ActorSelection("/user/MaterializeView*")
                .Tell(accountClosedEvent);

            if (Sender != ActorRefs.Nobody)
            {
                Sender.Tell(new Ack());
            }
        }

        private void HandleReceiveTimeout(ReceiveTimeout message)
        {
            Log.Info(
                "Received timeout in {0}({1}). Shutting down...",
                nameof(AccountActor),
                PersistenceId);

            // Process current message than discard the others.
            Context.Stop(Context.Self);

            // A message like any other. All messages that are ahead of it in the queue will be processed first
            // Self.Tell(PoisonPill.Instance);
        }
    }
}
