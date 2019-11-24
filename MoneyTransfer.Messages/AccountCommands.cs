namespace MoneyTransfer.Messages
{
    public static class AccountCommands
    {
        #region Open

        public sealed class Open
            : IAccountCommand
        {
            public Open(string firstName, string lastName, string iban, decimal amount, string currencyCode)
            {
                FirstName = firstName;
                LastName = lastName;
                Iban = iban;
                Amount = amount;
                CurrencyCode = currencyCode;
            }

            public string FirstName { get; }

            public string LastName { get; }

            public string Iban { get; }

            public decimal Amount { get; }

            public string CurrencyCode { get; }
        }

        #endregion Open

        #region Debit

        public sealed class Debit
            : IAccountCommand
        {
            public Debit(string iban, decimal amount, string currencyCode)
            {
                Iban = iban;
                Amount = amount;
                CurrencyCode = currencyCode;
            }

            public string Iban { get; }

            public decimal Amount { get; }

            public string CurrencyCode { get; }
        }

        #endregion Debit

        #region Credit

        public sealed class Credit
            : IAccountCommand
        {
            public Credit(string iban, decimal amount, string currencyCode)
            {
                Iban = iban;
                Amount = amount;
                CurrencyCode = currencyCode;
            }

            public string Iban { get; }

            public decimal Amount { get; }

            public string CurrencyCode { get; }
        }

        #endregion Credit

        #region Close

        public sealed class Close
            : IAccountCommand
        {
            public Close(string iban)
            {
                Iban = iban;
            }

            public string Iban { get; }
        }

        #endregion Close
    }
}
