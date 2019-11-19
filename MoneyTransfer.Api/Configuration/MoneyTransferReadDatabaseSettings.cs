namespace MoneyTransfer.Api.Configuration
{
    public class MoneyTransferReadDatabaseSettings
        : IMoneyTransferReadDatabaseSettings
    {
        public string AccountsCollectionName { get; set; }

        public string ConnectionString { get; set; }

        public string DatabaseName { get; set; }
    }
}
