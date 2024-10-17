using Microsoft.AspNetCore.Mvc;
using MyTrailerV2.Data;

namespace MyTrailerV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyTrailerController : Controller
    {
        DBManager _dbManager = new DBManager();

        [HttpPost("trailer/add")]
        public ActionResult<Trailer> addTrailer([FromBody] Trailer trailer)
        {
            try
            {
                _dbManager.insertTrailer(trailer);
                Trailer result = _dbManager.getTrailerByNumber(trailer.TrailerNumber);
                return Ok(result);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("trailer/{trailernumber}")]
        public ActionResult<Trailer> getTrailer(int trailernumber)
        {
            try
            {
                Trailer trailer = _dbManager.getTrailerByNumber(trailernumber);
                return Ok(trailer);
            }
            catch (InvalidDataException e)
            {
                return NotFound(e.Message);
            }
        }

        //public void getTrailers()
        //{

        //}

        [HttpPost("rental/add")]
        public ActionResult addRental([FromBody] RentalRequest rentalRequest)
        {
            try
            {
                _dbManager.insertRental(rentalRequest);
                return Ok();
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }
            
        }

        [HttpGet("rental/{email}")]
        public ActionResult<List<Rental>> getRental(string email)
        {
            try
            {
                List<Rental> rental = _dbManager.getRentalByEmail(email);
                return Ok(rental);
            }
            catch (InvalidDataException e)
            {
                return NotFound(e.Message);
            }
            
        }

        //public void getBill()
        //{

        //}

        //public void addBill()
        //{

        //}

        //[HttpPost("")]
        //public ActionResult<Bill> endRentalCreateBill()
        //{

        //}

        [HttpPost("customer/add")]
        public ActionResult<Customer> addCustomer([FromBody] Customer customer)
        {
            try
            {
                _dbManager.insertCustomer(customer);
                Customer result = _dbManager.getCustomerByEmail(customer.Email);
                return Ok(result);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("customer/{email}")]
        public ActionResult<Customer> getCustomer(string email) 
        {
            try
            {
                Customer customer = _dbManager.getCustomerByEmail(email);
                return Ok(customer);
            }
            catch (InvalidDataException e)
            {
                return NotFound(e.Message);
            }
           
        }
    }
}
