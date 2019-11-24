using Akka.Actor;
using Akka.Event;
using System;
using System.Threading.Tasks;

using MoneyTransfer.Messages;

namespace MoneyTransfer.Actors
{
    public class TransferActor
        : ReceiveActor
    {
        public TransferActor()
        {
            SetReceiveTimeout(TimeSpan.FromSeconds(30));

            ReceiveAsync<TransferCommand>(HandleTransfer, c => true);
            Receive<ReceiveTimeout>(HandleReceiveTimeout, c => true);
        }

        private ILoggingAdapter Log
            => Context.GetLogger();

        protected override void Unhandled(object message)
            => Log.Warning(
                "[Actor: {0}][Method: {1}][Message: {2}] Unhandled message.",
                nameof(TransferActor),
                nameof(Unhandled),
                message.GetType().FullName);

        private async Task HandleTransfer(TransferCommand command)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Transfer {3} {4} from {5} to {6}.",
                nameof(TransferActor),
                nameof(HandleTransfer),
                command.GetType().FullName,
                command.DebitAccountCommand.Amount,
                command.DebitAccountCommand.CurrencyCode,
                command.DebitAccountCommand.Iban,
                command.CreditAccountCommand.Iban);

            var selection = Context.ActorSelection("/user/AccountManager*");
            var response = await selection.Ask(command.DebitAccountCommand, TimeSpan.FromSeconds(10));

            if (response is Status.Success)
            {
                response = await selection.Ask(command.CreditAccountCommand, TimeSpan.FromSeconds(10));

                if (response is Status.Failure)
                {
                    var reverse = new AccountCommands.Credit(
                        command.DebitAccountCommand.Iban,
                        command.DebitAccountCommand.Amount,
                        command.DebitAccountCommand.CurrencyCode);

                    selection.Tell(reverse);
                }
            }
        }

        private void HandleReceiveTimeout(ReceiveTimeout command)
        {
            Log.Debug(
                "[Actor: {0}][Method: {1}][Message: {2}] Shutting down.",
                nameof(TransferActor),
                nameof(HandleReceiveTimeout),
                command.GetType().FullName);

            Context.Stop(Context.Self);
        }
    }
}
