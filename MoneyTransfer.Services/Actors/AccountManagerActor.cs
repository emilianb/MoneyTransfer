using Akka.Actor;
using Akka.Event;

using MoneyTransfer.Messages;

namespace MoneyTransfer.Actors
{
    public class AccountManagerActor
        : ReceiveActor
    {
        public AccountManagerActor()
        {
            Receive<IAccountCommand>(HandleAccountCommand, c => true);
        }

        private ILoggingAdapter Log
            => Context.GetLogger();

        protected override void Unhandled(object message)
            => Log.Warning(
                "[Actor: {0}][Method: {1}][Message: {2}] Unhandled message.",
                nameof(AccountManagerActor),
                nameof(Unhandled),
                message.GetType().FullName);

        private void HandleAccountCommand(IAccountCommand command)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Forwarding command for account {3}.",
                nameof(AccountManagerActor),
                nameof(HandleAccountCommand),
                command.GetType().FullName,
                command.Iban);

            GetOrCreateChild(Context, command.Iban).Forward(command);
        }

        private IActorRef GetOrCreateChild(IActorContext context, string iban)
        {
            var childName = $"Account-{iban.ToUpperInvariant()}";
            var child = Context.Child(childName);

            if (child.Equals(ActorRefs.Nobody))
            {
                var childProps = Props.Create(() => new AccountActor(iban.ToUpperInvariant()));

                child = context.ActorOf(childProps, childName);

                Log.Debug("[Actor: {0}][Method: {1}] Created {2} for {3}.",
                    nameof(AccountManagerActor),
                    nameof(GetOrCreateChild),
                    nameof(AccountActor),
                    iban);
            }

            return child;
        }
    }
}
