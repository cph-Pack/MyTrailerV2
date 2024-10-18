using System;
using System.Formats.Asn1;
using System.Net.Sockets;
using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static MongoDB.Driver.WriteConcern;

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

        public Bill insertBill(Rental rental)
        {
            Bill bill = new Bill(rental.Customer, rental);
            bill.RentaldId = rental.RentalId;
            if (rental.HasInsurance)
            {
                bill.TotalAmount += 50;
            }

            var rented = rental.StartTime;
            var rentEnd = DateTime.Now;

            double dif = (rented.Date - rentEnd.Date).TotalDays;
            if(dif < 0)
            {
                bill.TotalAmount += 50;
            }

            _billColl.InsertOne(bill);
            makeTrailerAvailable(rental.Trailer.TrailerNumber);
            makeRentalInactive(rental.RentalId);
            return bill;

            /* Example of a valid object to put into swagger. 
             * Removed all the crap that comes with swagger and mongodb
              
              {
                  "rentalId": "6710cef6a129f459a18e5dc6",
                  "trailer": {
                    "trailerNumber": 1,
                    "isAvailable": true,
                    "rentedUntil": "2024-10-17T23:35:05.478Z"
                  },
                  "customer": {
                    "firstName": "string",
                    "lastName": "string",
                    "email": "string"
                  },
                  "startTime": "2024-10-17T23:35:05.478Z",
                  "rentalType": 0,
                  "hasInsurance": true,
                  "isActive": true
               }
             */
        }

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


        public List<Rental> getActiveRentalsByEmail(string email)
        {
            List<Rental> rentals = _rentalColl.Find<Rental>(ele => ele.Customer.Email == email && ele.IsActive == true).ToList();
            foreach (var rental in rentals)
            {
                rental.RentalId = rental.Id.ToString();
            }
            return rentals;
        }

        public void insertRental(RentalRequest rentalRequest)
        {
            Trailer trailer = getTrailerByNumber(rentalRequest.Trailernumber);
            Customer customer = getCustomerByEmail(rentalRequest.Email);
            Rental rental = new Rental(trailer, customer, DateTime.Now, rentalRequest.Rentaltype, rentalRequest.HasInsurance);
            _rentalColl.InsertOne(rental);
        }

        public Rental getRentalById(string id)
        {
            var objectId = new ObjectId(id);
            var filter = Builders<Rental>.Filter.Eq(r => r.Id, objectId);
            Rental rental = _rentalColl.Find<Rental>(filter).FirstOrDefault();
            if(rental == null)
            {
                throw new InvalidDataException("Could not find a rental with that id");
            }
            rental.RentalId = rental.Id.ToString();
            return rental;
        }

        public void makeTrailerAvailable(int trailerNumber)
        {
            var filter = Builders<Trailer>.Filter.Eq(ele => ele.TrailerNumber, trailerNumber);
            var update = Builders<Trailer>.Update.Set(ele => ele.IsAvailable, true);
            _trailerColl.UpdateOne(filter, update);
        }

        public List<Bill> getAllBillsByEmail(string email)
        {
            List<Bill> bills = _billColl.Find(ele => ele.Customer.Email == email).ToList();
            if(bills.Count == 0)
            {
                throw new InvalidDataException("No bills found matching this email");
            }
            return bills;
        }

        public void makeRentalInactive(string rentalId)
        {
            var filter = Builders<Rental>.Filter.Eq(ele => ele.RentalId, rentalId);
            var update = Builders<Rental>.Update.Set(ele => ele.IsActive, false);
        }

        public Bill getBillByRentalId(string rentalId)
        {
            Bill bill = _billColl.Find(ele => ele.RentaldId == rentalId).FirstOrDefault();
            if(bill == null)
            {
                throw new InvalidDataException("No bill found with that rentalId");
            }
            return bill;
        }

    }
}


