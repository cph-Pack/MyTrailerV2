using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MyTrailerV2.Data
{
    public class Customer
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

    }
}
