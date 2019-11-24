namespace MoneyTransfer.Messages
{
    public static class AccountEvents
    {
        #region Opened

        public sealed class Opened
        {
            public Opened(string firstName, string lastName, string iban, decimal amount, string currencyCode)
            {
                FirstName = firstName;
                LastName = lastName;
                Iban = iban;
                Amount = amount;
                CurrencyCode = currencyCode;
            }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Iban { get; set; }

            public decimal Amount { get; set; }

            public string CurrencyCode { get; set; }
        }

        #endregion Opened

        #region Debited

        public sealed class Debited
        {
            public Debited(string iban, decimal amount, string currencyCode)
            {
                Iban = iban;
                Amount = amount;
                CurrencyCode = currencyCode;
            }

            public string Iban { get; private set; }

            public decimal Amount { get; private set; }

            public string CurrencyCode { get; private set; }
        }

        #endregion Debited

        #region Credited

        public sealed class Credited
        {
            public Credited(string iban, decimal amount, string currencyCode)
            {
                Iban = iban;
                Amount = amount;
                CurrencyCode = currencyCode;
            }

            public string Iban { get; private set; }

            public decimal Amount { get; private set; }

            public string CurrencyCode { get; private set; }
        }

        #endregion Credited

        #region Closed

        public sealed class Closed
        {
            public Closed(string iban)
            {
                Iban = iban;
            }

            public string Iban { get; set; }
        }

        #endregion Closed
    }
}
