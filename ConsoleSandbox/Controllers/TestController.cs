using Microsoft.AspNetCore.Mvc;

namespace ConsoleSandbox.Controllers
{
    public class TestController : Controller
    {
        // GET
        [Route("/test")]
        public IActionResult Index()
        {
            return View();
        }
    }
}