using Akka.Actor;
using Akka.Event;

using MoneyTransfer.Messages;

namespace MoneyTransfer.Actors
{
    public class TransferManagerActor
        : ReceiveActor
    {
        public TransferManagerActor()
        {
            Receive<TransferCommand>(HandleTransferCommand, c => true);
        }

        private ILoggingAdapter Log
            => Context.GetLogger();

        protected override void Unhandled(object message)
            => Log.Warning(
                "[Actor: {0}][Method: {1}][Message: {2}] Unhandled message.",
                nameof(TransferManagerActor),
                nameof(Unhandled),
                message.GetType().FullName);

        public void HandleTransferCommand(TransferCommand command)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Forwarding message.",
                nameof(TransferManagerActor),
                nameof(HandleTransferCommand),
                command.GetType().FullName);

            CreateChild(Context).Forward(command);
        }

        private IActorRef CreateChild(IActorContext context)
            => context.ActorOf(Props.Create(() => new TransferActor()));
    }
}
