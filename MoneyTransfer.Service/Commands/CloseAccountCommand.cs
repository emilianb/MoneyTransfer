namespace MoneyTransfer.Service.Commands
{
    public class CloseAccountCommand
        : IAccountCommand
    {
        public string Iban { get; private set; }

        public static CloseAccountCommand Create(string iban)
            => new CloseAccountCommand
            {
                Iban = iban
            };
    }
}
