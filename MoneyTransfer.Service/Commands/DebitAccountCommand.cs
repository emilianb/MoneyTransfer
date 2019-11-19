namespace MoneyTransfer.Service.Commands
{
    public class DebitAccountCommand
        : IAccountCommand
    {
        public string Iban { get; private set; }

        public decimal Amount { get; private set; }

        public string CurrencyCode { get; private set; }

        public static DebitAccountCommand Create(string iban, decimal amount, string currencyCode)
            => new DebitAccountCommand
            {
                Iban = iban,
                Amount = amount,
                CurrencyCode = currencyCode
            };
    }
}
