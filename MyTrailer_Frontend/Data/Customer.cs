using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MyTrailer_Frontend.Data
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
