using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

using MoneyTransfer.Entities;

namespace MoneyTransfer.Configuration.Mappings
{
    public class BsonMapper
        : IHasBsonMappings
    {
        public void Register()
        {
            BsonClassMap.RegisterClassMap<Account>(m =>
            {
                m.MapIdMember(c => c.Id)
                    .SetIdGenerator(StringObjectIdGenerator.Instance)
                    .SetSerializer(new StringSerializer(BsonType.ObjectId));

                m.MapMember(c => c.FirstName)
                    .SetElementName("first_name");

                m.MapMember(c => c.LastName)
                    .SetElementName("last_name");

                m.MapMember(c => c.Iban)
                    .SetElementName("iban");

                m.MapMember(c => c.Amount)
                    .SetElementName("amount");

                m.MapMember(c => c.CurrencyCode)
                    .SetElementName("currency_code");
            });
        }
    }
}
