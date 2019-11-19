using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MoneyTransfer.Service.Models
{
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("first_name")]
        public string FirstName { get; set; }

        [BsonElement("last_name")]
        public string LastName { get; set; }

        [BsonElement("iban")]
        public string Iban { get; set; }

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("currency_code")]
        public string CurrencyCode { get; set; }
    }
}
