namespace MoneyTransfer.Messages
{
    public static class AccountHelpers
    {
        public static AccountEvents.Opened ToAccountOpenedEvent(this AccountCommands.Open command)
            => new AccountEvents.Opened(
                firstName: command.FirstName,
                lastName: command.LastName,
                iban: command.Iban,
                amount: command.Amount,
                currencyCode: command.CurrencyCode);

        public static AccountEvents.Debited ToAccountDebitedEvent(this AccountCommands.Debit command)
            => new AccountEvents.Debited(
                iban: command.Iban,
                amount: command.Amount,
                currencyCode: command.CurrencyCode);

        public static AccountEvents.Credited ToAccountCreditedEvent(this AccountCommands.Credit command)
            => new AccountEvents.Credited(
                iban: command.Iban,
                amount: command.Amount,
                currencyCode: command.CurrencyCode);

        public static AccountEvents.Closed ToAccountClosedEvent(this AccountCommands.Close command)
            => new AccountEvents.Closed(iban: command.Iban);
    }
}
