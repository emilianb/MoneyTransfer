using Akka.Actor;
using Akka.Event;
using System;

namespace MoneyTransfer.Service.Actors
{
    public class GreetingActor
        : ReceiveActor
    {
        public GreetingActor()
        {
            SetReceiveTimeout(TimeSpan.FromSeconds(3));

            Receive<string>(name => {
                Log.Info("Received name '{0}' in '{1}'", name, nameof(GreetingActor));

                Sender.Tell($"Hello, {name}");
            });

            Receive<ReceiveTimeout>(message => {
                Log.Info(
                    "Received timeout in '{0}'. Shutting down...",
                    nameof(GreetingActor));

                Context.Stop(Context.Self);
            });
        }

        private ILoggingAdapter Log
            => Context.GetLogger();
    }
}
