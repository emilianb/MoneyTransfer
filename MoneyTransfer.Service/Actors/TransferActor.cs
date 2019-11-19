using Akka.Actor;
using Akka.Event;
using System;

using MoneyTransfer.Service.Commands;
using MoneyTransfer.Service.Messages;

namespace MoneyTransfer.Service.Actors
{
    public class TransferActor
        : ReceiveActor
    {
        public TransferActor()
        {
            SetReceiveTimeout(TimeSpan.FromSeconds(30));

            ReceiveAsync<TransferAmountCommand>(async command =>
            {
                var selection = Context.ActorSelection("/user/AccountManager*");
                var response = await selection.Ask(command.DebitAccountCommand, TimeSpan.FromSeconds(10));

                if (response is Ack)
                {
                    response = await selection.Ask(command.CreditAccountCommand, TimeSpan.FromSeconds(10));

                    if (response is Ack)
                    {
                        return;
                    }
                }

                Log.Error(
                    "The transfer between '{0}' and '{1}' failed in '{2}'.",
                    command.DebitAccountCommand.Iban,
                    command.CreditAccountCommand.Iban,
                    nameof(TransferActor));
            });

            Receive<ReceiveTimeout>(message => {
                Log.Info(
                    "Received timeout in '{0}'. Shutting down...",
                    nameof(TransferActor));

                Context.Stop(Context.Self);
            });
        }

        private ILoggingAdapter Log
            => Context.GetLogger();
    }
}
