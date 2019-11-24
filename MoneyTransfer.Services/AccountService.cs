using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

using MoneyTransfer.Entities;

namespace MoneyTransfer
{
    public class AccountService
        : IAccountService
    {
        public AccountService(string connectionString)
        {
            var url = MongoUrl.Create(connectionString);
            var client = new MongoClient(url);

            Accounts = client.GetDatabase(url.DatabaseName)
                .GetCollection<Account>("Accounts");
        }

        private IMongoCollection<Account> Accounts { get; }

        public void Add(Account account)
            => Accounts.InsertOne(account);

        public Account GetById(string id)
            => Accounts.Find(account => account.Id == id).FirstOrDefault();

        public Account GetByIban(string iban)
            => Accounts.Find(account => account.Iban == iban).FirstOrDefault();

        public List<Account> GetAll()
            => Accounts.Find(account => true).ToList();

        public void UpdateAmount(string iban, decimal amount)
        {
            var definition = Builders<Account>.Update
                .Set(a => a.Amount, GetByIban(iban).Amount + amount);

            Accounts.FindOneAndUpdate(a => a.Iban == iban, definition);
        }

        public void Remove(string iban)
            => Accounts.FindOneAndDelete(a => a.Iban == iban);
    }
}
