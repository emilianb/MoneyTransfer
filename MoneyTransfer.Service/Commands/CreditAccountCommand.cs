namespace MoneyTransfer.Service.Commands
{
    public class CreditAccountCommand
        : IAccountCommand
    {
        public string Iban { get; private set; }

        public decimal Amount { get; private set; }

        public string CurrencyCode { get; private set; }

        public static CreditAccountCommand Create(string iban, decimal amount, string currencyCode)
            => new CreditAccountCommand
            {
                Iban = iban,
                Amount = amount,
                CurrencyCode = currencyCode
            };
    }
}
