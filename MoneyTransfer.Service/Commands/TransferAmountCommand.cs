namespace MoneyTransfer.Service.Commands
{
    public class TransferAmountCommand
        : ITransferAmountCommand
    {
        public DebitAccountCommand DebitAccountCommand { get; private set; }

        public CreditAccountCommand CreditAccountCommand { get; private set; }

        public static TransferAmountCommand Create(string debitorIban, string creditorIban, decimal amount, string currencyCode)
            => new TransferAmountCommand
            {
                DebitAccountCommand = DebitAccountCommand.Create(debitorIban, amount, currencyCode),
                CreditAccountCommand = CreditAccountCommand.Create(creditorIban, amount, currencyCode)
            };
    }
}
