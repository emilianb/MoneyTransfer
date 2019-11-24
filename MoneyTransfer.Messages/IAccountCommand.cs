namespace MoneyTransfer.Messages
{
    public interface IAccountCommand
    {
        string Iban { get; }
    }
}
