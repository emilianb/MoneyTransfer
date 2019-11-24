namespace MoneyTransfer.Api.Models
{
    public class Transfer
    {
        public string DebitorIban { get; set; }

        public string CreditorIban { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }
    }
}
