namespace MoneyTransfer.Messages
{
    public class TransferCommand
    {
        private TransferCommand()
        { }

        public AccountCommands.Debit DebitAccountCommand { get; private set; }

        public AccountCommands.Credit CreditAccountCommand { get; private set; }

        public static TransferCommand Create(string debitorIban, string creditorIban, decimal amount, string currencyCode)
            => new TransferCommand
            {
                DebitAccountCommand = new AccountCommands.Debit(debitorIban, amount, currencyCode),
                CreditAccountCommand = new AccountCommands.Credit(creditorIban, amount, currencyCode)
            };
    }
}
