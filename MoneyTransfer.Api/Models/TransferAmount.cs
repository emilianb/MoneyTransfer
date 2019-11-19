namespace MoneyTransfer.Api.Models
{
    public class TransferAmount
    {
        public string DebitorIban { get; set; }

        public string CreditorIban { get; set; }

        public decimal Amount { get; set; }

        public string CurrencyCode { get; set; }
    }
}
