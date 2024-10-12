using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MyTrailerV2.Data
{
    public class Bill
    {
        [BsonId]
        public ObjectId Id { get; set; }

        public Rental rental { get; set; }
        public Customer Customer { get; private set; }
        public int TotalAmount { get; private set; } = 0;
        public DateTime BillingDate { get; private set; } = DateTime.Now;

        public Bill(Customer customer, Rental rental)
        {
            Customer = customer;
        }

        public void AddLateFee(int amount)
        {
            TotalAmount += amount;
        }

        public void AddInsurance()
        {
            TotalAmount += 50;
        }
    }
}
