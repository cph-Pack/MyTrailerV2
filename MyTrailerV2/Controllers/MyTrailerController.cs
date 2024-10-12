using Microsoft.AspNetCore.Mvc;
using MyTrailerV2.Data;

namespace MyTrailerV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyTrailerController : Controller
    {
        DBManager _dbManager = new DBManager();

        //public void addTrailer()
        //{

        //}

        //public void getTrailers()
        //{

        //}

        //public void addRental()
        //{

        //}

        //public void getRental()
        //{

        //}

        //public void getBill()
        //{

        //}

        //public void addBill()
        //{

        //}

        //public void endRentalCreateBill()
        //{

        //}

        [HttpPost("customer/add")]
        public ActionResult<Customer> postCustomer([FromBody] Customer customer)
        {
            try
            {
                _dbManager.InsertCustomer(customer);
                Customer result = _dbManager.GetCustomer(customer.Email);
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
                Customer customer = _dbManager.GetCustomer(email);
                if (customer == null)
                {
                    throw new InvalidDataException("No customer found with that email");
                }
                return Ok(customer);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }
           
        }
    }
}
