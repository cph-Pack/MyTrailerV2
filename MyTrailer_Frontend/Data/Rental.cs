using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MyTrailer_Frontend.Data
{
    public class Rental
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string RentalId { get; set; } = string.Empty;
        public Trailer Trailer { get; set; }
        public Customer Customer { get; set; }
        public DateTime StartTime { get; set; }
        public RentalType RentalType { get; set; }
        public bool HasInsurance { get; set; }
        public bool IsActive { get; set; } = true;

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
