using Microsoft.AspNetCore.Mvc;

namespace MyTrailerV2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyTrailerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
