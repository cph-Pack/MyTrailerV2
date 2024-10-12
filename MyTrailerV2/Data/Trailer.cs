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
            TrailerNumber = trailerNumber;
            RentedUntil = DateTime.Today.AddDays(1);
        }

        public void RentTrailer(DateTime endTime)
        {
            if (!IsAvailable)
                throw new InvalidOperationException("Trailer is already rented.");

            IsAvailable = false;
            RentedUntil = endTime;
        }

        public void ReturnTrailer()
        {
            IsAvailable = true;
            RentedUntil = null;
        }

        public bool IsLateReturn()
        {
            return RentedUntil.HasValue && RentedUntil.Value < DateTime.Now;
        }
    }
}
