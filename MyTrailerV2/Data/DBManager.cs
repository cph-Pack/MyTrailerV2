using System.Formats.Asn1;
using System.Net.Sockets;
using System.Xml.Linq;
using MongoDB.Driver;

namespace MyTrailerV2.Data
{
    public class DBManager
    {
        private const string connectionUri = "mongodb://localhost:27017";
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _database;

        private readonly IMongoCollection<Trailer> _trailerColl;
        private readonly IMongoCollection<Rental> _rentalColl;
        private readonly IMongoCollection<Bill> _billColl;
        private readonly IMongoCollection<Customer> _customerColl;

        public DBManager()
        {
            _client = new MongoClient(connectionUri);
            _database = _client.GetDatabase("SI");

            _trailerColl = _database.GetCollection<Trailer>("trailers");
            _customerColl = _database.GetCollection<Customer>("customers");
            _rentalColl = _database.GetCollection<Rental>("rentals");
            _billColl = _database.GetCollection<Bill>("bills");
        }

        public void insertBill(Bill bill)
        {
            _billColl.InsertOne(bill);
        }

        public void InsertCustomer(Customer customer)
        {
            bool isUnique = IsCustomerUnique(customer);
            if (!isUnique)
            {
                throw new InvalidDataException("A customer with that email already exists");
            }
                _customerColl.InsertOne(customer);
        }

        public bool IsCustomerUnique(Customer customer) 
        {
            bool isUnique = false;
            Customer result = GetCustomer(customer.Email);
            if(result == null)
            {
                isUnique = true;
            }
            return isUnique;
        }

        public Customer GetCustomer(string email)
        {
            Customer customer = _customerColl.Find<Customer>(ele => ele.Email == email).FirstOrDefault();
            return customer;
        }

        public Rental GetRental(string email)
        {
            Rental rental = _rentalColl.Find<Rental>(ele => ele.Customer.Email == email).FirstOrDefault();
            return rental;
        }

        public Bill addBill(Rental rental)
        {
            Bill bill = new Bill(rental.Customer, rental);
            _billColl.InsertOne(bill);
            return bill;
        }

        //public void InsertTrailer(Trailer trailer)
        //{
        //    _trailerColl.InsertOne(trailer);
        //}

        //public List<Trailer> GetAllTrailers()
        //{
        //    return _trailerColl.Find<Trailer>(_ => true).ToList();
        //}

        //public void InsertRental(Rental rental)
        //{
        //    _rentalColl.InsertOne(rental);
        //}

        //public Trailer findTrailerByTrailerNumber(int trailerNumber)
        //{
        //    return _trailerColl.Find<Trailer>(t => t.TrailerNumber == trailerNumber).FirstOrDefault();
        //}
    }
}


