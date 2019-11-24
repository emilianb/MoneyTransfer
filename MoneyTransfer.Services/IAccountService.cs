using System.Collections.Generic;

using MoneyTransfer.Entities;

namespace MoneyTransfer
{
    public interface IAccountService
    {
        void Add(Account account);

        Account GetById(string id);

        Account GetByIban(string iban);

        List<Account> GetAll();

        void UpdateAmount(string iban, decimal amount);

        void Remove(string iban);
    }
}
