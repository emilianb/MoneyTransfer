using Akka.Actor;
using Akka.Event;
using MongoDB.Driver;
using System.Linq;

using MoneyTransfer.Service.Models;
using MoneyTransfer.Service.Events;

namespace MoneyTransfer.Service.Actors
{
    public class MaterializeViewActor
        : ReceiveActor
    {
        public MaterializeViewActor()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            var database = client.GetDatabase("MoneyTransferReadDb");

            Accounts = database.GetCollection<Account>("Accounts");

            Receive<AccountOpenedEvent>(@event => {
                Accounts.InsertOne(new Account()
                {
                    FirstName = @event.FirstName,
                    LastName = @event.LastName,
                    Iban = @event.Iban,
                    Amount = @event.Amount,
                    CurrencyCode = @event.CurrencyCode,
                });
            });

            Receive<AccountDebitedEvent>(@event => {
                var account = Accounts.Find(a => a.Iban == @event.Iban).FirstOrDefault();

                var amount = account.Amount - @event.Amount;

                var update = Builders<Account>.Update.Set(s => s.Amount, amount);

                Accounts.FindOneAndUpdate(a => a.Iban == @event.Iban, update);
            });

            Receive<AccountCreditedEvent>(@event => {
                var account = Accounts.Find(a => a.Iban == @event.Iban).FirstOrDefault();

                var amount = account.Amount + @event.Amount;

                var update = Builders<Account>.Update.Set(s => s.Amount, amount);

                Accounts.FindOneAndUpdate(a => a.Iban == @event.Iban, update);
            });

            Receive<AccountClosedEvent>(@event => {
                Accounts.FindOneAndDelete(a => a.Iban == @event.Iban);
            });
        }

        private IMongoCollection<Account> Accounts { get; }

        private ILoggingAdapter Log
            => Context.GetLogger();

        protected override void Unhandled(object command)
            => Log.Warning(
                "Unhandled command '{0}' in '{1}'.",
                command.GetType().FullName,
                nameof(AccountManagerActor));
    }
}
