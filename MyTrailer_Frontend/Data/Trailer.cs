using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MyTrailer_Frontend.Data
{
    public class Trailer
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public int TrailerNumber { get; set; }
        public bool IsAvailable { get; set; } = true;
        public DateTime? RentedUntil { get; set; }

        public Trailer(int trailerNumber)
        {
            this.TrailerNumber = trailerNumber;
            this.RentedUntil = DateTime.Today.AddDays(1);
        }

    }
}
