using Akka.Actor;
using Akka.Event;

using MoneyTransfer.Service.Commands;

namespace MoneyTransfer.Service.Actors
{
    public class TransferManagerActor
        : ReceiveActor
    {
        public TransferManagerActor()
        {
            Receive<TransferAmountCommand>(command =>
            {
                Log.Info(
                    "Received transfer amount command in '{1}'.",
                    nameof(TransferManagerActor));

                CreateChild(Context).Forward(command);
            });
        }

        private ILoggingAdapter Log
            => Context.GetLogger();

        private IActorRef CreateChild(IActorContext context)
            => context.ActorOf(Props.Create(() => new TransferActor()));
    }
}
