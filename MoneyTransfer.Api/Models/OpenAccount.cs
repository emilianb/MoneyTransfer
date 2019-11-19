namespace MoneyTransfer.Api.Models
{
    public class OpenAccount
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Iban { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }
    }
}
