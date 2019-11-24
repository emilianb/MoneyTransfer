using Akka.Actor;
using Akka.Event;
using System;

namespace MoneyTransfer.Actors
{
    public class GreetingActor
        : ReceiveActor
    {
        public GreetingActor()
        {
            SetReceiveTimeout(TimeSpan.FromSeconds(3));

            Receive<string>(HandleGreeting, m => true);
            Receive<ReceiveTimeout>(HandleReceiveTimeout, m => true);
        }

        private ILoggingAdapter Log
            => Context.GetLogger();

        protected override void Unhandled(object message)
            => Log.Warning(
                "[Actor: {0}][Method: {1}][Message: {2}] Unhandled message.",
                nameof(GreetingActor),
                nameof(Unhandled),
                message.GetType().FullName);

        private void HandleGreeting(string message)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Greeting {3}.",
                nameof(GreetingActor),
                nameof(HandleReceiveTimeout),
                message.GetType().FullName);

            Sender.Tell($"Hello, {message}");
        }

        private void HandleReceiveTimeout(ReceiveTimeout message)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Shutting down.",
                nameof(GreetingActor),
                nameof(HandleReceiveTimeout),
                message.GetType().FullName);

            Context.Stop(Context.Self);
        }
    }
}
