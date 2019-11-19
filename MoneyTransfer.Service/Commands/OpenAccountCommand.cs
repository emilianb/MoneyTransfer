namespace MoneyTransfer.Service.Commands
{
    public class OpenAccountCommand
        : IAccountCommand
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Iban { get; private set; }

        public decimal Amount { get; private set; }

        public string CurrencyCode { get; private set; }

        public static OpenAccountCommand Create(string firstName, string lastName, string iban, decimal amount, string currencyCode)
            => new OpenAccountCommand
            {
                FirstName = firstName,
                LastName = lastName,
                Iban = iban,
                Amount = amount,
                CurrencyCode = currencyCode
            };
    }
}
