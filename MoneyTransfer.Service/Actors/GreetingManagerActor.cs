using Akka.Actor;
using Akka.Event;

namespace MoneyTransfer.Service.Actors
{
    public class GreetingManagerActor
        : ReceiveActor
    {
        public GreetingManagerActor()
        {
            Receive<string>(name =>
            {
                Log.Info(
                    "Received name '{0}' in '{1}'.",
                    name,
                    nameof(GreetingManagerActor));

                CreateChild().Forward(name);
            });
        }

        private ILoggingAdapter Log
            => Context.GetLogger();

        private IActorRef CreateChild()
            => Context.ActorOf(Props.Create(() => new GreetingActor()));
    }
}
