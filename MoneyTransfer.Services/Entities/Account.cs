namespace MoneyTransfer.Entities
{
    public class Account
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Iban { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }
    }
}
