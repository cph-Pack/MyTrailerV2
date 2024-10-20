﻿using System.Formats.Asn1;
using System.Net.Sockets;
using System.Xml.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MyTrailer_Frontend.Data
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

        //public Rental getRental(string email)
        //{
        //    Rental rental = _rentalColl.Find<Rental>(ele => ele.Customer.Email == email).FirstOrDefault();
        //    return rental;
        //}

        public Bill addBill(Rental rental)
        {
            Bill bill = new Bill(rental.Customer, rental);
            bill.RentaldId = rental.RentalId;
            if (rental.HasInsurance)
            {
                bill.TotalAmount += 50;
            }

            var a = rental.StartTime;
            var b = DateTime.Now;


            double dif = (a.Date - b.Date).TotalDays;
            if (dif < 0)
            {
                bill.TotalAmount += 50;
            }


            /*
             * 
             * {
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
             * 
             * 
             * */

            _billColl.InsertOne(bill);
            return bill;
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
                isUnique = true;
            }
            return isUnique;
        }

        public Trailer getTrailerByNumber(int number)
        {
            Trailer trailer = _trailerColl.Find<Trailer>(ele => ele.TrailerNumber == number).FirstOrDefault();
            if (trailer == null)
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
            if (rental == null)
            {
                throw new InvalidDataException("Could not find a rental with that id");
            }
            rental.RentalId = rental.Id.ToString();
            return rental;
        }


    }
}


