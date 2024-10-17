using System.Formats.Asn1;
using System.Net.Sockets;
using System.Xml.Linq;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        //public void insertBill(Bill bill)
        //{
        //    _billColl.InsertOne(bill);
        //}

        public void insertCustomer(Customer customer)
        {
            bool isUnique = isCustomerUnique(customer);
            if (!isUnique)
            {
                throw new InvalidDataException("A customer with that email already exists");
            }
            _customerColl.InsertOne(customer);
        }

        public bool isCustomerUnique(Customer customer) 
        {
            bool isUnique = false;
            Customer result = _customerColl.Find<Customer>(ele => ele.Email == customer.Email).FirstOrDefault();
            if (result == null)
            {
                isUnique = true;
            }
            return isUnique;
        }

        public Customer getCustomerByEmail(string email)
        {
            Customer customer = _customerColl.Find<Customer>(ele => ele.Email == email).FirstOrDefault();
            if (customer == null)
            {
                throw new InvalidDataException("No customer found with that email");
            }
            return customer;
        }

        //public Rental getRental(string email)
        //{
        //    Rental rental = _rentalColl.Find<Rental>(ele => ele.Customer.Email == email).FirstOrDefault();
        //    return rental;
        //}

        //public Bill addBill(Rental rental)
        //{
        //    Bill bill = new Bill(rental.Customer, rental);
        //    _billColl.InsertOne(bill);
        //    return bill;
        //}

        public void insertTrailer(Trailer trailer)
        {
            bool isUnique = isTrailerUnique(trailer);
            if (!isUnique)
            { 
                throw new InvalidDataException("A trailer with that number already exists");
            }
            _trailerColl.InsertOne(trailer);
        }

        public bool isTrailerUnique(Trailer trailer)
        {
            bool isUnique = false;
            Trailer result = _trailerColl.Find<Trailer>(ele => ele.TrailerNumber == trailer.TrailerNumber).FirstOrDefault();
            if (result == null)
            {
                isUnique= true;
            }
            return isUnique;
        }

        public Trailer getTrailerByNumber(int number)
        {
            Trailer trailer = _trailerColl.Find<Trailer>(ele => ele.TrailerNumber == number).FirstOrDefault();
            if(trailer == null)
            {
                throw new InvalidDataException("No trailer found matching the number provided");
            }
            return trailer;
        }

        //public List<Trailer> GetAllTrailers()
        //{
        //    return _trailerColl.Find<Trailer>(_ => true).ToList();
        //}

        public List<Rental> getRentalByEmail(string email)
        {
            List<Rental> rental = _rentalColl.Find<Rental>(ele => ele.Customer.Email == email && ele.IsActive == true).ToList();
            return rental;
        }

        public void insertRental(RentalRequest rentalRequest)
        {
            Trailer trailer = getTrailerByNumber(rentalRequest.Trailernumber);
            Customer customer = getCustomerByEmail(rentalRequest.Email);
            Rental rental = new Rental(trailer, customer, DateTime.Now, rentalRequest.Rentaltype, rentalRequest.HasInsurance);
            _rentalColl.InsertOne(rental);
        }

        //public Trailer findTrailerByTrailerNumber(int trailerNumber)
        //{
        //    return _trailerColl.Find<Trailer>(t => t.TrailerNumber == trailerNumber).FirstOrDefault();
        //}
    }
}


