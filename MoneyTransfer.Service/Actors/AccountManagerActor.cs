using Akka.Actor;
using Akka.Event;

using MoneyTransfer.Service.Commands;

namespace MoneyTransfer.Service.Actors
{
    public class AccountManagerActor
        : ReceiveActor, IWithUnboundedStash
    {
        public AccountManagerActor()
        {
            Become(Uninitialized);
        }

        private ILoggingAdapter Log
            => Context.GetLogger();

        public IStash Stash { get; set; }

        private void Uninitialized()
            => ReceiveAny(_ => Stash.Stash());

        void Initialized()
        {
            Receive<IAccountCommand>(HandleAccountCommand, m => true);

            Stash.UnstashAll();
        }

        void HandleAccountCommand(IAccountCommand command)
            => GetOrCreateChild(Context, command.Iban).Forward(command);

        // Initialize children here.
        protected override void PreStart()
            => Become(Initialized);

        protected override void Unhandled(object command)
            => Log.Warning(
                "Unhandled command '{0}' in '{1}'.",
                command.GetType().FullName,
                nameof(AccountManagerActor));

        private IActorRef GetOrCreateChild(IActorContext context, string iban)
        {
            var childName = $"Account-{iban.ToUpperInvariant()}";
            var child = Context.Child(childName);

            if (child.Equals(ActorRefs.Nobody))
            {
                var childProps = Props.Create(() => new AccountActor(iban.ToUpperInvariant()));

                child = context.ActorOf(childProps, childName);

                Log.Debug(
                    "Created '{0}' in '{1}' for iban '{2}'.",
                    nameof(AccountActor),
                    nameof(AccountManagerActor),
                    iban);
            }

            return child;
        }
    }
}
