using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MyTrailerV2.Data
{
    public class Trailer
    {
        [BsonId]
        public ObjectId Id { get; private set; }
        public int TrailerNumber { get; private set; }
        public bool IsAvailable { get; private set; } = true;
        public DateTime? RentedUntil { get; private set; }

        public Trailer(int trailerNumber)
        {
            this.TrailerNumber = trailerNumber;
            this.RentedUntil = DateTime.Today.AddDays(1);
        }

    }
}
