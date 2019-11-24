using Akka.Actor;
using Akka.Event;

namespace MoneyTransfer.Actors
{
    public class GreetingManagerActor
        : ReceiveActor
    {
        public GreetingManagerActor()
        {
            Receive<string>(HandleGreetingMessage, m => true);
        }

        private ILoggingAdapter Log
            => Context.GetLogger();

        protected override void Unhandled(object message)
            => Log.Warning(
                "[Actor: {0}][Method: {1}][Message: {2}] Unhandled message.",
                nameof(GreetingManagerActor),
                nameof(Unhandled),
                message.GetType().FullName);

        public void HandleGreetingMessage(string message)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Forwarding message {3}.",
                nameof(GreetingManagerActor),
                nameof(HandleGreetingMessage),
                message.GetType().FullName,
                message);

            CreateChild(Context).Forward(message);
        }

        private IActorRef CreateChild(IActorContext context)
            => context.ActorOf(Props.Create(() => new GreetingActor()));
    }
}
