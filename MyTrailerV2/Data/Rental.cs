using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyTrailerV2.Data
{
    public class Rental
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public Trailer Trailer { get; set; }
        public Customer Customer { get; set; }
        public DateTime StartTime { get; set; }
        public RentalType RentalType { get; set; }
        public bool HasInsurance { get; set; }

        public Rental(Trailer trailer, Customer customer, DateTime startTime, RentalType rentalType, bool hasInsurance)
        {

            this.Trailer = trailer;
            this.Customer = customer;
            this.StartTime = startTime;
            this.RentalType = rentalType;
            this.HasInsurance = hasInsurance;
        }

    }
}
