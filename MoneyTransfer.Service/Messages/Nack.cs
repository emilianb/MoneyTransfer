namespace MoneyTransfer.Service.Messages
{
    public class Nack
    {
        public Nack(string reason)
        {
            Reason = reason;
        }

        public string Reason { get; set; }
    }
}
