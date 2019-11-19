using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

using MoneyTransfer.Api.Configuration;
using MoneyTransfer.Service.Models;

namespace MoneyTransfer.Api.Services
{
    public class AccountService
    {
        public AccountService(IMoneyTransferReadDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Accounts = database.GetCollection<Account>(settings.AccountsCollectionName);
        }

        private IMongoCollection<Account> Accounts { get; }

        public Account GetById(string id)
            => Accounts.Find(account => account.Id == id).FirstOrDefault();

        public List<Account> GetAll()
            => Accounts.Find(account => true).ToList();
    }
}
