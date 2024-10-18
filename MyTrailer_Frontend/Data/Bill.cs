using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MyTrailer_Frontend.Data
{
    public class Bill
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public Rental Rental { get; set; }
        public string RentaldId { get; set; } = string.Empty;
        public Customer Customer { get; set; }
        public int TotalAmount { get; set; } = 0;
        public DateTime BillingDate { get; set; } = DateTime.Now;

        public Bill(Customer customer, Rental rental)
        {
            this.Customer = customer;
            this.Rental = rental;
        }


    }
}
