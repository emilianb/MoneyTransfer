namespace MoneyTransfer.Api.Configuration
{
    public interface IMoneyTransferReadDatabaseSettings
    {
        string AccountsCollectionName { get; set; }

        string ConnectionString { get; set; }

        string DatabaseName { get; set; }
    }
}
