using System;
using System.Runtime.Serialization;

namespace MoneyTransfer.Messages
{
    public static class AccountFailures
    {
        [Serializable]
        public class CurrencyCodeMismatchException
            : Exception
        {
            public CurrencyCodeMismatchException(string iban, string expectedCurrencyCode, string actualCurrencyCode)
                : base($"Currency code mismatch for account '{iban}'. Expected currency code '{expectedCurrencyCode}' but actual currency code is '{actualCurrencyCode}'.")
            {
                Iban = iban;
                ExpectedCurrencyCode = expectedCurrencyCode;
                ActualCurrencyCode = actualCurrencyCode;
            }
            protected CurrencyCodeMismatchException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            { }

            public string Iban { get; }

            public string ExpectedCurrencyCode { get; }

            public string ActualCurrencyCode { get; }
        }

        [Serializable]
        public class InsufficientFundsException
            : Exception
        {
            public InsufficientFundsException(string iban, decimal requestedAmount, decimal availableAmount)
                : base($"Currency code mismatch for account '{iban}'. Requested amount '{requestedAmount}' but available amount is '{availableAmount}'.")
            {
                Iban = iban;
                RequestedAmount = requestedAmount;
                AvailableAmount = availableAmount;
            }

            protected InsufficientFundsException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            { }

            public string Iban { get; }

            public decimal RequestedAmount { get; }

            public decimal AvailableAmount { get; }
        }
    }
}
