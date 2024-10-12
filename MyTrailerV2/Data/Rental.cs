using MongoDB.Bson.Serialization.Attributes;

namespace MyTrailerV2.Data
{
    public class Rental
    {
        [BsonId]
        public ObjectId Id { get; private set; }
        public Trailer Trailer { get; private set; }
        public Customer Customer { get; private set; }
        public DateTime StartTime { get; private set; }
        public DateTime EndTime { get; private set; }
        public bool HasInsurance { get; private set; }

        public Rental(Trailer trailer, Customer customer, DateTime startTime, DateTime endTime, bool hasInsurance)
        {

            Trailer = trailer;
            Trailer.RentTrailer(endTime);
            Customer = customer;
            StartTime = startTime;
            EndTime = endTime;
            HasInsurance = hasInsurance;
        }
        private static bool IsLaterThanMidnight(DateTime endTime)
        {
            return endTime > DateTime.Today.AddDays(1); //midnight
        }
    }
}
